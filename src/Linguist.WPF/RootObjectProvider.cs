using System;
using System.Windows;

#if ! NET35
using System.Xaml;
#else
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;
using System.Windows.Markup;
using System.Windows.Media;
using System.Xml;
using System.Xml.Linq;
#endif

namespace Linguist.WPF
{
    internal static class RootObjectProvider
    {
        #if ! NET35
        public static DependencyObject GetRootObject ( IServiceProvider serviceProvider )
        {
            var rootProvider = (IRootObjectProvider) serviceProvider.GetService ( typeof ( IRootObjectProvider ) );
            var root         = rootProvider?.RootObject as DependencyObject;

            return root;
        }
        #else
        private static bool         rootObjectPropertyInitialized;
        private static PropertyInfo rootObjectProperty;

        public static DependencyObject GetRootObject ( IServiceProvider serviceProvider )
        {
            if ( ! rootObjectPropertyInitialized )
            {
                rootObjectProperty = serviceProvider.GetType ( )
                                                    .GetInterface ( "System.Xaml.IRootObjectProvider" )
                                                   ?.GetProperty  ( "RootObject" );

                rootObjectPropertyInitialized = true;
            }

            if ( rootObjectProperty != null )
                return rootObjectProperty.GetValue ( serviceProvider, null ) as DependencyObject;

            var uriContext = (IUriContext) serviceProvider.GetService ( typeof ( IUriContext ) );
            var baseUri    = uriContext.BaseUri;
            var rootType   = GetResourceType ( baseUri );

            var pvt  = (IProvideValueTarget) serviceProvider.GetService ( typeof ( IProvideValueTarget ) );
            var root = pvt.TargetObject as DependencyObject;

            while ( root != null && root.GetType ( ) != rootType )
                root = VisualTreeHelper .GetParent ( root ) ??
                       LogicalTreeHelper.GetParent ( root );

            return root;
        }

        private static Dictionary < Uri, Type > resourceTypeCache;

        private static Type GetResourceType ( Uri packUri )
        {
            if ( resourceTypeCache == null )
                resourceTypeCache = new Dictionary < Uri, Type > ( );

            if ( ! resourceTypeCache.TryGetValue ( packUri, out var type ) )
                resourceTypeCache.Add ( packUri, type = ReadResourceType ( packUri ) );

            return type;
        }

        private static Type ReadResourceType ( Uri packUri )
        {
            var stream = Application.GetResourceStream ( packUri );

            if ( stream.ContentType == "application/baml+xml" )
                return ReadBamlResourceType ( stream.Stream );

            if ( stream.ContentType == "application/xaml+xml" )
                return ReadXamlResourceType ( stream.Stream );

            throw new Exception ( $"'{ stream.ContentType }' ContentType is not valid." );
        }

        private static Type ReadBamlResourceType ( Stream baml )
        {
            const char TypeInfoRecord           = '\u001d';
            const int  TypeInfoRecordNameOffset = 6;

            var buffer = new byte [ 1 ];

            while ( true )
            {
                if ( baml.Read ( buffer, 0, 1 ) == 0 )
                    throw new Exception ( "Could not read x:Class attribute from BAML stream." );

                if ( buffer [ 0 ] == TypeInfoRecord )
                {
                    baml.Seek ( TypeInfoRecordNameOffset, SeekOrigin.Current );

                    var type = new StringBuilder ( );

                    while ( true )
                    {
                        if ( baml.Read ( buffer, 0, 1 ) == 0 )
                            throw new Exception ( "Could not read x:Class attribute from BAML stream." );

                        if ( buffer [ 0 ] < '.' )
                            break;

                        type.Append ( (char) buffer [ 0 ] );
                    }

                    return FindResourceType ( type.ToString ( ) );
                }
            }
        }

        private static Type ReadXamlResourceType ( Stream xaml )
        {
            using ( var reader = XmlReader.Create ( xaml ) )
            {
                var document = XDocument.Load ( reader );
                var type     = document.Root.Attribute ( "{http://schemas.microsoft.com/winfx/2006/xaml}Class" );
                if ( type == null )
                    throw new Exception ( "Could not read x:Class attribute from XAML stream." );

                return FindResourceType ( type.Value );
            }
        }

        private static Type FindResourceType ( string typeName )
        {
            var type = Type.GetType ( typeName );
            if ( type != null )
                return type;

            foreach ( var assembly in AppDomain.CurrentDomain.GetAssemblies ( ) )
            {
                type = assembly.GetType ( typeName );
                if ( type != null )
                    return type;
            }

            throw new Exception ( $"Could not find type { typeName }." );
        }
        #endif
    }
}
using System;
using System.Collections.Generic;
using System.CodeDom;
using System.IO;
using System.Resources;
using System.Xml;
using System.Xml.Linq;

using Linguist.CodeDom;

namespace Linguist.Generator
{
    public static class ResXParser
    {
        public static ResourceSet ExtractResourceSet ( string inputFileName, string inputFileContent )
        {
            var resources = new Dictionary < string, Resource > ( StringComparer.InvariantCultureIgnoreCase );
            var document  = XDocument.Parse ( inputFileContent );

            foreach ( var data in document.Descendants ( "data" ) )
            {
                var name = data.Attribute ( "name" )?.Value;
                if ( name == null )
                    continue;

                var type     = GetType ( data );
                var comment  = data.Element ( "comment" )?.Value;
                var resource = (Resource) null;

                if ( type.BaseType == typeof ( string ).FullName )
                    resource = new StringResource ( type, data.Element ( "value" )?.Value, GetString, comment );
                else if ( type.BaseType == typeof ( MemoryStream          ).FullName ||
                          type.BaseType == typeof ( UnmanagedMemoryStream ).FullName )
                    resource = new Resource ( type, GetStream, comment );
                else
                    resource = new Resource ( type, GetObject ( type ), comment );

                resource.FileName = inputFileName;

                if ( data is IXmlLineInfo info && info.HasLineInfo ( ) )
                {
                    resource.Line   = info.LineNumber;
                    resource.Column = info.LinePosition;
                }

                resources.Add ( name, resource );
            }

            return new ResourceSet ( Code.TypeRef < ResourceManager >                     ( ), ManagerInitializer,
                                     Code.TypeRef < ResourceManagerLocalizationProvider > ( ), ProviderInitializer,
                                     resources );
        }

        private static CodeTypeReference GetType ( XElement data )
        {
            var type = data.Attribute ( "type" );
            if ( type == null )
                return Code.TypeRef < string > ( );

            if ( type.Value == "System.Resources.ResXFileRef, System.Windows.Forms" )
            {
                var value = data.Element ( "value" );
                if ( value != null )
                {
                    var parts    = value.Value.Split ( ';' );
                    var typeName = parts [ parts.Length - 1 ];

                    return Code.TypeRef ( typeName );
                }
            }

            return Code.TypeRef < object > ( );
        }

        private static CodeExpression ManagerInitializer ( string resourcesBaseName, string className )
        {
            return Code.TypeRef < ResourceManager > ( )
                       .Construct ( Code.Constant ( resourcesBaseName ),
                                    Code.TypeRef  ( className, default )
                                        .TypeOf   ( )
                                        .Property ( nameof ( Type.Assembly ) ) );
        }

        private static CodeExpression ProviderInitializer ( CodeExpression resourceManager, CodeExpression resourceNamingStrategy )
        {
            var initializer = Code.TypeRef < ResourceManagerLocalizationProvider > ( )
                                  .Construct ( resourceManager );

            if ( resourceNamingStrategy != null )
                initializer.Parameters.Add ( resourceNamingStrategy );

            return initializer;
        }

        private static CodeExpression GetString ( CodeExpression resourceManager, string name, CodeExpression culture )
        {
            return resourceManager.Method ( nameof ( ResourceManager.GetString ) )
                                  .Invoke ( Code.Constant ( name ), culture );
        }

        private static ResourceGetter GetObject ( CodeTypeReference type )
        {
            return (resourceManager, name, culture) =>
                   resourceManager.Method ( nameof ( ResourceManager.GetObject ) )
                                  .Invoke ( Code.Constant ( name ), culture )
                                  .Cast   ( type );
        }

        private static CodeExpression GetStream ( CodeExpression resourceManager, string name, CodeExpression culture )
        {
            return resourceManager.Method ( nameof ( ResourceManager.GetStream ) )
                                  .Invoke ( Code.Constant ( name ), culture );
        }
    }
}
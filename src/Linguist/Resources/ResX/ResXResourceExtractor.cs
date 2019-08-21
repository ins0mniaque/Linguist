using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Xml;
using System.Xml.Linq;

using Linguist.XML;

namespace Linguist.Resources.ResX
{
    public class ResXResourceExtractor : ResourceExtractor
    {
        public ResXResourceExtractor ( Stream stream   ) : base ( stream   ) { }
        public ResXResourceExtractor ( string fileName ) : base ( fileName ) { }

        protected override IEnumerable < IResource > Extract ( Stream stream )
        {
            var document = XDoc.Load ( stream );
            var source   = GetStreamSource ( stream );
            var aliases  = new Dictionary < string, string > ( );

            foreach ( var data in document.Descendants ( "assembly" ) )
            {
                var name  = data.Attribute ( "name"  )?.Value;
                var alias = data.Attribute ( "alias" )?.Value;
                if ( name == null || alias == null )
                    continue;

                aliases.Add ( alias, name );
            }

            foreach ( var data in document.Descendants ( "data" ) )
            {
                var name = data.Attribute ( "name" )?.Value;
                if ( name == null )
                    continue;

                var resource = ParseResource ( data, aliases );

                resource.Name    = name;
                resource.Comment = data.Element ( "comment" )?.Value;
                resource.Source  = source;

                if ( data is IXmlLineInfo info && info.HasLineInfo ( ) )
                {
                    resource.Line   = info.LineNumber;
                    resource.Column = info.LinePosition;
                }

                yield return resource;
            }
        }

        private Resource ParseResource ( XElement data, IDictionary < string, string > aliases )
        {
            var value = data.Element ( "value" )?.Value;
            if ( value == null )
                return new Resource ( );

            var mimetype = data.Attribute ( "mimetype" )?.Value;
            var type     = data.Attribute ( "type"     )?.Value;
            if ( type == null && mimetype == null )
                return new Resource ( ) { Value = value };

            var typeName = (TypeName) null;

            if ( type != null )
            {
                typeName = new TypeName ( type );

                if ( typeName.Type == "System.Resources.ResXNullRef" )
                    return new Resource ( );

                if ( typeName.Type == "System.Resources.ResXFileRef" )
                {
                    ResXFileRef.Parse ( value, out _, out var refType, out _ );

                    return new LoadableResource ( ResXFileRef.Load ) { Type = new TypeName ( refType ),
                                                                       Data = value };
                }

                if ( typeName == TypeNames.ByteArray )
                    return new Resource ( ) { Value = FromBase64 ( value ) };

                if ( aliases.TryGetValue ( typeName.Assembly, out var assemblyName ) )
                    typeName = typeName.WithAssembly ( assemblyName );
            }

            switch ( mimetype )
            {
                case "application/x-microsoft.net.object.binary.base64" :
                    return new LoadableResource ( DeserializeBinary ) { Data = FromBase64 ( value ) };

                case "application/x-microsoft.net.object.soap.base64" :
                    throw new NotSupportedException ( "SoapFormatter is not supported" );

                case "application/x-microsoft.net.object.bytearray.base64" :
                    return new LoadableResource ( ConvertFrom < byte [ ] > ) { Type = typeName ?? throw new Exception ( "Missing type for serialized resource" ),
                                                                               Data = FromBase64 ( value ) };

                case null : break;
                default   : throw new NotSupportedException ( "Unsupported mimetype " + mimetype );
            }

            if ( type != null )
                return new LoadableResource ( ConvertFromString ) { Type = typeName ?? throw new Exception ( "Missing type for serialized resource" ),
                                                                    Data = value };

            return new Resource ( ) { Value = value };
        }

        private   BinaryFormatter binaryFormatter;
        protected BinaryFormatter BinaryFormatter
        {
            get
            {
                if ( binaryFormatter == null )
                    binaryFormatter = new BinaryFormatter ( ) { Binder            = TypeResolver.Binder,
                                                                SurrogateSelector = TypeResolver.SurrogateSelector };

                return binaryFormatter;
            }
        }

        private object DeserializeBinary ( ILoadableResource resource )
        {
            var buffer = resource.Data as byte [ ];
            if ( buffer == null || buffer.Length == 0 )
                return null;

            var value = BinaryFormatter.Deserialize ( new MemoryStream ( buffer ) );
            if ( value?.GetType ( ).FullName == "System.Resources.ResXNullRef" )
                return null;

            return resource;
        }

        private static object ConvertFrom < T > ( ILoadableResource resource )
        {
            var resolvedType = TypeResolver  .ResolveType  ( resource.Type ?? throw new Exception ( "Missing type for serialized resource" ) );
            var converter    = TypeDescriptor.GetConverter ( resolvedType );

            if ( converter.CanConvertFrom ( typeof ( T ) ) && resource.Data is T data )
                return converter.ConvertFrom ( data );

            return null;
        }

        private static object ConvertFromString ( ILoadableResource resource )
        {
            var resolvedType = TypeResolver  .ResolveType  ( resource.Type ?? throw new Exception ( "Missing type for serialized resource" ) );
            var converter    = TypeDescriptor.GetConverter ( resolvedType );

            if ( converter.CanConvertFrom ( typeof ( string ) ) && resource.Data is string data )
                return converter.ConvertFromInvariantString ( data );

            return null;
        }

        private const           char     Base64WhitespaceA = '\n';
        private const           char     Base64WhitespaceB = '\r';
        private const           char     Base64WhitespaceC = ' ';
        private static readonly char [ ] Base64Whitespaces = new [ ] { Base64WhitespaceA,
                                                                       Base64WhitespaceB,
                                                                       Base64WhitespaceC };

        private static byte [ ] FromBase64 ( string text )
        {
            if ( text.IndexOfAny ( Base64Whitespaces ) < 0 )
                return Convert.FromBase64String ( text );

            var buffer = new StringBuilder ( text.Length );

            for ( var index = 0; index < text.Length; index++ )
            {
                var character    = text [ index ];
                var isWhitespace = character == Base64WhitespaceA ||
                                   character == Base64WhitespaceB ||
                                   character == Base64WhitespaceC;

                if ( ! isWhitespace )
                    buffer.Append ( character );
            }

            return Convert.FromBase64String ( buffer.ToString ( ) );
        }
    }
}
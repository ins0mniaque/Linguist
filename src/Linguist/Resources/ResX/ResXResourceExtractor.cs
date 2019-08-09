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
            var source   = GetStreamPath ( stream );
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

                var resource = (ResourceMetadata) null;
                var type     = ParseType ( data, out var externalResource );

                if ( externalResource == null )
                {
                    var value = ParseValue ( data, aliases );
                    if ( type == null )
                        type = value?.GetType ( ).FullName;

                    resource = new Resource ( ) { Value = value };
                }
                else
                    resource = externalResource;

                resource.Name    = name;
                resource.Type    = type;
                resource.Comment = data.Element ( "comment" )?.Value;

                resource.Source = source;

                if ( data is IXmlLineInfo info && info.HasLineInfo ( ) )
                {
                    resource.Line   = info.LineNumber;
                    resource.Column = info.LinePosition;
                }

                yield return (IResource) resource;
            }
        }

        private static string ParseType ( XElement data, out ExternalResource externalResource )
        {
            externalResource = null;

            var type = data.Attribute ( "type" )?.Value;

            if ( type?.Split ( ',' ) [ 0 ] == "System.Resources.ResXFileRef" )
            {
                var value = data.Element ( "value" )?.Value;
                if ( value == null )
                    throw new Exception ( "ResXFileRef missing value" );

                var parts    = value.Split ( ';' );
                var typeName = parts [ parts.Length - 1 ];

                externalResource = new ExternalResource ( ResXFileRef.Load ) { Reference = value };

                return typeName;
            }

            return type;
        }

        private object ParseValue ( XElement data, IDictionary < string, string > aliases )
        {
            var value = data.Element ( "value" )?.Value;
            if ( value == null )
                return null;

            var mimetype = data.Attribute ( "mimetype" )?.Value;
            var type     = data.Attribute ( "type"     )?.Value;
            if ( type == null && mimetype == null )
                return value;

            var resolvedType = (Type) null;

            if ( type != null )
            {
                var comma    = type.IndexOf   ( "," );
                var alias    = type.Substring ( comma + 2 );
                var typeName = type.Substring ( 0, comma );

                if ( typeName == "System.Resources.ResXNullRef" )
                    return null;

                if ( ! aliases.TryGetValue ( alias, out var assemblyName ) )
                    assemblyName = alias;

                resolvedType = TypeResolver.ResolveType ( typeName + ", " + assemblyName );
            }

            switch ( mimetype )
            {
                case "application/x-microsoft.net.object.binary.base64" :
                    return FromBinaryBase64 ( value );

                case "application/x-microsoft.net.object.soap.base64" :
                    throw new NotSupportedException ( "SoapFormatter is not supported" );

                case "application/x-microsoft.net.object.bytearray.base64" :
                    return FromByteArrayBase64 ( resolvedType, value );

                case null : break;
                default   : throw new NotSupportedException ( "Unsupported mimetype " + mimetype );
            }

            if ( resolvedType == typeof ( byte [ ] ) )
                return FromBase64 ( value );

            if ( resolvedType != null )
            {
                var converter = TypeDescriptor.GetConverter ( resolvedType );
                if ( converter.CanConvertFrom ( typeof ( string ) ) )
                    return converter.ConvertFromInvariantString ( value );
            }

            return value;
        }

        private   BinaryFormatter binaryFormatter;
        protected BinaryFormatter BinaryFormatter
        {
            get
            {
                if ( binaryFormatter == null )
                    binaryFormatter = new BinaryFormatter ( ) { Binder = new TypeResolver.Binder ( ) };

                return binaryFormatter;
            }
        }

        private object FromBinaryBase64 ( string value )
        {
            var buffer = FromBase64 ( value );
            if ( buffer == null || buffer.Length == 0 )
                return null;

            var resource = BinaryFormatter.Deserialize ( new MemoryStream ( buffer ) );
            if ( resource?.GetType ( ).FullName == "System.Resources.ResXNullRef" )
                return null;

            return resource;
        }

        private static object FromByteArrayBase64 ( Type type, string value )
        {
            var converter = TypeDescriptor.GetConverter ( type ?? throw new Exception ( "Missing type for byte array object" ) );
            if ( ! converter.CanConvertFrom ( typeof ( byte [ ] ) ) )
                return null;

            var buffer = FromBase64 ( value );
            if ( buffer == null )
                return null;

            return converter.ConvertFrom ( buffer );
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
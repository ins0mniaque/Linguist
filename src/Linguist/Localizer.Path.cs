namespace Linguist
{
    public static partial class Localizer
    {
        public static class Path
        {
            public static readonly char ProtocolChar     = ':';
            public static readonly char SeparatorChar    = '/';
            public static readonly char AltSeparatorChar = '\\';

            public enum Type
            {
                ManifestResource,
                FilePath,
                Protocol
            }

            public static Type GetType ( string path )
            {
                if ( path.Length < 1 )
                    return Type.ManifestResource;

                if ( path [ 0 ] == SeparatorChar || path [ 0 ] == AltSeparatorChar )
                    return Type.FilePath;

                return path.IndexOf ( ProtocolChar ) >= 0 ? Type.Protocol :
                                                            Type.ManifestResource;
            }

            public static string GetProtocol ( string path )
            {
                var protocol = path.IndexOf ( ProtocolChar );
                if ( protocol < 0 )
                    return null;

                return path.Substring ( 0, protocol );
            }

            public static string GetManifestResourceAssembly ( string path )
            {
                return path.Split ( SeparatorChar, AltSeparatorChar ) [ 0 ];
            }

            public static string GetManifestResourceName ( string path )
            {
                return path.Replace ( SeparatorChar,    '.' )
                           .Replace ( AltSeparatorChar, '.' );
            }
        }
    }
}
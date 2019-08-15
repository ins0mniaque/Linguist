using System;
using System.IO;
using System.Text.RegularExpressions;

namespace Linguist.Resources
{
    public static class AutoDetect
    {
        public static Type ResourceSetType ( string path )
        {
            var extension = Path.GetExtension ( path )?.ToLowerInvariant ( );

            switch ( extension )
            {
                case ".resx"      :
                case ".resw"      : return typeof ( ResX.ResXResourceSet );
                case ".resources" :
                default           : return typeof ( Binary.BinaryResourceSet );
            }
        }

        public static Type ResourceExtractorType ( string path )
        {
            var extension = Path.GetExtension ( path )?.ToLowerInvariant ( );

            switch ( extension )
            {
                case ".resx"      :
                case ".resw"      : return typeof ( ResX.ResXResourceExtractor );
                case ".resources" :
                default           : return typeof ( Binary.BinaryResourceExtractor );
            }
        }

        public static string PathFormat ( string path, out string neutralCultureName )
        {
            neutralCultureName = null;

            if ( TemplateEngine.HasArguments ( path ) )
                return path;

            var extension = Path.GetExtension ( path )?.ToLowerInvariant ( );
            var filename  = Path.GetFileNameWithoutExtension ( path );
            var culture   = Path.GetExtension ( filename )?.TrimStart ( '.' );
            var language  = (string) null;

            if ( CultureValidator.IsCulture ( culture ) )
            {
                filename = Path.GetFileNameWithoutExtension ( filename );
                language = "{culture}";
            }
            else if ( CultureValidator.IsLocale ( culture ) )
            {
                filename = Path.GetFileNameWithoutExtension ( filename );
                language = "{locale}";
            }
            else
                culture = null;

            neutralCultureName = culture;

            switch ( extension )
            {
                case ".resx"      :
                case ".resw"      :
                case ".resources" :
                default           : return Path.Combine ( Path.GetDirectoryName ( path ),
                                                          filename + "." + language ?? "{culture}" + extension );
            }
        }

        private static class CultureValidator
        {
            private static readonly Regex culture = new Regex ( @"^[A-Za-z]{2,4}(-([A-Za-z]{4}|[0-9]{3}))?(-([A-Za-z]{2}|[0-9]{3}))?$", RegexOptions.Compiled );
            private static readonly Regex locale  = new Regex ( @"^[A-Za-z]{2,4}(_([A-Za-z]{4}|[0-9]{3}))?(_([A-Za-z]{2}|[0-9]{3}))?$", RegexOptions.Compiled );

            public static bool IsCulture ( string name )
            {
                return ! string.IsNullOrEmpty ( name ) && culture.IsMatch ( name );
            }

            public static bool IsLocale ( string name )
            {
                return ! string.IsNullOrEmpty ( name ) && locale.IsMatch ( name );
            }
        }
    }
}
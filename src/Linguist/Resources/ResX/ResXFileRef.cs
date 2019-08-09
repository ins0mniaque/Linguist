using System;
using System.IO;
using System.Reflection;
using System.Text;

namespace Linguist.Resources.ResX
{
    public static class ResXFileRef
    {
        /// <summary>Creates the text representation of a <see cref="T:System.Resources.ResXFileRef" /> object.</summary>
        /// <returns>A string that consists of the concatenated text representations of the parameters specified in the current <see cref="Overload:System.Resources.ResXFileRef.#ctor" /> constructor.</returns>
        public static string Create ( string fileName, string type, Encoding encoding = null )
        {
            var resxFileRef = string.Empty;

            if ( fileName.IndexOf ( ";"  ) != -1 ||
                 fileName.IndexOf ( "\"" ) != -1 )
                resxFileRef = resxFileRef + "\"" + fileName + "\";";
            else
                resxFileRef = resxFileRef + fileName + ";";

            resxFileRef += type;

            if ( encoding != null )
                resxFileRef = resxFileRef + ";" + encoding.WebName;

            return resxFileRef;
        }

        public static object Load ( IExternalResource resource )
        {
            return Load ( Path.GetDirectoryName ( resource.Source ),
                          resource.Reference?.ToString ( ) );
        }

        public static object Load ( string baseDirectory, string resxFileRef )
        {
            if ( resxFileRef == null )
                return null;

            Parse ( resxFileRef, out var fileName, out var typeName, out var encodingName );

            fileName = Path.Combine ( baseDirectory, fileName );

            var type = TypeResolver.ResolveType ( typeName );

            if ( type == typeof ( string ) )
            {
                var encoding = Encoding.Default;
                if ( encodingName != null )
                    encoding = Encoding.GetEncoding ( encodingName );

                using ( var streamReader = new StreamReader ( fileName, encoding ) )
                    return streamReader.ReadToEnd ( );
            }

            var buffer = (byte [ ]) null;

            using ( var fileStream = new FileStream ( fileName, FileMode.Open, FileAccess.Read, FileShare.Read ) )
            {
                buffer = new byte [ fileStream.Length ];

                fileStream.Read ( buffer, 0, (int) fileStream.Length );
            }

            if ( type == typeof ( byte [ ] ) )
                return buffer;

            var memoryStream = new MemoryStream ( buffer );

            if ( type == typeof ( MemoryStream ) )
                return memoryStream;

            if ( type.Name == "System.Drawing.Bitmap" && fileName.EndsWith ( ".ico" ) )
            {
                var iconType = type.Assembly.GetType ( "System.Drawing.Icon" );
                var icon     = CreateInstance ( iconType, memoryStream );

                return iconType.GetMethod ( "ToBitmap", Type.EmptyTypes )
                               .Invoke    ( icon, null );
            }

            return CreateInstance ( type, memoryStream );
        }

        private static void Parse ( string resxFileRef, out string fileName, out string typeName, out string encodingName )
        {
            fileName     = null;
            typeName     = null;
            encodingName = null;

            if ( resxFileRef == null )
                return;

            resxFileRef = resxFileRef.Trim ( );

            var parameters = (string [ ]) null;

            if ( resxFileRef.StartsWith ( "\"" ) )
            {
                var lastIndexOfQuote = resxFileRef.LastIndexOf ( "\"" );
                if ( lastIndexOfQuote - 1 < 0 )
                    throw new ArgumentException ( nameof ( resxFileRef ) );

                fileName = resxFileRef.Substring ( 1, lastIndexOfQuote - 1 );
                if ( lastIndexOfQuote + 2 > resxFileRef.Length )
                    throw new ArgumentException ( nameof ( resxFileRef ) );

                parameters = resxFileRef.Substring ( lastIndexOfQuote + 2 ).Split ( ';' );
            }
            else
            {
                var nextSemiColumn = resxFileRef.IndexOf ( ";" );
                if ( nextSemiColumn == -1 )
                    throw new ArgumentException ( nameof ( resxFileRef ) );

                fileName = resxFileRef.Substring ( 0, nextSemiColumn );
                if ( nextSemiColumn + 1 > resxFileRef.Length )
                    throw new ArgumentException ( nameof ( resxFileRef ) );

                parameters = resxFileRef.Substring ( nextSemiColumn + 1 ).Split ( ';' );
            }

            typeName = parameters [ 0 ];

            if ( parameters.Length > 1 )
                encodingName = parameters [ 1 ];
        }

        private static object CreateInstance ( Type type, MemoryStream stream )
        {
            return Activator.CreateInstance ( type,
                                              BindingFlags.Instance | BindingFlags.Public | BindingFlags.CreateInstance,
                                              null,
                                              new object [ ] { stream },
                                              null );
        }
    }
}
using System.IO;
using System.Xml;
using System.Xml.Linq;

namespace Linguist.XML
{
    internal static class XDoc
    {
        public static XDocument Load ( Stream stream, LoadOptions options = LoadOptions.None )
        {
            #if ! NET35
            return XDocument.Load ( stream, options );
            #else
            var settings = new XmlReaderSettings ( );
            if ( ( options & LoadOptions.PreserveWhitespace ) == LoadOptions.None )
                settings.IgnoreWhitespace = true;

            settings.ProhibitDtd               = false;
            settings.MaxCharactersFromEntities = 10000000L;
            settings.XmlResolver               = null;

            using ( var reader = XmlReader.Create ( stream, settings ) )
                return XDocument.Load ( reader, options );
            #endif
        }
    }
}
using System;
using System.IO;
using System.Linq;
using System.Text;

using Linguist.Resources;

namespace Linguist.Generator
{
    using static String;
    using static ErrorMessages;

    public static class ResourceExtractor
    {
        public static IResource [ ] ExtractResources ( string fileName )
        {
            var extractorType = AutoDetect.ResourceExtractorType ( fileName );
            if ( extractorType == null )
                throw new ArgumentException ( Format ( UnknownResourceFileFormat, Path.GetFileName ( fileName ) ), nameof ( fileName ) );

            return ExtractResources ( extractorType, fileName );
        }

        public static IResource [ ] ExtractResources ( string fileName, string content )
        {
            var extractorType = AutoDetect.ResourceExtractorType ( fileName );
            if ( extractorType == null )
                throw new ArgumentException ( Format ( UnknownResourceFileFormat, Path.GetFileName ( fileName ) ), nameof ( fileName ) );

            return ExtractResources ( extractorType, new MemoryStream ( Encoding.UTF8.GetBytes ( content ) ) );
        }

        public static IResource [ ] ExtractResources ( Type resourceExtractorType, Stream stream )
        {
            return ( (IResourceExtractor) Activator.CreateInstance ( resourceExtractorType, stream ) ).Extract ( ).ToArray ( );
        }

        public static IResource [ ] ExtractResources ( Type resourceExtractorType, string fileName )
        {
            return ( (IResourceExtractor) Activator.CreateInstance ( resourceExtractorType, fileName ) ).Extract ( ).ToArray ( );
        }
    }
}
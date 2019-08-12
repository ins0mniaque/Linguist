using System.IO;
using System.Linq;

using Xunit;

namespace Linguist.Resources.Binary.Tests
{
    public class BinaryResourceExtractorTests
    {
        [ Theory ]
        [ InlineData ( "Resources/Binary/Data/Resources.resources", 21 ) ]
        public void ReadsTheCorrectAmountOfResources ( string resourcesFile, int expectedNumberOfResources )
        {
            var stream    = new FileStream ( FindFile ( resourcesFile ), FileMode.Open, FileAccess.Read, FileShare.Read );
            var extractor = new BinaryResourceExtractor ( stream );
            var resources = extractor.Read ( ).ToList ( );

            Assert.All   ( resources, resource => Assert.NotNull ( resource.Value ) );
            Assert.Equal ( expectedNumberOfResources, resources.Count );
        }

        private static string FindFile ( string filename ) => Path.Combine ( "..\\..\\..\\", filename );
    }
}
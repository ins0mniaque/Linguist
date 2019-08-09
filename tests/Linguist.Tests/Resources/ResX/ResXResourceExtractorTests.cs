using System.IO;
using System.Linq;

using Xunit;

namespace Linguist.Resources.ResX.Tests
{
    public class ResXResourceExtractorTests
    {
        [ Theory ]
        [ InlineData ( "Resources/ResX/Data/Resources.resx", 21 ) ]
        public void ReadsTheCorrectAmountOfResources ( string resxFile, int expectedNumberOfResources )
        {
            var stream    = new FileStream ( FindFile ( resxFile ), FileMode.Open, FileAccess.Read, FileShare.Read );
            var extractor = new ResXResourceExtractor ( stream );
            var resources = extractor.Read ( ).ToList ( );

            Assert.All   ( resources, resource => Assert.NotNull ( resource.Value ) );
            Assert.Equal ( expectedNumberOfResources, resources.Count );
        }

        private static string FindFile ( string filename ) => Path.Combine ( "..\\..\\..\\", filename );
    }
}
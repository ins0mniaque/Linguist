using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.IO;

using Xunit;

namespace Linguist.Resources.ResX.Tests
{
    public class ResXResourceSetTests
    {
        [ Theory ]
        [ InlineData ( "Resources/ResX/Data/Resources.resx", 21 ) ]
        public void ReadsTheCorrectAmountOfResources ( string resxFile, int expectedNumberOfResources )
        {
            var resourceManager = new FileBasedResourceManager < ResXResourceSet > ( Path.GetFileNameWithoutExtension ( resxFile ),
                                                                                     Path.GetDirectoryName ( FindFile ( resxFile ) ),
                                                                                     "{resource}.{culture}.resx",
                                                                                     null );
            var resourceSet     = resourceManager.GetResourceSet ( CultureInfo.InvariantCulture, true, true );
            var resources       = new List < DictionaryEntry > ( );

            var enumerator = resourceSet.GetEnumerator ( );
            while ( enumerator.MoveNext ( ) )
                resources.Add ( enumerator.Entry );

            Assert.All   ( resources, resource => Assert.NotNull ( resource.Value ) );
            Assert.Equal ( expectedNumberOfResources, resources.Count );
        }

        private static string FindFile ( string filename ) => Path.Combine ( "..\\..\\..\\", filename );
    }
}
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.IO;

using Xunit;

namespace Linguist.Resources.Binary.Tests
{
    public class BinaryResourceSetTests
    {
        [ Theory ]
        [ InlineData ( "Resources/Binary/Data/Resources.resources", 21 ) ]
        public void ReadsTheCorrectAmountOfResources ( string resourcesFile, int expectedNumberOfResources )
        {
            var resourceManager = new FileBasedResourceManager < BinaryResourceSet > ( Path.GetFileNameWithoutExtension ( resourcesFile ),
                                                                                       Path.GetDirectoryName ( FindFile ( resourcesFile ) ),
                                                                                       "{resource}.{culture}.resources",
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
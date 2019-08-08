using System.Collections;
using System.Globalization;
using System.Resources;

using Xunit;

namespace Linguist.Resources.Tests
{
    public class ResourceManagerLocalizerFixture
    {
        private class en_US : IResourceReader
        {
            public void Close   ( ) { }
            public void Dispose ( ) { }

            public IDictionaryEnumerator GetEnumerator ( )
            {
                return new Hashtable ( )
                {
                    { "Items",      "{0} items"  },
                    { "Items.Zero", "No items"   },
                    { "Items.One",  "{0} item"   },
                    { "Pixels",     "{0} pixels" },
                    { "Pixels.One", "{0} pixel"  }
                }.GetEnumerator ( );
            }

            IEnumerator IEnumerable.GetEnumerator ( ) => GetEnumerator ( );
        }

        private class fr_FR : IResourceReader
        {
            public void Close   ( ) { }
            public void Dispose ( ) { }

            public IDictionaryEnumerator GetEnumerator ( )
            {
                return new Hashtable ( )
                {
                    { "Items",      "{0} items"  },
                    { "Items.Zero", "Aucun item" },
                    { "Items.One",  "{0} item"   },
                    { "Pixels",     "{0} pixels" },
                    { "Pixels.One", "{0} pixel"  }
                }.GetEnumerator ( );
            }

            IEnumerator IEnumerable.GetEnumerator ( ) => GetEnumerator ( );
        }

        private class InMemoryResourceManager : ResourceManager
        {
            protected override ResourceSet InternalGetResourceSet ( CultureInfo culture, bool createIfNotExists, bool tryParents )
            {
                if ( culture.Name == "en-US" ) return new ResourceSet ( new en_US ( ) );
                if ( culture.Name == "fr-FR" ) return new ResourceSet ( new fr_FR ( ) );

                return null;
            }
        }

        public ResourceManagerLocalizerFixture ( )
        {
            Localizer = new ResourceManagerLocalizer ( new InMemoryResourceManager ( ) );
        }

        public ResourceManagerLocalizer Localizer { get; }
    }

    public class ResourceManagerLocalizerTests : IClassFixture < ResourceManagerLocalizerFixture >
    {
        private readonly ResourceManagerLocalizerFixture fixture;

        public ResourceManagerLocalizerTests ( ResourceManagerLocalizerFixture fixture )
        {
            this.fixture = fixture;
        }

        [ Theory ]
        [ InlineData ( "No items",   "Items",  "0"   ) ]
        [ InlineData ( "0.0 items",  "Items",  "0.0" ) ]
        [ InlineData ( "1.0 items",  "Items",  "1.0" ) ]
        [ InlineData ( "No items",   "Items",  0     ) ]
        [ InlineData ( "1 item",     "Items",  1     ) ]
        [ InlineData ( "2 items",    "Items",  2     ) ]
        [ InlineData ( "0 pixels",   "Pixels", 0     ) ]
        [ InlineData ( "1 pixel",    "Pixels", 1     ) ]
        [ InlineData ( "2 pixels",   "Pixels", 2     ) ]
        [ InlineData ( "0.0 pixels", "Pixels", "0.0" ) ]
        [ InlineData ( "1.0 pixels", "Pixels", "1.0" ) ]
        [ InlineData ( "2.0 pixels", "Pixels", "2.0" ) ]
        public void EnglishFormatReturnsTheCorrectString ( string expectedString, string name, params object [ ] args )
        {
            var culture = CultureInfo.GetCultureInfo ( "en-US" );

            Assert.Equal ( expectedString, fixture.Localizer.Format ( culture, null, name, args ) );
        }

        [ Theory ]
        [ InlineData ( "Aucun item", "Items",  "0"   ) ]
        [ InlineData ( "0,0 item",   "Items",  "0,0" ) ]
        [ InlineData ( "1,0 item",   "Items",  "1,0" ) ]
        [ InlineData ( "Aucun item", "Items",  0     ) ]
        [ InlineData ( "1 item",     "Items",  1     ) ]
        [ InlineData ( "2 items",    "Items",  2     ) ]
        [ InlineData ( "0 pixel",    "Pixels", 0     ) ]
        [ InlineData ( "1 pixel",    "Pixels", 1     ) ]
        [ InlineData ( "2 pixels",   "Pixels", 2     ) ]
        [ InlineData ( "0,0 pixel",  "Pixels", "0,0" ) ]
        [ InlineData ( "1,0 pixel",  "Pixels", "1,0" ) ]
        [ InlineData ( "2,0 pixels", "Pixels", "2,0" ) ]
        public void FrenchFormatReturnsTheCorrectString ( string expectedString, string name, params object [ ] args )
        {
            var culture = CultureInfo.GetCultureInfo ( "fr-FR" );

            Assert.Equal ( expectedString, fixture.Localizer.Format ( culture, null, name, args ) );
        }
    }
}
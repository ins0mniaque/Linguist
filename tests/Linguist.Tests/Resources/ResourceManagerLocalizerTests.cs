using System.Collections;
using System.Globalization;

using Xunit;

using Linguist.Resources.Dictionary;

namespace Linguist.Resources.Tests
{
    public class ResourceManagerLocalizerFixture
    {
        public ResourceManagerLocalizerFixture ( )
        {
            var resourceManager = new DictionaryResourceManager ( "en-US" );

            resourceManager.Add ( "en-US", new Hashtable ( )
            {
                { "Items",      "{0} items"  },
                { "Items.Zero", "No items"   },
                { "Items.One",  "{0} item"   },
                { "Pixels",     "{0} pixels" },
                { "Pixels.One", "{0} pixel"  }
            } );

            resourceManager.Add ( "fr-FR", new Hashtable ( )
            {
                { "Items",      "{0} items"  },
                { "Items.Zero", "Aucun item" },
                { "Items.One",  "{0} item"   },
                { "Pixels",     "{0} pixels" },
                { "Pixels.One", "{0} pixel"  }
            } );

            Localizer = new ResourceManagerLocalizer ( resourceManager );
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
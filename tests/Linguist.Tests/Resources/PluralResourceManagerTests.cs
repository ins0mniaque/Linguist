using System.Collections.Generic;
using System.Globalization;

using Xunit;

namespace Linguist.Resources.Tests
{
    public class PluralResourceManagerFixture
    {
        private static readonly IDictionary < CultureInfo, IDictionary < string, object > > resources;

        static PluralResourceManagerFixture ( )
        {
            resources = new Dictionary < CultureInfo, IDictionary < string, object > > ( );

            resources.Add ( CultureInfo.GetCultureInfo ( "en-US" ),
                            new Dictionary < string, object > ( )
                            {
                                { "Items",      "{0} items"  },
                                { "Items.Zero", "No items"   },
                                { "Items.One",  "{0} item"   },
                                { "Pixels",     "{0} pixels" },
                                { "Pixels.One", "{0} pixel"  }
                            } );

            resources.Add ( CultureInfo.GetCultureInfo ( "fr-FR" ),
                            new Dictionary < string, object > ( )
                            {
                                { "Items",      "{0} items"  },
                                { "Items.Zero", "Aucun item" },
                                { "Items.One",  "{0} item"   },
                                { "Pixels",     "{0} pixels" },
                                { "Pixels.One", "{0} pixel"  }
                            } );
        }

        public PluralResourceManagerFixture ( )
        {
            PluralResourceManager = new PluralResourceManager ( culture => resources [ culture ] );
        }

        public PluralResourceManager PluralResourceManager { get; }
    }

    public class PluralResourceManagerTests : IClassFixture < PluralResourceManagerFixture >
    {
        private readonly PluralResourceManagerFixture fixture;

        public PluralResourceManagerTests ( PluralResourceManagerFixture fixture )
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
            var culture      = CultureInfo.GetCultureInfo ( "en-US" );
            var manager      = fixture.PluralResourceManager;
            var resourceSet  = manager.GetResourceSet ( PluralRules.GetPluralRules ( culture ) );
            var formatString = resourceSet.SelectPluralResource ( name, args );

            Assert.Equal ( expectedString, string.Format ( culture, formatString.Format, args ) );
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
            var culture      = CultureInfo.GetCultureInfo ( "fr-FR" );
            var manager      = fixture.PluralResourceManager;
            var resourceSet  = manager.GetResourceSet ( PluralRules.GetPluralRules ( culture ) );
            var formatString = resourceSet.SelectPluralResource ( name, args );

            Assert.Equal ( expectedString, string.Format ( culture, formatString.Format, args ) );
        }
    }
}
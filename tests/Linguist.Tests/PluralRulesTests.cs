using System;
using System.Globalization;

using Xunit;

namespace Linguist.Tests
{
    using static PluralForm;

    public class PluralRulesTests
    {
        private static readonly CultureInfo [ ] AllCultures = CultureInfo.GetCultures ( CultureTypes.AllCultures );

        [ Fact ]
        public static void GetPluralRulesReturnsWithTheCurrentUICulture ( )
        {
            Assert.Equal ( CultureInfo.CurrentUICulture, PluralRules.GetPluralRules ( ).Culture );
        }

        [ Fact ]
        public static void GetPluralRulesThrowsOnNull ( )
        {
            Assert.Throws < ArgumentNullException > ( ( ) => PluralRules.GetPluralRules ( null ) );
        }

        [ Fact ]
        public static void GetPluralRulesReturnsWithTheCorrectCulture ( )
        {
            foreach ( var culture in AllCultures )
                Assert.Equal ( culture, PluralRules.GetPluralRules ( culture ).Culture );
        }

        [ Theory ]
        [ InlineData ( "",             new PluralForm [ ] { },   new object [ ] { },          new PluralForm [ ] { }   ) ]
        [ InlineData ( "{0}",          new [ ] { Other },        new object [ ] { 0.0 },      new [ ] { Other }        ) ]
        [ InlineData ( "{0:N0}",       new [ ] { Other },        new object [ ] { 0.0 },      new [ ] { Other }        ) ]
        [ InlineData ( "{0:N0}",       new [ ] { Zero  },        new object [ ] { 0.0 },      new [ ] { Zero  }        ) ]
        [ InlineData ( "{0:N1}",       new [ ] { Zero  },        new object [ ] { 0.0 },      new [ ] { Other }        ) ]
        [ InlineData ( "{0:N0}",       new [ ] { One   },        new object [ ] { 1.0 },      new [ ] { One   }        ) ]
        [ InlineData ( "{0:N1}",       new [ ] { One   },        new object [ ] { 1.0 },      new [ ] { Other }        ) ]
        [ InlineData ( "{0:N0}",       new [ ] { ExplicitZero }, new object [ ] { 0.0 },      new [ ] { ExplicitZero } ) ]
        [ InlineData ( "{0:N1}",       new [ ] { ExplicitZero }, new object [ ] { 0.0 },      new [ ] { Other        } ) ]
        [ InlineData ( "{0:N0}",       new [ ] { ExplicitOne  }, new object [ ] { 1.0 },      new [ ] { ExplicitOne  } ) ]
        [ InlineData ( "{0:N1}",       new [ ] { ExplicitOne  }, new object [ ] { 1.0 },      new [ ] { Other        } ) ]
        [ InlineData ( "{0:N0}{1:N0}", new [ ] { Other, Other }, new object [ ] { 0.0, 1.0 }, new [ ] { Other, Other } ) ]
        [ InlineData ( "{0:N0}{1:N0}", new [ ] { Zero,  One   }, new object [ ] { 0.0, 1.0 }, new [ ] { Zero,  One   } ) ]
        [ InlineData ( "{0:N1}{1:N1}", new [ ] { Zero,  One   }, new object [ ] { 0.0, 1.0 }, new [ ] { Other, Other } ) ]
        public static void SelectPluralFormsOnInvariantReturnsTheCorrectPluralForms ( string format, PluralForm [ ] availablePluralForms, object [ ] arguments, PluralForm [ ] expectedPluralForms )
        {
            var formatString = FormatString.Parse ( format );

            for ( var index = 0; index < formatString.Arguments.Length; index++ )
                formatString.Arguments [ index ].AvailablePluralForms = availablePluralForms [ index ];

            Assert.Equal ( expectedPluralForms, PluralRules.Invariant.SelectPluralForms ( formatString, arguments ) );
        }

        [ Theory ]
        [ InlineData ( "",             new PluralForm [ ] { },   new object [ ] { },          new PluralForm [ ] { }   ) ]
        [ InlineData ( "{0}",          new [ ] { Other },        new object [ ] { 0.0 },      new [ ] { Other }        ) ]
        [ InlineData ( "{0:N0}",       new [ ] { Other },        new object [ ] { 0.0 },      new [ ] { Other }        ) ]
        [ InlineData ( "{0:N0}",       new [ ] { Zero  },        new object [ ] { 0.0 },      new [ ] { Other }        ) ]
        [ InlineData ( "{0:N1}",       new [ ] { Zero  },        new object [ ] { 0.0 },      new [ ] { Other }        ) ]
        [ InlineData ( "{0:N0}",       new [ ] { One   },        new object [ ] { 1.0 },      new [ ] { Other }        ) ]
        [ InlineData ( "{0:N1}",       new [ ] { One   },        new object [ ] { 1.0 },      new [ ] { Other }        ) ]
        [ InlineData ( "{0:N0}",       new [ ] { ExplicitZero }, new object [ ] { 0.0 },      new [ ] { Other }        ) ]
        [ InlineData ( "{0:N1}",       new [ ] { ExplicitZero }, new object [ ] { 0.0 },      new [ ] { Other }        ) ]
        [ InlineData ( "{0:N0}",       new [ ] { ExplicitOne  }, new object [ ] { 1.0 },      new [ ] { Other }        ) ]
        [ InlineData ( "{0:N1}",       new [ ] { ExplicitOne  }, new object [ ] { 1.0 },      new [ ] { Other }        ) ]
        [ InlineData ( "{0:N0}{1:N0}", new [ ] { Other, Other }, new object [ ] { 0.0, 1.0 }, new [ ] { Other, Other } ) ]
        [ InlineData ( "{0:N0}{1:N0}", new [ ] { Zero,  One   }, new object [ ] { 0.0, 1.0 }, new [ ] { Other, Other } ) ]
        [ InlineData ( "{0:N1}{1:N1}", new [ ] { Zero,  One   }, new object [ ] { 0.0, 1.0 }, new [ ] { Other, Other } ) ]
        public static void SelectPluralFormsOrdinalOnInvariantReturnsTheCorrectPluralForms ( string format, PluralForm [ ] availablePluralForms, object [ ] arguments, PluralForm [ ] expectedPluralForms )
        {
            var formatString = FormatString.Parse ( format );

            for ( var index = 0; index < formatString.Arguments.Length; index++ )
            {
                formatString.Arguments [ index ].NumberForm           = NumberForm.Ordinal;
                formatString.Arguments [ index ].AvailablePluralForms = availablePluralForms [ index ];
            }

            Assert.Equal ( expectedPluralForms, PluralRules.Invariant.SelectPluralForms ( formatString, arguments ) );
        }
    }
}
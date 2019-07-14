using Xunit;

namespace Localizer.Tests
{
    public class PluralizerTests
    {
        [ Theory ]
        [ InlineData ( "",             new PluralForm [ ] { }, new object [ ] { }, new PluralForm [ ] { } ) ]
        [ InlineData ( "{0}",          new [ ] { PluralForm.Other }, new object [ ] { 0.0 }, new [ ] { PluralForm.Other } ) ]
        [ InlineData ( "{0:N0}",       new [ ] { PluralForm.Other }, new object [ ] { 0.0 }, new [ ] { PluralForm.Other } ) ]
        [ InlineData ( "{0:N0}",       new [ ] { PluralForm.Zero  }, new object [ ] { 0.0 }, new [ ] { PluralForm.Zero  } ) ]
        [ InlineData ( "{0:N1}",       new [ ] { PluralForm.Zero  }, new object [ ] { 0.0 }, new [ ] { PluralForm.Other } ) ]
        [ InlineData ( "{0:N0}",       new [ ] { PluralForm.One   }, new object [ ] { 1.0 }, new [ ] { PluralForm.One   } ) ]
        [ InlineData ( "{0:N1}",       new [ ] { PluralForm.One   }, new object [ ] { 1.0 }, new [ ] { PluralForm.Other } ) ]
        [ InlineData ( "{0:N0}{1:N0}", new [ ] { PluralForm.Other, PluralForm.Other }, new object [ ] { 0.0, 1.0 }, new [ ] { PluralForm.Other, PluralForm.Other } ) ]
        [ InlineData ( "{0:N0}{1:N0}", new [ ] { PluralForm.Zero,  PluralForm.One   }, new object [ ] { 0.0, 1.0 }, new [ ] { PluralForm.Zero,  PluralForm.One   } ) ]
        [ InlineData ( "{0:N1}{1:N1}", new [ ] { PluralForm.Zero,  PluralForm.One   }, new object [ ] { 0.0, 1.0 }, new [ ] { PluralForm.Other, PluralForm.Other } ) ]
        public static void SelectPluralFormsOnInvariantReturnsTheCorrectPluralForms ( string format, PluralForm [ ] availablePluralForms, object [ ] arguments, PluralForm [ ] expectedPluralForms )
        {
            var formatString = FormatString.Parse ( format );

            for ( var index = 0; index < formatString.Arguments.Length; index++ )
                formatString.Arguments [ index ].AvailablePluralForms = availablePluralForms [ index ];

            Assert.Equal ( expectedPluralForms, PluralRules.Invariant.SelectPluralForms ( formatString, arguments ) );
        }

        [ Theory ]
        [ InlineData ( "",             new PluralForm [ ] { }, new object [ ] { }, new PluralForm [ ] { } ) ]
        [ InlineData ( "{0}",          new [ ] { PluralForm.Other }, new object [ ] { 0.0 }, new [ ] { PluralForm.Other } ) ]
        [ InlineData ( "{0:N0}",       new [ ] { PluralForm.Other }, new object [ ] { 0.0 }, new [ ] { PluralForm.Other } ) ]
        [ InlineData ( "{0:N0}",       new [ ] { PluralForm.Zero  }, new object [ ] { 0.0 }, new [ ] { PluralForm.Other } ) ]
        [ InlineData ( "{0:N1}",       new [ ] { PluralForm.Zero  }, new object [ ] { 0.0 }, new [ ] { PluralForm.Other } ) ]
        [ InlineData ( "{0:N0}",       new [ ] { PluralForm.One   }, new object [ ] { 1.0 }, new [ ] { PluralForm.Other } ) ]
        [ InlineData ( "{0:N1}",       new [ ] { PluralForm.One   }, new object [ ] { 1.0 }, new [ ] { PluralForm.Other } ) ]
        [ InlineData ( "{0:N0}{1:N0}", new [ ] { PluralForm.Other, PluralForm.Other }, new object [ ] { 0.0, 1.0 }, new [ ] { PluralForm.Other, PluralForm.Other } ) ]
        [ InlineData ( "{0:N0}{1:N0}", new [ ] { PluralForm.Zero,  PluralForm.One   }, new object [ ] { 0.0, 1.0 }, new [ ] { PluralForm.Other, PluralForm.Other } ) ]
        [ InlineData ( "{0:N1}{1:N1}", new [ ] { PluralForm.Zero,  PluralForm.One   }, new object [ ] { 0.0, 1.0 }, new [ ] { PluralForm.Other, PluralForm.Other } ) ]
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
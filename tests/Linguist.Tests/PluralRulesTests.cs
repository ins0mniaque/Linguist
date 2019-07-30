using System;
using System.Globalization;

using Xunit;

namespace Linguist.Tests
{
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
    }
}
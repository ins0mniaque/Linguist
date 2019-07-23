using Xunit;

namespace Localizer.CLDR.Tests
{
    public class PluralRuleTests
    {
        [ Theory ]
        [ InlineData ( "i = 0 or n = 1 @integer 0, 1 @decimal 0.0~1.0, 0.00~0.04", "i = 0 or n = 1" ) ]
        public static void ParseReturnsTheCorrectRule ( string rule, string expectedRule )
        {
            Assert.Equal ( expectedRule, PluralRule.Parse ( rule ) );
        }
    }
}
using System;

using Xunit;

namespace Linguist.CLDR.Tests
{
    public class PluralRuleTests
    {
        public const string Rule0 = " @integer 3~10, 13~19, 23, 103, 1003, …";
        public const string Rule1 = "i = 0 or n = 1 @integer 0, 1 @decimal 0.0~1.0, 0.00~0.04";
        public const string Rule2 = "v = 0 and i % 10 = 2..4 and i % 100 != 12..14 @integer 2~4, 22~24, 32~34, 42~44, 52~54, 62, 102, 1002, …";
        public const string Rule3 = "n % 10 = 3..4,9 and n % 100 != 10..19,70..79,90..99 @integer 3, 4, 9, 23, 24, 29, 33, 34, 39, 43, 44, 49, 103, 1003, … @decimal 3.0, 4.0, 9.0, 23.0, 24.0, 29.0, 33.0, 34.0, 103.0, 1003.0, …";
        public const string Rule4 = "n % 100 = 2,22,42,62,82 or n%1000 = 0 and n%100000=1000..20000,40000,60000,80000 or n!=0 and n%1000000=100000@integer 2, 22, 42, 62, 82, 102, 122, 142, 1002, … @decimal 2.0, 22.0, 42.0, 62.0, 82.0, 102.0, 122.0, 142.0, 1002.0, …";
        public const string Rule5 = "v = 0 and i = 1,2,3 or v = 0 and i % 10 != 4,6,9 or v != 0 and f % 10 != 4,6,9 @integer 0~3, 5, 7, 8, 10~13, 15, 17, 18, 20, 21, 100, 1000, 10000, 100000, 1000000, … @decimal 0.0~0.3, 0.5, 0.7, 0.8, 1.0~1.3, 1.5, 1.7, 1.8, 2.0, 2.1, 10.0, 100.0, 1000.0, 10000.0, 100000.0, 1000000.0, …";

        [ Theory ]
        [ InlineData ( "zero",  Rule0, PluralForm.Zero         ) ]
        [ InlineData ( "one",   Rule1, PluralForm.One          ) ]
        [ InlineData ( "two",   Rule2, PluralForm.Two          ) ]
        [ InlineData ( "few",   Rule3, PluralForm.Few          ) ]
        [ InlineData ( "many",  Rule4, PluralForm.Many         ) ]
        [ InlineData ( "other", Rule5, PluralForm.Other        ) ]
        [ InlineData ( "0",     Rule5, PluralForm.ExplicitZero ) ]
        [ InlineData ( "1",     Rule5, PluralForm.ExplicitOne  ) ]
        public static void ParseReturnsTheCorrectPluralForm ( string count, string rule, PluralForm expectedPluralForm )
        {
            Assert.Equal ( expectedPluralForm, PluralRule.Parse ( count, rule, out var _ ).PluralForm );
        }

        [ Theory ]
        [ InlineData ( "",       Rule0 ) ]
        [ InlineData ( " ",      Rule1 ) ]
        [ InlineData ( "2",      Rule2 ) ]
        [ InlineData ( "-1",     Rule3 ) ]
        [ InlineData ( "none",   Rule4 ) ]
        [ InlineData ( "others", Rule5 ) ]
        public static void ParseThrowsOnInvalidPluralForm ( string count, string rule )
        {
            Assert.Throws < FormatException > ( ( ) => PluralRule.Parse ( count, rule, out var _ ) );
        }

        [ Theory ]
        [ InlineData ( "zero",  Rule0, null ) ]
        [ InlineData ( "one",   Rule1, "i == 0m || n == 1m" ) ]
        [ InlineData ( "two",   Rule2, "v == 0m && ( i % 10m ).between ( 2m, 4m ) && ! ( i % 100m ).between ( 12m, 14m )" ) ]
        [ InlineData ( "few",   Rule3, "( ( n % 10m ).between ( 3m, 4m ) || n % 10m == 9m ) && ! ( n % 100m ).between ( 10m, 19m, 70m, 79m, 90m, 99m )" ) ]
        [ InlineData ( "many",  Rule4, "( n % 100m ).equals ( 2m, 22m, 42m, 62m, 82m ) || n % 1000m == 0m && ( ( n % 100000m ).between ( 1000m, 20000m ) || ( n % 100000m ).equals ( 40000m, 60000m, 80000m ) ) || n != 0m && n % 1000000m == 100000m" ) ]
        [ InlineData ( "other", Rule5, "v == 0m && i.equals ( 1m, 2m, 3m ) || v == 0m && ! ( i % 10m ).equals ( 4m, 6m, 9m ) || v != 0m && ! ( f % 10m ).equals ( 4m, 6m, 9m )" ) ]
        public static void ParseReturnsTheCorrectRule ( string count, string rule, string expectedExpression )
        {
            Assert.Equal ( expectedExpression, PluralRule.Parse ( count, rule, out var _ ).Rule?.ToString ( ) );
        }

        [ Theory ]
        [ InlineData ( "zero",  Rule0, null          ) ]
        [ InlineData ( "one",   Rule1, "i", "n"      ) ]
        [ InlineData ( "two",   Rule2, "v", "i"      ) ]
        [ InlineData ( "few",   Rule3, "n"           ) ]
        [ InlineData ( "many",  Rule4, "n"           ) ]
        [ InlineData ( "other", Rule5, "v", "i", "f" ) ]
        public static void ParseReturnsTheCorrectOperands ( string count, string rule, params string [ ] expectedOperands )
        {
            PluralRule.Parse ( count, rule, out var operands );

            Assert.Equal ( expectedOperands, operands );
        }

        [ Theory ]
        [ InlineData ( "zero",  Rule0, "" ) ]
        [ InlineData ( "one",   Rule1, "i = 0 or n = 1" ) ]
        [ InlineData ( "two",   Rule2, "v = 0 and i % 10 = 2..4 and i % 100 != 12..14" ) ]
        [ InlineData ( "few",   Rule3, "n % 10 = 3..4,9 and n % 100 != 10..19,70..79,90..99" ) ]
        [ InlineData ( "many",  Rule4, "n % 100 = 2,22,42,62,82 or n%1000 = 0 and n%100000=1000..20000,40000,60000,80000 or n!=0 and n%1000000=100000" ) ]
        [ InlineData ( "other", Rule5, "v = 0 and i = 1,2,3 or v = 0 and i % 10 != 4,6,9 or v != 0 and f % 10 != 4,6,9" ) ]
        public static void ParseReturnsTheCorrectRuleCode ( string count, string rule, string expectedRuleCode )
        {
            Assert.Equal ( expectedRuleCode, PluralRule.Parse ( count, rule, out var _ ).RuleCode );
        }

        [ Theory ]
        [ InlineData ( "zero",  Rule0, "3~10", "13~19", "23", "103", "1003", "…" ) ]
        [ InlineData ( "one",   Rule1, "0", "1" ) ]
        [ InlineData ( "two",   Rule2, "2~4", "22~24", "32~34", "42~44", "52~54", "62", "102", "1002", "…" ) ]
        [ InlineData ( "few",   Rule3, "3", "4", "9", "23", "24", "29", "33", "34", "39", "43", "44", "49", "103", "1003", "…" ) ]
        [ InlineData ( "many",  Rule4, "2", "22", "42", "62", "82", "102", "122", "142", "1002", "…" ) ]
        [ InlineData ( "other", Rule5, "0~3", "5", "7", "8", "10~13", "15", "17", "18", "20", "21", "100", "1000", "10000", "100000", "1000000", "…" ) ]
        public static void ParseReturnsTheCorrectIntegerSamples ( string count, string rule, params string [ ] expectedSamples )
        {
            Assert.Equal ( expectedSamples, PluralRule.Parse ( count, rule, out var _ ).IntegerSamples );
        }

        [ Theory ]
        [ InlineData ( "zero",  Rule0, null ) ]
        [ InlineData ( "one",   Rule1, "0.0~1.0", "0.00~0.04" ) ]
        [ InlineData ( "two",   Rule2, null ) ]
        [ InlineData ( "few",   Rule3, "3.0", "4.0", "9.0", "23.0", "24.0", "29.0", "33.0", "34.0", "103.0", "1003.0", "…" ) ]
        [ InlineData ( "many",  Rule4, "2.0", "22.0", "42.0", "62.0", "82.0", "102.0", "122.0", "142.0", "1002.0", "…" ) ]
        [ InlineData ( "other", Rule5, "0.0~0.3", "0.5", "0.7", "0.8", "1.0~1.3", "1.5", "1.7", "1.8", "2.0", "2.1", "10.0", "100.0", "1000.0", "10000.0", "100000.0", "1000000.0", "…" ) ]
        public static void ParseReturnsTheCorrectDecimalSamples ( string count, string rule, params string [ ] expectedSamples )
        {
            Assert.Equal ( expectedSamples, PluralRule.Parse ( count, rule, out var _ ).DecimalSamples );
        }
    }
}
using System.Globalization;

using Xunit;

namespace Localizer.Plural.Tests
{
    using static PluralOperand;

    public class PluralOperandTests
    {
        [ Theory ]
        [ InlineData ( "0",      "0"     ) ]
        [ InlineData ( "0.0",    "0.0"   ) ]
        [ InlineData ( "0.00",   "0.00"  ) ]
        [ InlineData ( "1.0",    "1.0"   ) ]
        [ InlineData ( "1.00",   "1.00"  ) ]
        [ InlineData ( "1.9",    "1.9"   ) ]
        [ InlineData ( "1.90",   "1.90"  ) ]
        [ InlineData ( "1.09",   "1.09"  ) ]
        [ InlineData ( "1.999",  "1.999" ) ]
        [ InlineData ( "-1.0",   "1.0"   ) ]
        [ InlineData ( "-1.00",  "1.00"  ) ]
        [ InlineData ( "-1.9",   "1.9"   ) ]
        [ InlineData ( "-1.90",  "1.90"  ) ]
        [ InlineData ( "-1.09",  "1.09"  ) ]
        [ InlineData ( "-1.999", "1.999" ) ]
        public static void OperandNReturnsTheCorrectResult ( string number, string expected )
        {
            var result = n ( decimal.Parse ( number, CultureInfo.InvariantCulture ) );

            Assert.Equal ( expected, result.ToString ( CultureInfo.InvariantCulture ) );
        }

        [ Theory ]
        [ InlineData ( "0",      "0" ) ]
        [ InlineData ( "0.0",    "0" ) ]
        [ InlineData ( "0.00",   "0" ) ]
        [ InlineData ( "1.0",    "1" ) ]
        [ InlineData ( "1.00",   "1" ) ]
        [ InlineData ( "1.9",    "1" ) ]
        [ InlineData ( "1.90",   "1" ) ]
        [ InlineData ( "1.09",   "1" ) ]
        [ InlineData ( "1.999",  "1" ) ]
        [ InlineData ( "-1.0",   "1" ) ]
        [ InlineData ( "-1.00",  "1" ) ]
        [ InlineData ( "-1.9",   "1" ) ]
        [ InlineData ( "-1.90",  "1" ) ]
        [ InlineData ( "-1.09",  "1" ) ]
        [ InlineData ( "-1.999", "1" ) ]
        public static void OperandIReturnsTheCorrectResult ( string number, string expected )
        {
            var result = i ( decimal.Parse ( number, CultureInfo.InvariantCulture ) );

            Assert.Equal ( expected, result.ToString ( CultureInfo.InvariantCulture ) );
        }

        [ Theory ]
        [ InlineData ( "0",      "0" ) ]
        [ InlineData ( "0.0",    "1" ) ]
        [ InlineData ( "0.00",   "2" ) ]
        [ InlineData ( "1.0",    "1" ) ]
        [ InlineData ( "1.00",   "2" ) ]
        [ InlineData ( "1.9",    "1" ) ]
        [ InlineData ( "1.90",   "2" ) ]
        [ InlineData ( "1.09",   "2" ) ]
        [ InlineData ( "1.999",  "3" ) ]
        [ InlineData ( "-1.0",   "1" ) ]
        [ InlineData ( "-1.00",  "2" ) ]
        [ InlineData ( "-1.9",   "1" ) ]
        [ InlineData ( "-1.90",  "2" ) ]
        [ InlineData ( "-1.09",  "2" ) ]
        [ InlineData ( "-1.999", "3" ) ]
        public static void OperandVReturnsTheCorrectResult ( string number, string expected )
        {
            var result = v ( decimal.Parse ( number, CultureInfo.InvariantCulture ) );

            Assert.Equal ( expected, result.ToString ( CultureInfo.InvariantCulture ) );
        }

        [ Theory ]
        [ InlineData ( "0",      "0" ) ]
        [ InlineData ( "0.0",    "0" ) ]
        [ InlineData ( "0.00",   "0" ) ]
        [ InlineData ( "1.0",    "0" ) ]
        [ InlineData ( "1.00",   "0" ) ]
        [ InlineData ( "1.9",    "1" ) ]
        [ InlineData ( "1.90",   "1" ) ]
        [ InlineData ( "1.09",   "2" ) ]
        [ InlineData ( "1.999",  "3" ) ]
        [ InlineData ( "-1.0",   "0" ) ]
        [ InlineData ( "-1.00",  "0" ) ]
        [ InlineData ( "-1.9",   "1" ) ]
        [ InlineData ( "-1.90",  "1" ) ]
        [ InlineData ( "-1.09",  "2" ) ]
        [ InlineData ( "-1.999", "3" ) ]
        public static void OperandWReturnsTheCorrectResult ( string number, string expected )
        {
            var result = w ( decimal.Parse ( number, CultureInfo.InvariantCulture ) );

            Assert.Equal ( expected, result.ToString ( CultureInfo.InvariantCulture ) );
        }

        [ Theory ]
        [ InlineData ( "0",      "0"     ) ]
        [ InlineData ( "0.0",    "0.0"   ) ]
        [ InlineData ( "0.00",   "0.00"  ) ]
        [ InlineData ( "1.0",    "0.0"   ) ]
        [ InlineData ( "1.00",   "0.00"  ) ]
        [ InlineData ( "1.9",    "0.9"   ) ]
        [ InlineData ( "1.90",   "0.90"  ) ]
        [ InlineData ( "1.09",   "0.09"  ) ]
        [ InlineData ( "1.999",  "0.999" ) ]
        [ InlineData ( "-1.0",   "0.0"   ) ]
        [ InlineData ( "-1.00",  "0.00"  ) ]
        [ InlineData ( "-1.9",   "0.9"   ) ]
        [ InlineData ( "-1.90",  "0.90"  ) ]
        [ InlineData ( "-1.09",  "0.09"  ) ]
        [ InlineData ( "-1.999", "0.999" ) ]
        public static void OperandFReturnsTheCorrectResult ( string number, string expected )
        {
            var result = f ( decimal.Parse ( number, CultureInfo.InvariantCulture ) );

            Assert.Equal ( expected, result.ToString ( CultureInfo.InvariantCulture ) );
        }

        [ Theory ]
        [ InlineData ( "0",      "0"     ) ]
        [ InlineData ( "0.0",    "0"     ) ]
        [ InlineData ( "0.00",   "0"     ) ]
        [ InlineData ( "1.0",    "0"     ) ]
        [ InlineData ( "1.00",   "0"     ) ]
        [ InlineData ( "1.9",    "0.9"   ) ]
        [ InlineData ( "1.90",   "0.9"   ) ]
        [ InlineData ( "1.09",   "0.09"  ) ]
        [ InlineData ( "1.999",  "0.999" ) ]
        [ InlineData ( "-1.0",   "0"     ) ]
        [ InlineData ( "-1.00",  "0"     ) ]
        [ InlineData ( "-1.9",   "0.9"   ) ]
        [ InlineData ( "-1.90",  "0.9"   ) ]
        [ InlineData ( "-1.09",  "0.09"  ) ]
        [ InlineData ( "-1.999", "0.999" ) ]
        public static void OperandTReturnsTheCorrectResult ( string number, string expected )
        {
            var result = t ( decimal.Parse ( number, CultureInfo.InvariantCulture ) );

            Assert.Equal ( expected, result.ToString ( CultureInfo.InvariantCulture ) );
        }
    }
}
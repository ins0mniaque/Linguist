using System.Globalization;

using Xunit;

namespace Localizer.CLDR.Tests
{
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
            var result = decimal.Parse ( number, CultureInfo.InvariantCulture ).n ( );

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
            var result = decimal.Parse ( number, CultureInfo.InvariantCulture ).i ( );

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
            var result = decimal.Parse ( number, CultureInfo.InvariantCulture ).v ( );

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
            var result = decimal.Parse ( number, CultureInfo.InvariantCulture ).w ( );

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
            var result = decimal.Parse ( number, CultureInfo.InvariantCulture ).f ( );

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
            var result = decimal.Parse ( number, CultureInfo.InvariantCulture ).t ( );

            Assert.Equal ( expected, result.ToString ( CultureInfo.InvariantCulture ) );
        }
    }
}
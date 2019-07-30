using System;
using System.Globalization;

using Xunit;

namespace Linguist.Tests
{
    public class FormattedNumberTests
    {
        [ Theory ]
        [ InlineData ( "{0}",         0, "Not a number",   null         ) ]
        [ InlineData ( "{0}",         0,  1,               "1"          ) ]
        [ InlineData ( "{0:N0}",      0,  1,               "1"          ) ]
        [ InlineData ( "{0,42:N0}",   0,  1,               "1"          ) ]
        [ InlineData ( "{0,-42:N0}",  0,  1,               "1"          ) ]
        [ InlineData ( "{0:N1}",      0,  1,               "1.0"        ) ]
        [ InlineData ( "{0:N2}",      0,  1,               "1.00"       ) ]
        [ InlineData ( "{0:N4}",      0,  1,               "1.0000"     ) ]
        [ InlineData ( "{0:N8}",      0,  1,               "1.00000000" ) ]
        [ InlineData ( "{0:X}",       0, 42,               "42"         ) ]
        [ InlineData ( "{0:X2}",      0, 42,               "42"         ) ]
        [ InlineData ( "{0:X4}",      0, 42,               "42"         ) ]
        [ InlineData ( "{0:X8}",      0, 42,               "42"         ) ]
        [ InlineData ( "{0:P}",       0,  0.55,            "55.00"      ) ]
        [ InlineData ( "{0:P}",       0,  0.555,           "55.50"      ) ]
        [ InlineData ( "{0:P0}",      0,  0.55,            "55"         ) ]
        [ InlineData ( "{0:P0}",      0,  0.555,           "56"         ) ]
        [ InlineData ( "{0:#0.## ‰}", 0,  0.55,            "550"        ) ]
        [ InlineData ( "{0:#0.## ‰}", 0,  0.555,           "555"        ) ]
        [ InlineData ( "{0:#0.## ‰}", 0,  0.5555,          "555.5"      ) ]
        [ InlineData ( "{0:#0.## ‰}", 0,  0.55555,         "555.55"     ) ]
        [ InlineData ( "{0:#0.## ‰}", 0,  0.555555,        "555.56"     ) ]
        [ InlineData ( "{0:#0‰}",     0,  0.55,            "550"        ) ]
        [ InlineData ( "{0:#0‰}",     0,  0.555,           "555"        ) ]
        [ InlineData ( "{0:G}",       0, -1.234567890e-20, "-0.0000000000000000000123456789" ) ]
        public static void ParseReturnsTheCorrectNumber ( string format, int argumentIndex, object value, string expectedNumber )
        {
            var number = FormattedNumber.Parse ( CultureInfo.InvariantCulture, format, argumentIndex, value );

            Assert.Equal ( expectedNumber, number?.ToString ( CultureInfo.InvariantCulture ) );
        }

        [ Theory ]
        [ InlineData ( "{0:N0} items",         0,  1, "1"   ) ]
        [ InlineData ( "{0:N1} pixels",        0,  1, "1.0" ) ]
        [ InlineData ( "{0:N8}/{1:N0} items",  1,  1, "1"   ) ]
        [ InlineData ( "{0:N8}/{1:N1} pixels", 1,  1, "1.0" ) ]
        public static void ParseExtractsTheSpecifiedArgument ( string format, int argumentIndex, object value, string expectedNumber )
        {
            var number = FormattedNumber.Parse ( CultureInfo.InvariantCulture, format, argumentIndex, value );

            Assert.Equal ( expectedNumber, number?.ToString ( CultureInfo.InvariantCulture ) );
        }

        [ Theory ]
        [ InlineData ( "",               0 ) ]
        [ InlineData ( "{0:N0}",         1 ) ]
        [ InlineData ( "{0:N1}",        -1 ) ]
        [ InlineData ( "{0:N8}/{1:N0}",  2 ) ]
        [ InlineData ( "{0:N8}/{1:N1}", 42 ) ]
        public static void ParseThrowsOnOutOfRangeArgumentIndex ( string format, int argumentIndex )
        {
            Assert.Throws < ArgumentOutOfRangeException > ( ( ) => FormattedNumber.Parse ( CultureInfo.InvariantCulture, format, argumentIndex, 1 ) );
        }
    }
}
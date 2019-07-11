using System;
using System.Linq;

using Xunit;

namespace Localizer.Tests
{
    public class FormatStringTests
    {
        [ Fact ]
        public void ParseThrowsOnNull ( )
        {
            Assert.Throws < ArgumentNullException > ( ( ) => FormatString.Parse ( null ) );
        }

        [ Fact ]
        public void TryParseReturnsFalseOnNull ( )
        {
            Assert.False ( FormatString.TryParse ( null, out _ ) );
        }

        [ Theory ]
        [ InlineData ( 0,  0, null, 0, 2, "{0}",      "{0}@[0..2]"      ) ]
        [ InlineData ( 42, 0, null, 0, 3, "{42}",     "{42}@[0..3]"     ) ]
        [ InlineData ( 0,  0, "",   0, 2, "{0}",      "{0}@[0..2]"      ) ]
        [ InlineData ( 0, -1, null, 0, 5, "{0,-1}",   "{0,-1}@[0..5]"   ) ]
        [ InlineData ( 0,  1, null, 0, 4, "{0,1}",    "{0,1}@[0..4]"    ) ]
        [ InlineData ( 0,  0, "N6", 0, 5, "{0:N6}",   "{0:N6}@[0..5]"   ) ]
        [ InlineData ( 0,  1, "N6", 0, 7, "{0,1:N6}", "{0,1:N6}@[0..7]" ) ]
        public static void ArgumentHoleHasCorrectStringRepresention ( int index, int alignment, string format, int startOffset, int endOffset, string expectedFormatString, string expectedString )
        {
            var argumentHole = new FormatString.ArgumentHole ( index, alignment, format, startOffset, endOffset );

            Assert.Equal ( expectedFormatString, argumentHole.ToFormatString ( ) );
            Assert.Equal ( expectedString,       argumentHole.ToString       ( ) );
        }

        [ Theory ]
        [ InlineData ( ""     ) ]
        [ InlineData ( "Item" ) ]
        [ InlineData ( "{{"   ) ]
        [ InlineData ( "}}"   ) ]
        [ InlineData ( "{{}}" ) ]
        [ InlineData ( "}}{{" ) ]
        [ InlineData ( "{{{42}" ,   "{42}@[2..5]" ) ]
        [ InlineData ( "{42}}}" ,   "{42}@[0..3]" ) ]
        [ InlineData ( "{{{42}}}" , "{42}@[2..5]" ) ]
        [ InlineData ( "{0}",       "{0}@[0..2]"  ) ]
        [ InlineData ( "{0} items", "{0}@[0..2]"  ) ]
        [ InlineData ( "Item {0}",  "{0}@[5..7]"  ) ]
        [ InlineData ( "{0}{0}{0}", "{0}@[0..2]",
                                    "{0}@[3..5]",
                                    "{0}@[6..8]"  ) ]
        [ InlineData ( "{0}{1}{2}", "{0}@[0..2]",
                                    "{1}@[3..5]",
                                    "{2}@[6..8]"  ) ]
        [ InlineData ( "{999999}",  "{999999}@[0..7]" ) ]
        [ InlineData ( "{42,-34:N6}",          "{42,-34:N6}@[0..10]"     ) ]
        [ InlineData ( "{42  ,  -34   : N6 }", "{42,-34: N6 }@[0..19]"   ) ]
        [ InlineData ( "{42,256:N6}",          "{42,256:N6}@[0..10]"     ) ]
        [ InlineData ( "{42  ,  256   : N6 }", "{42,256: N6 }@[0..19]"   ) ]
        [ InlineData ( "{42,-999999:N6}",      "{42,-999999:N6}@[0..14]" ) ]
        [ InlineData ( "{42,999999:N6}",       "{42,999999:N6}@[0..13]"  ) ]
        public static void ParseReturnsTheCorrectFormatString ( string format, params string [ ] expectedArgumentHoles )
        {
            var formatString  = FormatString.Parse ( format );
            var argumentHoles = formatString.ArgumentHoles
                                            .Select  ( argumentHole => argumentHole.ToString ( ) )
                                            .ToArray ( );

            Assert.Equal ( expectedArgumentHoles, argumentHoles );
        }

        [ Theory ]
        [ InlineData ( "{",                     "Missing closing brace at position '0'." ) ]
        [ InlineData ( "{0",                    "Missing closing brace at position '0'." ) ]
        [ InlineData ( "{0,",                   "Missing closing brace at position '0'." ) ]
        [ InlineData ( "{0,-42",                "Missing closing brace at position '0'." ) ]
        [ InlineData ( "{0:N6",                 "Missing closing brace at position '0'." ) ]
        [ InlineData ( "{0,-42:N6",             "Missing closing brace at position '0'." ) ]
        [ InlineData ( "}",                     "Mismatched closing brace at position '0'." ) ]
        [ InlineData ( "{0}}",                  "Mismatched closing brace at position '3'." ) ]
        [ InlineData ( "{0:{",                  "Invalid opening brace '{' inside argument at position '3'." ) ]
        [ InlineData ( "{1000000}",             "Invalid argument index at position '0'." ) ]
        [ InlineData ( "{10000000}",            "Invalid argument index at position '0'." ) ]
        [ InlineData ( "{0,1000000}",           "Invalid alignment width at position '3'." ) ]
        [ InlineData ( "{0,10000000}",          "Invalid alignment width at position '3'." ) ]
        [ InlineData ( "{X}",                   "Unexpected character 'X' encountered at position '1'." ) ]
        [ InlineData ( "{0X}",                  "Unexpected character 'X' encountered at position '2'." ) ]
        [ InlineData ( "{0,X}",                 "Unexpected character 'X' encountered at position '3'." ) ]
        [ InlineData ( "{999999X}",             "Unexpected character 'X' encountered at position '7'." ) ]
        [ InlineData ( "{{{42}}} {",            "Missing closing brace at position '9'." ) ]
        [ InlineData ( "{{{42}}} {0",           "Missing closing brace at position '9'." ) ]
        [ InlineData ( "{{{42}}} {0,",          "Missing closing brace at position '9'." ) ]
        [ InlineData ( "{{{42}}} {0,-42",       "Missing closing brace at position '9'." ) ]
        [ InlineData ( "{{{42}}} {0:N6",        "Missing closing brace at position '9'." ) ]
        [ InlineData ( "{{{42}}} {0,-42:N6",    "Missing closing brace at position '9'." ) ]
        [ InlineData ( "{{{42}}} }",            "Mismatched closing brace at position '9'." ) ]
        [ InlineData ( "{{{42}}} {0}}",         "Mismatched closing brace at position '12'." ) ]
        [ InlineData ( "{{{42}}} {0:{",         "Invalid opening brace '{' inside argument at position '12'." ) ]
        [ InlineData ( "{{{42}}} {1000000}",    "Invalid argument index at position '9'." ) ]
        [ InlineData ( "{{{42}}} {10000000}",   "Invalid argument index at position '9'." ) ]
        [ InlineData ( "{{{42}}} {0,1000000}",  "Invalid alignment width at position '12'." ) ]
        [ InlineData ( "{{{42}}} {0,10000000}", "Invalid alignment width at position '12'." ) ]
        [ InlineData ( "{{{42}}} {X}",          "Unexpected character 'X' encountered at position '10'." ) ]
        [ InlineData ( "{{{42}}} {0X}",         "Unexpected character 'X' encountered at position '11'." ) ]
        [ InlineData ( "{{{42}}} {0,X}",        "Unexpected character 'X' encountered at position '12'." ) ]
        [ InlineData ( "{{{42}}} {999999X}",    "Unexpected character 'X' encountered at position '16'." ) ]
        public static void ParseThrowsTheCorrectFormatException ( string format, string expectedError )
        {
            var exception = Assert.Throws < FormatException > ( ( ) => FormatString.Parse ( format ) );

            Assert.Equal ( expectedError, exception.Message );
        }
    }
}
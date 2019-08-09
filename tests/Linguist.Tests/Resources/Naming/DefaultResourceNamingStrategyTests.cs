using System.Linq;

using Xunit;

namespace Linguist.Resources.Naming.Tests
{
    public class DefaultResourceNamingStrategyTests
    {
        [ Theory ]
        [ InlineData ( "Name",                "Name" ) ]
        [ InlineData ( "Name.Zero",           "Name", "{}: Cardinal, Zero"  ) ]
        [ InlineData ( "Name.One",            "Name", "{}: Cardinal, One"   ) ]
        [ InlineData ( "Name.Many",           "Name", "{}: Cardinal, Many"  ) ]
        [ InlineData ( "Name._",              "Name", "{}: Cardinal, Other" ) ]
        [ InlineData ( "Name.OrdinalOne",     "Name", "{}: Ordinal, One"    ) ]
        [ InlineData ( "Name.OrdinalTwo",     "Name", "{}: Ordinal, Two"    ) ]
        [ InlineData ( "Name.Range",          "Name.Range" ) ]
        [ InlineData ( "Name.Range.One",      "Name", "{}: Cardinal, Range, One",   "{}: Cardinal, Range, One"   ) ]
        [ InlineData ( "Name.Range._",        "Name", "{}: Cardinal, Range, Other", "{}: Cardinal, Range, Other" ) ]
        [ InlineData ( "Name.One.Two",        "Name", "{}: Cardinal, One",   "{}: Cardinal, Two" ) ]
        [ InlineData ( "Name.One.OrdinalTwo", "Name", "{}: Cardinal, One",   "{}: Ordinal, Two"  ) ]
        [ InlineData ( "Name._.OrdinalTwo",   "Name", "{}: Cardinal, Other", "{}: Ordinal, Two"  ) ]
        [ InlineData ( "Name._.Range",        "Name._.Range" ) ]
        [ InlineData ( "Name.Few.Range.One",  "Name", "{}: Cardinal, Few",  "{}: Cardinal, Range, One",   "{}: Cardinal, Range, One"   ) ]
        [ InlineData ( "Name.Many.Range._",   "Name", "{}: Cardinal, Many", "{}: Cardinal, Range, Other", "{}: Cardinal, Range, Other" ) ]
        public static void ParseArgumentsReturnsTheCorrectArguments ( string resourceName, string expectedName, params string [ ] expectedArguments )
        {
            var name = ResourceNamingStrategy.Default.ParseArguments ( PluralRules.Invariant, resourceName, out var arguments );

            Assert.Equal ( expectedName,      name );
            Assert.Equal ( expectedArguments, arguments.Select ( argument => argument.ToString ( ) ) );
        }
    }
}
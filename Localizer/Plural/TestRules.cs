namespace Localizer.Plural
{
    using static System.Decimal;
    using static PluralOperand;

    // <pluralRule count="one">i = 1 and v = 0 @integer 1</pluralRule>
    // <pluralRule count="other"> @integer 0, 2~16, 100, 1000, 10000, 100000, 1000000, … @decimal 0.0~1.5, 10.0, 100.0, 1000.0, 10000.0, 100000.0, 1000000.0, …</pluralRule>
    public sealed class EnglishOneRule : OneRule
    {
        public override bool AppliesTo ( decimal number )
        {
            return Rule ( i ( number ), v ( number ) );
        }

        private static bool Rule ( decimal i, decimal v ) => i == One && v == Zero;
    }

    // <pluralRule count="one">i = 0,1 @integer 0, 1 @decimal 0.0~1.5</pluralRule>
    // <pluralRule count="other"> @integer 2~17, 100, 1000, 10000, 100000, 1000000, … @decimal 2.0~3.5, 10.0, 100.0, 1000.0, 10000.0, 100000.0, 1000000.0, …</pluralRule>
    public sealed class FrenchOneRule : OneRule
    {
        public override bool AppliesTo ( decimal number )
        {
            return Rule ( i ( number ) );
        }

        private static bool Rule ( decimal i ) => i == Zero || i == One;
    }
}
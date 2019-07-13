namespace Localizer.Plural
{
    using static System.Decimal;
    using static PluralOperand;

    public abstract class ZeroRule : PluralRule
    {
        public ZeroRule ( ) : base ( PluralForm.Zero ) { }
    }

    public abstract class OneRule : PluralRule
    {
        public OneRule ( ) : base ( PluralForm.One ) { }
    }

    public abstract class TwoRule : PluralRule
    {
        public TwoRule ( ) : base ( PluralForm.Two ) { }
    }

    public abstract class FewRule : PluralRule
    {
        public FewRule ( ) : base ( PluralForm.Few ) { }
    }

    public abstract class ManyRule : PluralRule
    {
        public ManyRule ( ) : base ( PluralForm.Many ) { }
    }

    public abstract class OtherRule : PluralRule
    {
        public OtherRule ( ) : base ( PluralForm.Other ) { }
    }

    public sealed class ImplicitZeroRule : ZeroRule
    {
        public override bool AppliesTo ( decimal number )
        {
            return Rule ( i ( number ), v ( number ) );
        }

        private static bool Rule ( decimal i, decimal v ) => i == Zero && v == Zero;
    }

    public sealed class ImplicitOneRule : OneRule
    {
        public override bool AppliesTo ( decimal number )
        {
            return Rule ( i ( number ), v ( number ) );
        }

        private static bool Rule ( decimal i, decimal v ) => i == One && v == Zero;
    }
}
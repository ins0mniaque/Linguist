namespace Localizer.Plural
{
    using Localizer.CLDR;

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
            return Rule ( number.i ( ), number.v ( ) );
        }

        private static bool Rule ( decimal i, decimal v ) => i == 0m && v == 0m;
    }

    public sealed class ImplicitOneRule : OneRule
    {
        public override bool AppliesTo ( decimal number )
        {
            return Rule ( number.i ( ), number.v ( ) );
        }

        private static bool Rule ( decimal i, decimal v ) => i == 1m && v == 0m;
    }
}
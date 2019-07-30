namespace Linguist.Pluralization
{
    using Linguist.CLDR;

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

    public sealed class ExplicitZeroRule : PluralRule
    {
        public ExplicitZeroRule ( ) : base ( PluralForm.ExplicitZero ) { }

        public override bool AppliesTo ( decimal number ) => ExplicitRule.Zero ( number.i ( ), number.v ( ) );
    }

    public sealed class ExplicitOneRule : PluralRule
    {
        public ExplicitOneRule ( ) : base ( PluralForm.ExplicitOne ) { }

        public override bool AppliesTo ( decimal number ) => ExplicitRule.One ( number.i ( ), number.v ( ) );
    }
}
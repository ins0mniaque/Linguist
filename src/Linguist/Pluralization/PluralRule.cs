namespace Linguist.Pluralization
{
    public abstract class PluralRule
    {
        public static PluralRule Default { get; } = new DefaultPluralRule ( );

        protected PluralRule ( PluralForm pluralForm ) { PluralForm = pluralForm; }

        public PluralForm PluralForm { get; }

        public abstract bool AppliesTo ( decimal number );
    }

    public sealed class DefaultPluralRule : OtherRule
    {
        public override bool AppliesTo ( decimal number ) => true;
    }
}
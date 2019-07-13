using System;

namespace Localizer.Plural
{
    public class CardinalRuleSet : PluralRuleSet
    {
        public static CardinalRuleSet Default { get; } = new CardinalRuleSet ( Array.Empty < PluralRule > ( ) );

        public static PluralRule ImplicitZeroRule { get; } = new ImplicitZeroRule ( );
        public static PluralRule ImplicitOneRule  { get; } = new ImplicitOneRule  ( );

        private readonly bool hasZeroForm;
        private readonly bool hasOneForm;

        public CardinalRuleSet ( params PluralRule [ ] ruleSet ) : base ( ruleSet )
        {
            hasZeroForm = PluralForms.HasFlag ( PluralForm.Zero );
            hasOneForm  = PluralForms.HasFlag ( PluralForm.One  );
        }

        public override PluralForm SelectPluralForm ( decimal number, PluralForm availablePluralForms )
        {
            if ( ! hasZeroForm && availablePluralForms.HasFlag ( PluralForm.Zero ) )
                if ( ImplicitZeroRule.AppliesTo ( number ) )
                    return ImplicitZeroRule.PluralForm;

            if ( ! hasOneForm && availablePluralForms.HasFlag ( PluralForm.One ) )
                if ( ImplicitOneRule.AppliesTo ( number ) )
                    return ImplicitOneRule.PluralForm;

            return base.SelectPluralForm ( number, availablePluralForms );
        }
    }
}
using System;

namespace Localizer.Pluralization
{
    public class CardinalRuleSet : PluralRuleSet
    {
        public static CardinalRuleSet Default { get; } = new CardinalRuleSet ( Array.Empty < PluralRule > ( ) );

        public static PluralRule ExplicitZeroRule { get; } = new ExplicitZeroRule ( );
        public static PluralRule ExplicitOneRule  { get; } = new ExplicitOneRule  ( );

        private readonly bool hasZeroForm;
        private readonly bool hasOneForm;

        public CardinalRuleSet ( params PluralRule [ ] ruleSet ) : base ( ruleSet )
        {
            hasZeroForm = PluralForms.HasFlag ( PluralForm.Zero );
            hasOneForm  = PluralForms.HasFlag ( PluralForm.One  );
        }

        public override PluralForm SelectPluralForm ( decimal number, PluralForm availablePluralForms )
        {
            if ( availablePluralForms.HasFlag ( PluralForm.ExplicitZero ) )
            {
                if ( ExplicitZeroRule.AppliesTo ( number ) )
                    return ExplicitZeroRule.PluralForm;
            }
            else if ( ! hasZeroForm && availablePluralForms.HasFlag ( PluralForm.Zero ) )
            {
                if ( ExplicitZeroRule.AppliesTo ( number ) )
                    return PluralForm.Zero;
            }

            if ( availablePluralForms.HasFlag ( PluralForm.ExplicitOne ) )
            {
                if ( ExplicitOneRule.AppliesTo ( number ) )
                    return ExplicitOneRule.PluralForm;
            }
            else if ( ! hasOneForm && availablePluralForms.HasFlag ( PluralForm.One ) )
            {
                if ( ExplicitOneRule.AppliesTo ( number ) )
                    return PluralForm.One;
            }

            return base.SelectPluralForm ( number, availablePluralForms );
        }
    }
}
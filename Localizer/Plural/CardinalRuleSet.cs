using System;

namespace Localizer.Plural
{
    public class CardinalRuleSet : PluralFormSelector
    {
        public static CardinalRuleSet Default { get; } = new CardinalRuleSet ( Array.Empty < PluralRule > ( ) );

        public static PluralRule ImplicitZeroRule { get; } = new ImplicitZeroRule ( );
        public static PluralRule ImplicitOneRule  { get; } = new ImplicitOneRule  ( );

        private readonly PluralRule [ ] rules;
        private readonly bool           hasZeroForm;
        private readonly bool           hasOneForm;

        public CardinalRuleSet ( params PluralRule [ ] ruleSet )
        {
            rules = ruleSet ?? Array.Empty < PluralRule > ( );

            var pluralForms = default ( PluralForm );
            for ( var index = 0; index < ruleSet.Length; index++ )
                pluralForms |= ruleSet [ index ].PluralForm;

            hasZeroForm = pluralForms.HasFlag ( PluralForm.Zero );
            hasOneForm  = pluralForms.HasFlag ( PluralForm.One  );
        }

        public override PluralForm SelectPluralForm ( decimal number, PluralForm availablePluralForms )
        {
            if ( ! hasZeroForm && availablePluralForms.HasFlag ( PluralForm.Zero ) )
                if ( ImplicitZeroRule.AppliesTo ( number ) )
                    return ImplicitZeroRule.PluralForm;

            if ( ! hasOneForm && availablePluralForms.HasFlag ( PluralForm.One ) )
                if ( ImplicitOneRule.AppliesTo ( number ) )
                    return ImplicitOneRule.PluralForm;

            foreach ( var rule in rules )
                if ( rule.AppliesTo ( number ) )
                    return rule.PluralForm & availablePluralForms;

            return PluralRule.Default.PluralForm;
        }
    }
}
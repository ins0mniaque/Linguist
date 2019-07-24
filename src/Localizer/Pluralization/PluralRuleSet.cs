using System;

namespace Localizer.Pluralization
{
    public abstract class PluralRuleSet : PluralFormSelector
    {
        protected readonly PluralRule [ ] rules;

        protected PluralRuleSet ( params PluralRule [ ] ruleSet ) : base ( GetPluralForms ( ruleSet ) )
        {
            rules = ruleSet ?? Array.Empty < PluralRule > ( );
        }

        public override PluralForm SelectPluralForm ( decimal number, PluralForm availablePluralForms )
        {
            foreach ( var rule in rules )
                if ( rule.AppliesTo ( number ) )
                    return rule.PluralForm & availablePluralForms;

            return PluralRule.Default.PluralForm;
        }

        private static PluralForm GetPluralForms ( PluralRule [ ] ruleSet )
        {
            var pluralForms = default ( PluralForm );

            if ( ruleSet != null )
                for ( var index = 0; index < ruleSet.Length; index++ )
                    pluralForms |= ruleSet [ index ].PluralForm;

            return pluralForms;
        }
    }
}
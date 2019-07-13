using System;

namespace Localizer.Plural
{
    public class OrdinalRuleSet : PluralFormSelector
    {
        public static OrdinalRuleSet Default { get; } = new OrdinalRuleSet ( Array.Empty < PluralRule > ( ) );

        private readonly PluralRule [ ] rules;

        public OrdinalRuleSet ( params PluralRule [ ] ruleSet )
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
    }
}
namespace Localizer.Pluralization
{
    public class OrdinalRuleSet : PluralRuleSet
    {
        public static OrdinalRuleSet Default { get; } = new OrdinalRuleSet ( );

        public OrdinalRuleSet ( params PluralRule [ ] ruleSet ) : base ( ruleSet ) { }
    }
}
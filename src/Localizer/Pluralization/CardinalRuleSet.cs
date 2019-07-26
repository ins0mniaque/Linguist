namespace Localizer.Pluralization
{
    public class CardinalRuleSet : PluralRuleSet
    {
        public static PluralRule ExplicitZeroRule { get; } = new ExplicitZeroRule ( );
        public static PluralRule ExplicitOneRule  { get; } = new ExplicitOneRule  ( );

        private readonly bool hasZeroForm;
        private readonly bool hasOneForm;

        public CardinalRuleSet ( params PluralRule [ ] ruleSet ) : base ( ruleSet )
        {
            hasZeroForm = PluralForms.HasBitMask ( PluralForm.Zero );
            hasOneForm  = PluralForms.HasBitMask ( PluralForm.One  );
        }

        public override PluralForm SelectPluralForm ( decimal number, PluralForm availablePluralForms )
        {
            if ( availablePluralForms.HasBitMask ( PluralForm.ExplicitZero ) )
            {
                if ( ExplicitZeroRule.AppliesTo ( number ) )
                    return ExplicitZeroRule.PluralForm;
            }
            else if ( ! hasZeroForm && availablePluralForms.HasBitMask ( PluralForm.Zero ) )
            {
                if ( ExplicitZeroRule.AppliesTo ( number ) )
                    return PluralForm.Zero;
            }

            if ( availablePluralForms.HasBitMask ( PluralForm.ExplicitOne ) )
            {
                if ( ExplicitOneRule.AppliesTo ( number ) )
                    return ExplicitOneRule.PluralForm;
            }
            else if ( ! hasOneForm && availablePluralForms.HasBitMask ( PluralForm.One ) )
            {
                if ( ExplicitOneRule.AppliesTo ( number ) )
                    return PluralForm.One;
            }

            return base.SelectPluralForm ( number, availablePluralForms );
        }
    }
}
namespace Localizer.Plural
{
    public abstract class PluralFormSelector
    {
        protected PluralFormSelector ( PluralForm pluralForms ) { PluralForms = pluralForms; }

        public PluralForm PluralForms { get; }

        public PluralForm SelectPluralForm ( decimal number )
        {
            return SelectPluralForm ( number, PluralForms );
        }

        public abstract PluralForm SelectPluralForm ( decimal number, PluralForm availablePluralForms );
    }
}
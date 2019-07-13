namespace Localizer.Plural
{
    public abstract class PluralFormSelector
    {
        public abstract PluralForm SelectPluralForm ( decimal number, PluralForm availablePluralForms );
    }
}
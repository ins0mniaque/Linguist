namespace Localizer.Pluralization
{
    public abstract class PluralFormRangeSelector
    {
        public abstract PluralForm SelectPluralForm ( PluralForm start, PluralForm end );
    }

    public class DefaultPluralFormRangeSelector : PluralFormRangeSelector
    {
        public override PluralForm SelectPluralForm ( PluralForm start, PluralForm end ) => end;
    }
}
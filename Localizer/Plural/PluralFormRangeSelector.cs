namespace Localizer.Plural
{
    public abstract class PluralFormRangeSelector
    {
        public static PluralFormRangeSelector Default { get; } = new DefaultPluralFormRangeSelector ( );

        public abstract PluralForm SelectPluralForm ( PluralForm start, PluralForm end );
    }

    public sealed class DefaultPluralFormRangeSelector : PluralFormRangeSelector
    {
        public sealed override PluralForm SelectPluralForm ( PluralForm start, PluralForm end ) => end;
    }
}
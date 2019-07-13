namespace Localizer.Plural
{
    public abstract class PluralRangeFormSelector
    {
        public static PluralRangeFormSelector Default { get; } = new DefaultPluralRangeFormSelector ( );

        public abstract PluralForm SelectPluralForm ( PluralForm start, PluralForm end );
    }

    public sealed class DefaultPluralRangeFormSelector : PluralRangeFormSelector
    {
        public sealed override PluralForm SelectPluralForm ( PluralForm start, PluralForm end ) => end;
    }
}
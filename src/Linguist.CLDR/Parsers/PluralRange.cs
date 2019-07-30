namespace Linguist.CLDR
{
    public class PluralRange
    {
        public PluralRange ( PluralForm start, PluralForm end, PluralForm result )
        {
            Start  = start;
            End    = end;
            Result = result;
        }

        public PluralForm Start  { get; }
        public PluralForm End    { get; }
        public PluralForm Result { get; }

        public override string ToString ( ) => $"[{ Start }, { End }] = { Result }";
    }
}
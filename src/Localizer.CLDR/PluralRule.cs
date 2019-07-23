namespace Localizer.CLDR
{
    public static class PluralRule
    {
        public static string Parse ( string rule )
        {
            return rule.Split ( '@' ) [ 0 ].Trim ( );
        }
    }
}
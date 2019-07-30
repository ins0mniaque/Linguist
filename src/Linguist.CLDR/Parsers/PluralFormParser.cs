using System;

namespace Linguist.CLDR
{
    public static class PluralFormParser
    {
        public static PluralForm Parse ( string count )
        {
            switch ( count )
            {
                case "zero"  : return PluralForm.Zero;
                case "one"   : return PluralForm.One;
                case "two"   : return PluralForm.Two;
                case "few"   : return PluralForm.Few;
                case "many"  : return PluralForm.Many;
                case "other" : return PluralForm.Other;
                case "0"     : return PluralForm.ExplicitZero;
                case "1"     : return PluralForm.ExplicitOne;
                default      : throw new FormatException ( "Invalid plural form" );
            }
        }
    }
}
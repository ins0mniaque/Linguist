using System;

namespace Localizer.Plural
{
    using static System.Decimal;

    /// <summary>
    /// Plural Operand Meanings
    /// 
    /// Symbol    Value
    /// n         absolute value of the source number (integer and decimals).
    /// i         integer digits of n.
    /// v         number of visible fraction digits in n, with trailing zeros.
    /// w         number of visible fraction digits in n, without trailing zeros.
    /// f         visible fractional digits in n, with trailing zeros.
    /// t         visible fractional digits in n, without trailing zeros.
    /// </summary>
    internal static class PluralOperand
    {
        public static decimal n ( decimal number ) => Math.Abs ( number );
        public static decimal i ( decimal number ) => Truncate ( Math.Abs ( number ) );
        public static decimal v ( decimal number ) => Scale ( number );
        public static decimal w ( decimal number ) => Scale ( Normalize ( number ) );
        public static decimal f ( decimal number ) => Math.Abs ( number ) % One;
        public static decimal t ( decimal number ) => Normalize ( Math.Abs ( number ) % One );

        private static decimal Scale     ( decimal number ) => ( GetBits ( number ) [ 3 ] >> 16 ) & 0x7F;
        private static decimal Normalize ( decimal number ) => number / 1.000000000000000000000000000000000m;
    }
}
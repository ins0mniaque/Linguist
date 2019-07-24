using System;
using System.Runtime.CompilerServices;

namespace Localizer.CLDR
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
        [ MethodImpl ( MethodImplOptions.AggressiveInlining ) ]
        public static decimal n ( this decimal number ) => Math.Abs ( number );

        [ MethodImpl ( MethodImplOptions.AggressiveInlining ) ]
        public static decimal i ( this decimal number ) => Truncate ( Math.Abs ( number ) );

        [ MethodImpl ( MethodImplOptions.AggressiveInlining ) ]
        public static decimal v ( this decimal number ) => Scale ( number );

        [ MethodImpl ( MethodImplOptions.AggressiveInlining ) ]
        public static decimal w ( this decimal number ) => Scale ( Normalize ( number ) );

        [ MethodImpl ( MethodImplOptions.AggressiveInlining ) ]
        public static decimal f ( this decimal number ) => Math.Abs ( number ) % One;

        [ MethodImpl ( MethodImplOptions.AggressiveInlining ) ]
        public static decimal t ( this decimal number ) => Normalize ( Math.Abs ( number ) % One );

        [ MethodImpl ( MethodImplOptions.AggressiveInlining ) ]
        private static decimal Scale ( decimal number ) => ( GetBits ( number ) [ 3 ] >> 16 ) & 0x7F;

        [ MethodImpl ( MethodImplOptions.AggressiveInlining ) ]
        private static decimal Normalize ( decimal number ) => number / 1.000000000000000000000000000000000m;
    }

    internal static class ExplicitRule
    {
        [ MethodImpl ( MethodImplOptions.AggressiveInlining ) ]
        public static bool Zero ( decimal i, decimal v ) => i == 0m && v == 0m;

        [ MethodImpl ( MethodImplOptions.AggressiveInlining ) ]
        public static bool One  ( decimal i, decimal v ) => i == 1m && v == 0m;
    }

    internal static class PluralRuleOperator
    {
        [ MethodImpl ( MethodImplOptions.AggressiveInlining ) ]
        public static bool equals ( this decimal number, decimal arg0 )
        {
            return number == arg0;
        }

        [ MethodImpl ( MethodImplOptions.AggressiveInlining ) ]
        public static bool equals ( this decimal number, decimal arg0, decimal arg1 )
        {
            return number == arg0 ||
                   number == arg1;
        }

        [ MethodImpl ( MethodImplOptions.AggressiveInlining ) ]
        public static bool equals ( this decimal number, decimal arg0, decimal arg1, decimal arg2 )
        {
            return number == arg0 ||
                   number == arg1 ||
                   number == arg2;
        }

        [ MethodImpl ( MethodImplOptions.AggressiveInlining ) ]
        public static bool equals ( this decimal number, decimal arg0, decimal arg1, decimal arg2, decimal arg3 )
        {
            return number == arg0 ||
                   number == arg1 ||
                   number == arg2 ||
                   number == arg3;
        }

        [ MethodImpl ( MethodImplOptions.AggressiveInlining ) ]
        public static bool equals ( this decimal number, decimal arg0, decimal arg1, decimal arg2, decimal arg3, decimal arg4 )
        {
            return number == arg0 ||
                   number == arg1 ||
                   number == arg2 ||
                   number == arg3 ||
                   number == arg4;
        }

        [ MethodImpl ( MethodImplOptions.AggressiveInlining ) ]
        public static bool equals ( this decimal number, decimal arg0, decimal arg1, decimal arg2, decimal arg3, decimal arg4, decimal arg5 )
        {
            return number == arg0 ||
                   number == arg1 ||
                   number == arg2 ||
                   number == arg3 ||
                   number == arg4 ||
                   number == arg5;
        }

        [ MethodImpl ( MethodImplOptions.AggressiveInlining ) ]
        public static bool between ( this decimal number, decimal start0, decimal end0 )
        {
            return number >= start0 && number <= end0;
        }

        [ MethodImpl ( MethodImplOptions.AggressiveInlining ) ]
        public static bool between ( this decimal number, decimal start0, decimal end0, decimal start1, decimal end1 )
        {
            return number >= start0 && number <= end0 ||
                   number >= start1 && number <= end1;
        }

        [ MethodImpl ( MethodImplOptions.AggressiveInlining ) ]
        public static bool between ( this decimal number, decimal start0, decimal end0, decimal start1, decimal end1, decimal start2, decimal end2 )
        {
            return number >= start0 && number <= end0 ||
                   number >= start1 && number <= end1 ||
                   number >= start2 && number <= end2;
        }
    }
}
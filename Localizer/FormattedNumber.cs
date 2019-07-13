using System;
using System.Globalization;
using System.Text.RegularExpressions;

namespace Localizer
{
    public static class FormattedNumber
    {
        public static decimal? Parse ( IFormatProvider provider, string format, int argumentIndex, object argument )
        {
            foreach ( var argumentHole in FormatString.Parse ( format ).ArgumentHoles )
                if ( argumentHole.Index == argumentIndex )
                    return Parse ( provider, argumentHole, argument );

            throw new ArgumentOutOfRangeException ( nameof ( argumentIndex ), argumentIndex, "Argument index not found in format." );
        }

        public static decimal? Parse ( IFormatProvider provider, FormatString.ArgumentHole argumentHole, object value )
        {
            argumentHole = new FormatString.ArgumentHole ( 0, argumentHole.Alignment, argumentHole.Format, 0, 0 );

            var formatted = string.Format ( provider, argumentHole.ToFormatString ( ), value );
            var number    = ExtractFormattedNumber ( formatted, argumentHole.Format, out var isHexNumber );

            if ( isHexNumber )
            {
                if ( long.TryParse ( number, NumberStyles.HexNumber, provider, out var hexNumber ) )
                    return hexNumber;
            }
            else if ( decimal.TryParse ( number, NumberStyles.Any, provider, out var decimalNumber ) )
                return decimalNumber;

            return null;
        }

        private static readonly Regex numberExtractor    = new Regex ( @"[-+]?(?<![0-9][.,])[.,]?[0-9]+(?:[.,\s][0-9]+)*[.,]?[0-9]?(?:[eE][-+]?[0-9]+)?(?!\.[0-9])", RegexOptions.Compiled );
        private static readonly Regex hexNumberExtractor = new Regex ( @"^([a-fA-F0-9]{1,2}\s?)+$", RegexOptions.Compiled );

        private static string ExtractFormattedNumber ( string text, string format, out bool isHexNumber )
        {
            isHexNumber = IsHexFormat ( format );

            var extractor = isHexNumber ? hexNumberExtractor : numberExtractor;
            var matches   = extractor.Matches ( text );

            return matches.Count > 0 ? matches [ 0 ].Value : text;
        }

        private static bool IsHexFormat ( string format )
        {
            if ( string.IsNullOrEmpty ( format ) )
                return false;

            if ( format [ 0 ] != 'X' && format [ 0 ] != 'x' )
                return false;

            if ( format.Length == 1 )
                return true;

            return int.TryParse ( format.Substring ( 1, format.Length - 1 ),
                                  NumberStyles.None,
                                  CultureInfo.InvariantCulture,
                                  out var _ );
        }
    }
}
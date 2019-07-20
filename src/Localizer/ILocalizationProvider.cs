using System;
using System.Globalization;

namespace Localizer
{
    public interface ILocalizationProvider
    {
        object GetObject ( CultureInfo culture, string name );
        string GetString ( CultureInfo culture, string name );
        string Format    ( CultureInfo culture, IFormatProvider provider, string name, params object [ ] args );
    }
}
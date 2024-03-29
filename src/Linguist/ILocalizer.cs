﻿using System;
using System.Globalization;

namespace Linguist
{
    public interface ILocalizer
    {
        object GetObject ( CultureInfo culture, string name );
        string GetString ( CultureInfo culture, string name );
        string Format    ( CultureInfo culture, IFormatProvider provider, string name, params object [ ] args );
    }
}
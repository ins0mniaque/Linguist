using System;
using System.ComponentModel;
using System.Globalization;

namespace Linguist.WPF
{
    public class LocalizerTypeConverter : TypeConverter
    {
        public override bool CanConvertFrom ( ITypeDescriptorContext context, Type sourceType )
        {
            return sourceType == typeof ( string );
        }

        public override object ConvertFrom ( ITypeDescriptorContext context, CultureInfo culture, object value )
        {
            if ( value is string path )
                return Localizer.Load ( path );

            return base.ConvertFrom ( context, culture, value );
        }
    }
}
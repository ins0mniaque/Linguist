using System;
using System.ComponentModel;
using System.Globalization;
using System.Windows.Data;

namespace Localizer.WPF
{
    public class BindingBaseTypeConverter : TypeConverter
    {
        public override bool CanConvertFrom ( ITypeDescriptorContext context, Type sourceType )
        {
            return sourceType == typeof ( string ) || sourceType == typeof ( BindingBase );
        }
 
        public override object ConvertFrom ( ITypeDescriptorContext context, CultureInfo culture, object value )
        {
            if ( value is BindingBase binding )
                return binding;
            if ( value is string name )
                return new Binding ( name );

            return base.ConvertFrom ( context, culture, value );
        }
    }
}
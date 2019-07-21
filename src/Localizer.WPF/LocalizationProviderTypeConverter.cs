using System;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Resources;

namespace Localizer.WPF
{
    public class LocalizationProviderTypeConverter : TypeConverter
    {
        public override bool CanConvertFrom ( ITypeDescriptorContext context, Type sourceType )
        {
            return sourceType == typeof ( string );
        }
 
        public override object ConvertFrom ( ITypeDescriptorContext context, CultureInfo culture, object value )
        {
            if ( value is string name )
            {
                var assembly = Assembly.Load ( name.Split ( '.' ).First ( ) );

                return new ResourceManagerLocalizationProvider ( new ResourceManager ( name, assembly ) );
            }

            return base.ConvertFrom ( context, culture, value );
        }
    }
}
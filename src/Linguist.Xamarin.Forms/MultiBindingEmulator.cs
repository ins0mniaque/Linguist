using System;
using System.Collections.Generic;
using System.Globalization;

using Xamarin.Forms;

namespace Linguist.Xamarin.Forms
{
    public static class MultiBindingEmulator
    {
        public static void EmulateMultiBinding ( this Binding binding, IList < BindingBase > bindings )
        {
            if ( bindings.Count > 1 )
                throw new NotSupportedException ( "Multiple bindings are not supported yet." );

            if ( bindings.Count == 1 )
            {
                if ( bindings [ 0 ] is Binding single )
                {
                    binding.Path                  = single.Path;
                    binding.Mode                  = single.Mode;
                    binding.Source                = single.Source;
                    binding.UpdateSourceEventName = single.UpdateSourceEventName;
                    binding.FallbackValue         = single.FallbackValue;
                    binding.TargetNullValue       = single.TargetNullValue;

                    if ( single.Converter != null || single.StringFormat != null )
                        binding.Converter = new SingleBinding ( binding.Converter,
                                                                single.Converter,
                                                                single.ConverterParameter,
                                                                single.StringFormat );
                }
                else
                    throw new NotSupportedException ( $"{ nameof ( MultiBindingEmulator ) } does not support bindings of type { bindings [ 0 ]?.GetType ( ).Name ?? "null" }." );
            }
            else
                binding.Source = NoBindings;
        }

        private static readonly object [ ] NoBindings = new object [ 0 ];

        private class SingleBinding : IValueConverter
        {
            private readonly IValueConverter parentConverter;
            private readonly IValueConverter converter;
            private readonly object          parameter;
            private readonly string          format;

            public SingleBinding ( IValueConverter parentConverter, IValueConverter converter, object parameter, string format )
            {
                this.parentConverter = parentConverter;
                this.converter       = converter;
                this.parameter       = parameter;
                this.format          = format;
            }

            public object Convert ( object value, Type targetType, object parentParameter, CultureInfo culture )
            {
                if ( converter != null )
                    value = converter.Convert ( value, targetType, parameter, culture );

                if ( format != null )
                    value = string.Format ( culture, format, value );

                if ( parentConverter != null )
                    value = parentConverter.Convert ( new [ ] { value }, targetType, parentParameter, culture );

                return value;
            }

            public object ConvertBack ( object value, Type targetType, object parentParameter, CultureInfo culture )
            {
                if ( converter != null )
                    return converter.ConvertBack ( value, targetType, parameter, culture );

                return null;
            }
        }
    }
}
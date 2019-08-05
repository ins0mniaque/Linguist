using System;
using System.Collections.Generic;
using System.Globalization;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Linguist.Xamarin.Forms
{
    public static class MultiBindingEmulator
    {
        public static void EmulateMultiBinding ( this IServiceProvider serviceProvider, Binding multiBinding, IList < BindingBase > bindings )
        {
            if      ( bindings.Count >  1 ) MultipleBindings.Emulate ( serviceProvider, multiBinding, bindings );
            else if ( bindings.Count == 1 ) SingleBinding   .Emulate ( multiBinding, bindings [ 0 ] );
            else                            NoBindings      .Emulate ( multiBinding );
        }

        private class NoBindings
        {
            private static readonly object [ ] Source = new object [ 0 ];

            public static void Emulate ( Binding multiBinding )
            {
                multiBinding.Source = Source;
            }
        }

        private class SingleBinding : IValueConverter
        {
            private readonly IValueConverter multiValueConverter;
            private readonly IValueConverter converter;
            private readonly object          parameter;
            private readonly string          format;

            public static void Emulate ( Binding multiBinding, BindingBase binding )
            {
                if ( ! ( binding is Binding single ) )
                    throw new NotSupportedException ( $"{ nameof ( MultiBindingEmulator ) } does not support bindings of type { binding?.GetType ( ).Name ?? "null" }." );

                multiBinding.Converter = new SingleBinding ( multiBinding, single );

                multiBinding.Path                  = single.Path;
                multiBinding.Mode                  = single.Mode;
                multiBinding.Source                = single.Source;
                multiBinding.UpdateSourceEventName = single.UpdateSourceEventName;
                multiBinding.FallbackValue         = single.FallbackValue;
                multiBinding.TargetNullValue       = single.TargetNullValue;
            }

            private SingleBinding ( Binding multiBinding, Binding binding )
            {
                multiValueConverter = multiBinding.Converter;
                converter           = binding.Converter;
                parameter           = binding.ConverterParameter;
                format              = binding.StringFormat;
            }

            public object Convert ( object value, Type targetType, object multiValueParameter, CultureInfo culture )
            {
                if ( converter != null )
                    value = converter.Convert ( value, targetType, parameter, culture );

                if ( format != null )
                    value = string.Format ( culture, format, value );

                if ( multiValueConverter != null )
                    value = multiValueConverter.Convert ( new [ ] { value }, targetType, multiValueParameter, culture );

                return value;
            }

            public object ConvertBack ( object value, Type targetType, object multiValueParameter, CultureInfo culture )
            {
                if ( multiValueConverter != null )
                {
                    var values = multiValueConverter.ConvertBack ( value, targetType, multiValueParameter, culture ) as object [ ];
                    if ( values != null && values.Length == 1 )
                        value = values [ 0 ];
                }

                if ( converter != null )
                    return converter.ConvertBack ( value, targetType, parameter, culture );

                return value;
            }
        }

        private class MultipleBindings : IValueConverter
        {
            private readonly IValueConverter multiValueConverter;

            public static void Emulate ( IServiceProvider serviceProvider, Binding multiBinding, IList < BindingBase > bindings )
            {
                var pvt = (IProvideValueTarget) serviceProvider.GetService ( typeof ( IProvideValueTarget ) );

                multiBinding.Converter = new MultipleBindings ( pvt.TargetObject, multiBinding, bindings );
            }

            private MultipleBindings ( object target, Binding multiBinding, IList < BindingBase > bindings )
            {
                var proxies   = new BindingProxy [ bindings.Count ];
                var source    = new MultiBindingProxy ( proxies );
                var reference = new WeakReference < MultiBindingProxy > ( source );

                for ( var index = 0; index < proxies.Length; index++ )
                {
                    var proxy   = new BindingProxy ( reference );
                    var context = new Binding ( nameof ( BindableObject.BindingContext ),
                                                BindingMode.OneWay,
                                                source: target );

                    proxy.SetBinding ( BindableObject.BindingContextProperty, context );
                    proxy.SetBinding ( BindingProxy.ValueProperty, bindings [ index ] );

                    proxies [ index ] = proxy;
                }

                multiValueConverter = multiBinding.Converter;

                multiBinding.Path   = nameof ( MultiBindingProxy.Value );
                multiBinding.Source = source;
            }

            public object Convert ( object value, Type targetType, object parameter, CultureInfo culture )
            {
                if ( multiValueConverter != null && value != null )
                    value = multiValueConverter.Convert ( value, targetType, parameter, culture );

                return value;
            }

            public object ConvertBack ( object value, Type targetType, object parameter, CultureInfo culture )
            {
                if ( multiValueConverter != null )
                    value = multiValueConverter.ConvertBack ( value, targetType, parameter, culture );

                return value;
            }
        }

        private class MultiBindingProxy : BindableObject
        {
            private readonly BindingProxy [ ] bindings;

            public MultiBindingProxy ( BindingProxy [ ] bindings )
            {
                this.bindings = bindings;
            }

            public object [ ] Value
            {
                get => (object [ ]) GetValue ( ValueProperty );
                set => SetValue ( ValueProperty, value );
            }

            public static readonly BindableProperty ValueProperty =
                BindableProperty.Create ( nameof ( Value ),
                                          typeof ( object [ ] ),
                                          typeof ( MultiBindingProxy ),
                                          null );

            internal void UpdateTarget ( )
            {
                var oldValue = Value;
                var newValue = new object [ bindings.Length ];

                for ( var index = 0; index < newValue.Length; index++ )
                    newValue [ index ] = bindings [ index ].Value;

                if ( oldValue == null || HasChanged ( oldValue, newValue ) )
                    Value = newValue;
            }

            private static bool HasChanged ( object [ ] oldValue, object [ ] newValue )
            {
                for ( var index = 0; index < newValue.Length; index++ )
                    if ( oldValue [ index ] != newValue [ index ] )
                        return true;

                return false;
            }
        }

        private class BindingProxy : BindableObject
        {
            private readonly WeakReference < MultiBindingProxy > weakMultiBinding;

            public BindingProxy ( WeakReference < MultiBindingProxy > multiBinding )
            {
                weakMultiBinding = multiBinding;
            }

            public object Value
            {
                get => GetValue ( ValueProperty );
                set => SetValue ( ValueProperty, value );
            }

            public static readonly BindableProperty ValueProperty =
                BindableProperty.Create ( nameof ( Value ),
                                          typeof ( object ),
                                          typeof ( BindingProxy ),
                                          null,
                                          propertyChanged: OnValueChanged );

            private static void OnValueChanged ( BindableObject sender, object oldValue, object newValue )
            {
                if ( ( (BindingProxy) sender ).weakMultiBinding.TryGetTarget ( out var multiBinding ) )
                    multiBinding.UpdateTarget ( );
            }
        }
    }
}
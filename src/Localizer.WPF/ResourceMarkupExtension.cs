using System;
using System.Globalization;
using System.Linq;
using System.Windows;
using System.Windows.Data;
using System.Windows.Markup;

namespace Localizer.WPF
{
    [ MarkupExtensionReturnType ( typeof ( MultiBindingExpression ) ) ]
    public abstract class ResourceMarkupExtension : MarkupExtension, IResourceMarkupExtension, IMultiValueConverter
    {
        protected readonly object [ ] arguments;

        protected ResourceMarkupExtension ( params object [ ] args )
        {
            arguments = args;
        }

        public abstract void SetupBinding ( MultiBinding binding, IServiceProvider serviceProvider );

        protected abstract object ProvideResource ( object [ ] values, Type targetType, object parameter, CultureInfo culture );

        public override object ProvideValue ( IServiceProvider serviceProvider )
        {
            var pvt    = (IProvideValueTarget) serviceProvider.GetService ( typeof ( IProvideValueTarget ) );
            var target = pvt.TargetObject as DependencyObject;
            if ( target == null )
                return this;

            var binding = new MultiBinding ( );

            SetupBinding ( binding, serviceProvider );

            return binding.ProvideValue ( serviceProvider );
        }

        protected static BindingBase ProvideInheritanceBinding ( DependencyProperty property )
        {
            return new Binding ( ) { Path           = new PropertyPath ( "(0)", property ),
                                     RelativeSource = RelativeSource.Self };
        }

        protected static BindingBase ProvideParameterBinding ( object parameter )
        {
            if ( parameter is Binding binding )
            {
                if ( ! string.IsNullOrEmpty ( binding.StringFormat ) )
                    binding.Converter = new Formatter ( binding.StringFormat, binding.Converter );

                return binding;
            }
            else if ( parameter is BindingBase       ) return (BindingBase) parameter;
            else if ( parameter is PropertyPath path ) return new Binding ( ) { Path = path };
            else                                       return new Binding ( parameter?.ToString ( ) );
        }

        object IMultiValueConverter.Convert ( object [ ] values, Type targetType, object parameter, CultureInfo culture )
        {
            return ProvideResource ( values, targetType, parameter, culture );
        }

        object [ ] IMultiValueConverter.ConvertBack ( object value, Type [ ] targetTypes, object parameter, CultureInfo culture )
        {
            return targetTypes.Select ( _ => Binding.DoNothing ).ToArray ( );
        }

        private class Formatter : IValueConverter
        {
            private readonly IValueConverter converter;
            private readonly string          format;

            public Formatter ( string format, IValueConverter converter )
            {
                this.format    = format;
                this.converter = converter;
            }

            public object Convert ( object value, Type targetType, object parameter, CultureInfo culture )
            {
                if ( converter != null )
                    value = converter.Convert ( value, targetType, parameter, culture );

                return string.Format ( culture, format, value );
            }

            public object ConvertBack ( object value, Type targetType, object parameter, CultureInfo culture )
            {
                if ( converter != null )
                    return converter.ConvertBack ( value, targetType, parameter, culture );

                return Binding.DoNothing;
            }
        }
    }
}
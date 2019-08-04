using System;
using System.Collections.Generic;
using System.Globalization;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Linguist.Xamarin.Forms
{
    [ ContentProperty ( nameof ( Argument ) ) ]
    public abstract class ResourceMarkupExtension : IMarkupExtension < BindingBase >, IValueConverter
    {
        protected IList < BindingBase > arguments;

        [ TypeConverter ( typeof ( BindingBaseTypeConverter ) ) ]
        public BindingBase Argument
        {
            set => AddArgument ( value );
        }

        private void AddArgument ( BindingBase argument )
        {
            if ( arguments == null )
                arguments = new List < BindingBase > ( );

            arguments.Add ( argument );
        }

        protected abstract void SetupBinding ( Binding binding, IServiceProvider serviceProvider );

        protected abstract object ProvideResource ( object [ ] values, Type targetType, object parameter, CultureInfo culture );

        public BindingBase ProvideValue ( IServiceProvider serviceProvider )
        {
            var binding = new Binding ( );

            SetupBinding ( binding, serviceProvider );

            return binding;
        }

        protected static BindingBase ProvideParameterBinding ( object parameter )
        {
            if ( parameter is BindingBase ) return (BindingBase) parameter;
            else                            return new Binding ( parameter?.ToString ( ) );
        }

        object IMarkupExtension.ProvideValue ( IServiceProvider serviceProvider )
        {
            return ProvideValue ( serviceProvider );
        }

        object IValueConverter.Convert ( object value, Type targetType, object parameter, CultureInfo culture )
        {
            return ProvideResource ( value as object [ ] ?? new [ ] { value }, targetType, parameter, culture );
        }

        object IValueConverter.ConvertBack ( object value, Type targetType, object parameter, CultureInfo culture )
        {
            return null;
        }
    }
}
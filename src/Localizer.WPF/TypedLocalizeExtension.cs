using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace Localizer.WPF
{
    public abstract class TypedLocalizeExtension < TKey > : ResourceMarkupExtension where TKey : struct
    {
        protected TypedLocalizeExtension ( params object [ ] parameters ) : base ( parameters ) { }

        public abstract TKey        Key     { get; set; }
        public abstract BindingBase KeyPath { get; set; }
        public abstract Type        Type    { get; set; }

        protected abstract ILocalizationProvider Provider  { get; }
        protected abstract string                KeyToName ( TKey key );

        public override void SetupBinding ( MultiBinding binding, IServiceProvider serviceProvider )
        {
            binding.Converter          = this;
            binding.ConverterParameter = Key;

            binding.Bindings.Add ( ProvideInheritanceBinding ( FrameworkElement.LanguageProperty ) );

            if ( KeyPath != null )
            {
                binding.Bindings.Add ( ProvideParameterBinding ( KeyPath ) );
                binding.ConverterParameter = null;
            }

            foreach ( var parameter in arguments )
                binding.Bindings.Add ( ProvideParameterBinding ( parameter ) );
        }

        protected override object ProvideResource ( object [ ] values, Type targetType, object parameter, CultureInfo culture )
        {
            if ( parameter == null )
            {
                var keyPath = values [ 1 ];
                if ( keyPath is TKey typedKey )
                    values [ 1 ] = KeyToName ( typedKey );
                else if ( keyPath != null && Enum.TryParse < TKey > ( keyPath.ToString ( ), out var key ) )
                    values [ 1 ] = KeyToName ( key );
            }
            else if ( parameter is TKey key )
                parameter = KeyToName ( key );

            return Localize.ProvideResource ( Provider, culture, parameter, values, Type ?? targetType );
        }
    }
}
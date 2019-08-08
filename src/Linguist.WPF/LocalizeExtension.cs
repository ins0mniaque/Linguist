using System;
using System.ComponentModel;
using System.Globalization;
using System.Windows;
using System.Windows.Data;
using System.Windows.Markup;

namespace Linguist.WPF
{
    public class LocalizeExtension : ResourceMarkupExtension
    {
        public LocalizeExtension ( )                                                                                                                                                                                                                                                                              : base ( null ) { }
        public LocalizeExtension ( object arg0 )                                                                                                                                                                                                                                                                  : base ( arg0 ) { }
        public LocalizeExtension ( object arg0, object arg1 )                                                                                                                                                                                                                                                     : base ( arg0, arg1 ) { }
        public LocalizeExtension ( object arg0, object arg1, object arg2 )                                                                                                                                                                                                                                        : base ( arg0, arg1, arg2 ) { }
        public LocalizeExtension ( object arg0, object arg1, object arg2, object arg3 )                                                                                                                                                                                                                           : base ( arg0, arg1, arg2, arg3 ) { }
        public LocalizeExtension ( object arg0, object arg1, object arg2, object arg3, object arg4 )                                                                                                                                                                                                              : base ( arg0, arg1, arg2, arg3, arg4 ) { }
        public LocalizeExtension ( object arg0, object arg1, object arg2, object arg3, object arg4, object arg5 )                                                                                                                                                                                                 : base ( arg0, arg1, arg2, arg3, arg4, arg5 ) { }
        public LocalizeExtension ( object arg0, object arg1, object arg2, object arg3, object arg4, object arg5, object arg6 )                                                                                                                                                                                    : base ( arg0, arg1, arg2, arg3, arg4, arg5, arg6 ) { }
        public LocalizeExtension ( object arg0, object arg1, object arg2, object arg3, object arg4, object arg5, object arg6, object arg7 )                                                                                                                                                                       : base ( arg0, arg1, arg2, arg3, arg4, arg5, arg6, arg7 ) { }
        public LocalizeExtension ( object arg0, object arg1, object arg2, object arg3, object arg4, object arg5, object arg6, object arg7, object arg8 )                                                                                                                                                          : base ( arg0, arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8 ) { }
        public LocalizeExtension ( object arg0, object arg1, object arg2, object arg3, object arg4, object arg5, object arg6, object arg7, object arg8, object arg9 )                                                                                                                                             : base ( arg0, arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9 ) { }
        public LocalizeExtension ( object arg0, object arg1, object arg2, object arg3, object arg4, object arg5, object arg6, object arg7, object arg8, object arg9, object arg10 )                                                                                                                               : base ( arg0, arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10 ) { }
        public LocalizeExtension ( object arg0, object arg1, object arg2, object arg3, object arg4, object arg5, object arg6, object arg7, object arg8, object arg9, object arg10, object arg11 )                                                                                                                 : base ( arg0, arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11 ) { }
        public LocalizeExtension ( object arg0, object arg1, object arg2, object arg3, object arg4, object arg5, object arg6, object arg7, object arg8, object arg9, object arg10, object arg11, object arg12 )                                                                                                   : base ( arg0, arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12 ) { }
        public LocalizeExtension ( object arg0, object arg1, object arg2, object arg3, object arg4, object arg5, object arg6, object arg7, object arg8, object arg9, object arg10, object arg11, object arg12, object arg13 )                                                                                     : base ( arg0, arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13 ) { }
        public LocalizeExtension ( object arg0, object arg1, object arg2, object arg3, object arg4, object arg5, object arg6, object arg7, object arg8, object arg9, object arg10, object arg11, object arg12, object arg13, object arg14 )                                                                       : base ( arg0, arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13, arg14 ) { }
        public LocalizeExtension ( object arg0, object arg1, object arg2, object arg3, object arg4, object arg5, object arg6, object arg7, object arg8, object arg9, object arg10, object arg11, object arg12, object arg13, object arg14, object arg15 )                                                         : base ( arg0, arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13, arg14, arg15 ) { }
        public LocalizeExtension ( object arg0, object arg1, object arg2, object arg3, object arg4, object arg5, object arg6, object arg7, object arg8, object arg9, object arg10, object arg11, object arg12, object arg13, object arg14, object arg15, object arg16 )                                           : base ( arg0, arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13, arg14, arg15, arg16 ) { }
        public LocalizeExtension ( object arg0, object arg1, object arg2, object arg3, object arg4, object arg5, object arg6, object arg7, object arg8, object arg9, object arg10, object arg11, object arg12, object arg13, object arg14, object arg15, object arg16, object arg17 )                             : base ( arg0, arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13, arg14, arg15, arg16, arg17 ) { }
        public LocalizeExtension ( object arg0, object arg1, object arg2, object arg3, object arg4, object arg5, object arg6, object arg7, object arg8, object arg9, object arg10, object arg11, object arg12, object arg13, object arg14, object arg15, object arg16, object arg17, object arg18 )               : base ( arg0, arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13, arg14, arg15, arg16, arg17, arg18 ) { }
        public LocalizeExtension ( object arg0, object arg1, object arg2, object arg3, object arg4, object arg5, object arg6, object arg7, object arg8, object arg9, object arg10, object arg11, object arg12, object arg13, object arg14, object arg15, object arg16, object arg17, object arg18, object arg19 ) : base ( arg0, arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13, arg14, arg15, arg16, arg17, arg18, arg19 ) { }

        protected LocalizeExtension ( params object [ ] parameters ) : base ( parameters ) { }

        [ TypeConverter ( typeof ( LocalizerTypeConverter ) ) ]
        public ILocalizer Localizer { get; set; }

        public string Key { get; set; }

        [ TypeConverter ( typeof ( BindingSyntax.TypeConverter ) ) ]
        public BindingBase KeyPath { get; set; }

        public Type Type { get; set; }

        public override void SetupBinding ( MultiBinding binding, IServiceProvider serviceProvider )
        {
            binding.Converter          = this;
            binding.ConverterParameter = Key;

            if ( Localizer == null )
                binding.Bindings.Add ( InheritanceBinding ( Localize.LocalizerProperty ) );

            binding.Bindings.Add ( InheritanceBinding ( FrameworkElement.LanguageProperty ) );

            if ( KeyPath != null )
            {
                binding.Bindings.Add ( KeyPath );
                binding.ConverterParameter = null;
            }
            else if ( Key == null )
            {
                if ( serviceProvider == null )
                    throw new XamlParseException ( "Missing resource key; auto-generated keys are not supported in styles. e.g. {Localize Key=Name}" );

                Auto.SetComponent ( serviceProvider );

                var pvt = (IProvideValueTarget) serviceProvider.GetService ( typeof ( IProvideValueTarget ) );

                binding.ConverterParameter = new ProvideValueTarget ( pvt.TargetObject, pvt.TargetProperty );
            }

            if ( arguments != null )
                foreach ( var argument in arguments )
                    binding.Bindings.Add ( argument );
        }

        protected override object ProvideResource ( object [ ] values, Type targetType, object parameter, CultureInfo culture )
        {
            return Localize.ProvideResource ( Localizer, culture, parameter, values, Type ?? targetType );
        }

        private sealed class ProvideValueTarget : IProvideValueTarget
        {
            public ProvideValueTarget ( object target, object property )
            {
                TargetObject   = target;
                TargetProperty = property;
            }

            public object TargetObject   { get; }
            public object TargetProperty { get; }
        }
    }
}
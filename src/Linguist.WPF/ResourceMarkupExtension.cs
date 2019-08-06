using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Windows;
using System.Windows.Data;
using System.Windows.Markup;

namespace Linguist.WPF
{
    using EB   = EditorBrowsableAttribute;
    using EBS  = EditorBrowsableState;
    using TC   = TypeConverterAttribute;
    using BSTC = BindingSyntax.TypeConverter;

    [ MarkupExtensionReturnType ( typeof ( MultiBindingExpression ) ) ]
    public abstract class ResourceMarkupExtension : MarkupExtension, IResourceMarkupExtension, IMultiValueConverter
    {
        protected IList < BindingBase > arguments;

        protected ResourceMarkupExtension ( params object [ ] args )
        {
            if ( args != null )
            {
                arguments = new List < BindingBase > ( args.Length );
                foreach ( var argument in args )
                    arguments.Add ( BindingSyntax.From ( argument ) );
            }
        }

        [                   TC ( typeof ( BSTC ) ) ] public BindingBase Arg0  { set => SetArgument ( value,  0 ); }
        [                   TC ( typeof ( BSTC ) ) ] public BindingBase Arg1  { set => SetArgument ( value,  1 ); }
        [                   TC ( typeof ( BSTC ) ) ] public BindingBase Arg2  { set => SetArgument ( value,  2 ); }
        [                   TC ( typeof ( BSTC ) ) ] public BindingBase Arg3  { set => SetArgument ( value,  3 ); }
        [ EB ( EBS.Never ), TC ( typeof ( BSTC ) ) ] public BindingBase Arg4  { set => SetArgument ( value,  4 ); }
        [ EB ( EBS.Never ), TC ( typeof ( BSTC ) ) ] public BindingBase Arg5  { set => SetArgument ( value,  5 ); }
        [ EB ( EBS.Never ), TC ( typeof ( BSTC ) ) ] public BindingBase Arg6  { set => SetArgument ( value,  6 ); }
        [ EB ( EBS.Never ), TC ( typeof ( BSTC ) ) ] public BindingBase Arg7  { set => SetArgument ( value,  7 ); }
        [ EB ( EBS.Never ), TC ( typeof ( BSTC ) ) ] public BindingBase Arg8  { set => SetArgument ( value,  8 ); }
        [ EB ( EBS.Never ), TC ( typeof ( BSTC ) ) ] public BindingBase Arg9  { set => SetArgument ( value,  9 ); }
        [ EB ( EBS.Never ), TC ( typeof ( BSTC ) ) ] public BindingBase Arg10 { set => SetArgument ( value, 10 ); }
        [ EB ( EBS.Never ), TC ( typeof ( BSTC ) ) ] public BindingBase Arg11 { set => SetArgument ( value, 11 ); }
        [ EB ( EBS.Never ), TC ( typeof ( BSTC ) ) ] public BindingBase Arg12 { set => SetArgument ( value, 12 ); }
        [ EB ( EBS.Never ), TC ( typeof ( BSTC ) ) ] public BindingBase Arg13 { set => SetArgument ( value, 13 ); }
        [ EB ( EBS.Never ), TC ( typeof ( BSTC ) ) ] public BindingBase Arg14 { set => SetArgument ( value, 14 ); }
        [ EB ( EBS.Never ), TC ( typeof ( BSTC ) ) ] public BindingBase Arg15 { set => SetArgument ( value, 15 ); }
        [ EB ( EBS.Never ), TC ( typeof ( BSTC ) ) ] public BindingBase Arg16 { set => SetArgument ( value, 16 ); }
        [ EB ( EBS.Never ), TC ( typeof ( BSTC ) ) ] public BindingBase Arg17 { set => SetArgument ( value, 17 ); }
        [ EB ( EBS.Never ), TC ( typeof ( BSTC ) ) ] public BindingBase Arg18 { set => SetArgument ( value, 18 ); }
        [ EB ( EBS.Never ), TC ( typeof ( BSTC ) ) ] public BindingBase Arg19 { set => SetArgument ( value, 19 ); }

        private void SetArgument ( BindingBase argument, int index )
        {
            if ( arguments == null )
                arguments = new List < BindingBase > ( index + 1 );

            while ( arguments.Count <= index )
                arguments.Add ( null );

            arguments [ index ] = argument;
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

            foreach ( var argument in binding.Bindings )
                Formatter.EnforceStringFormatUsage ( argument );

            return binding.ProvideValue ( serviceProvider );
        }

        protected static BindingBase InheritanceBinding ( DependencyProperty property )
        {
            return new Binding ( ) { Path           = new PropertyPath ( "(0)", property ),
                                     RelativeSource = RelativeSource.Self };
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

            public static void EnforceStringFormatUsage ( BindingBase bindingBase )
            {
                if ( bindingBase is Binding binding && ! string.IsNullOrEmpty ( binding.StringFormat ) )
                    binding.Converter = new Formatter ( binding.StringFormat, binding.Converter );
            }

            private Formatter ( string format, IValueConverter converter )
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
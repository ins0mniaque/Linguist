using System;
using System.Collections.Generic;
using System.Globalization;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Linguist.Xamarin.Forms
{
    using EB   = System.ComponentModel.EditorBrowsableAttribute;
    using EBS  = System.ComponentModel.EditorBrowsableState;
    using TC   = TypeConverterAttribute;
    using BBTC = BindingBaseTypeConverter;

    [ ContentProperty ( nameof ( Argument ) ) ]
    public abstract class ResourceMarkupExtension : IMarkupExtension < BindingBase >, IValueConverter
    {
        protected IList < BindingBase > arguments;

        [ EB ( EBS.Never ), TC ( typeof ( BBTC ) ) ] public BindingBase Argument { set => AddArgument ( value );     }
        [                   TC ( typeof ( BBTC ) ) ] public BindingBase Arg0     { set => SetArgument ( value,  0 ); }
        [                   TC ( typeof ( BBTC ) ) ] public BindingBase Arg1     { set => SetArgument ( value,  1 ); }
        [                   TC ( typeof ( BBTC ) ) ] public BindingBase Arg2     { set => SetArgument ( value,  2 ); }
        [                   TC ( typeof ( BBTC ) ) ] public BindingBase Arg3     { set => SetArgument ( value,  3 ); }
        [ EB ( EBS.Never ), TC ( typeof ( BBTC ) ) ] public BindingBase Arg4     { set => SetArgument ( value,  4 ); }
        [ EB ( EBS.Never ), TC ( typeof ( BBTC ) ) ] public BindingBase Arg5     { set => SetArgument ( value,  5 ); }
        [ EB ( EBS.Never ), TC ( typeof ( BBTC ) ) ] public BindingBase Arg6     { set => SetArgument ( value,  6 ); }
        [ EB ( EBS.Never ), TC ( typeof ( BBTC ) ) ] public BindingBase Arg7     { set => SetArgument ( value,  7 ); }
        [ EB ( EBS.Never ), TC ( typeof ( BBTC ) ) ] public BindingBase Arg8     { set => SetArgument ( value,  8 ); }
        [ EB ( EBS.Never ), TC ( typeof ( BBTC ) ) ] public BindingBase Arg9     { set => SetArgument ( value,  9 ); }
        [ EB ( EBS.Never ), TC ( typeof ( BBTC ) ) ] public BindingBase Arg10    { set => SetArgument ( value, 10 ); }
        [ EB ( EBS.Never ), TC ( typeof ( BBTC ) ) ] public BindingBase Arg11    { set => SetArgument ( value, 11 ); }
        [ EB ( EBS.Never ), TC ( typeof ( BBTC ) ) ] public BindingBase Arg12    { set => SetArgument ( value, 12 ); }
        [ EB ( EBS.Never ), TC ( typeof ( BBTC ) ) ] public BindingBase Arg13    { set => SetArgument ( value, 13 ); }
        [ EB ( EBS.Never ), TC ( typeof ( BBTC ) ) ] public BindingBase Arg14    { set => SetArgument ( value, 14 ); }
        [ EB ( EBS.Never ), TC ( typeof ( BBTC ) ) ] public BindingBase Arg15    { set => SetArgument ( value, 15 ); }
        [ EB ( EBS.Never ), TC ( typeof ( BBTC ) ) ] public BindingBase Arg16    { set => SetArgument ( value, 16 ); }
        [ EB ( EBS.Never ), TC ( typeof ( BBTC ) ) ] public BindingBase Arg17    { set => SetArgument ( value, 17 ); }
        [ EB ( EBS.Never ), TC ( typeof ( BBTC ) ) ] public BindingBase Arg18    { set => SetArgument ( value, 18 ); }
        [ EB ( EBS.Never ), TC ( typeof ( BBTC ) ) ] public BindingBase Arg19    { set => SetArgument ( value, 19 ); }

        private void AddArgument ( BindingBase argument )
        {
            if ( arguments == null )
                arguments = new List < BindingBase > ( );

            arguments.Add ( argument );
        }

        private void SetArgument ( BindingBase argument, int index )
        {
            if ( arguments == null )
                arguments = new List < BindingBase > ( index + 1 );

            while ( arguments.Count <= index )
                arguments.Add ( null );

            arguments [ index ] = argument;
        }

        protected abstract void SetupBinding ( Binding binding, IServiceProvider serviceProvider );

        protected abstract object ProvideResource ( object [ ] values, Type targetType, object parameter, CultureInfo culture );

        public BindingBase ProvideValue ( IServiceProvider serviceProvider )
        {
            var binding = new Binding ( );

            if ( arguments != null )
            {
                for ( var index = 0; index < arguments.Count; index++ )
                {
                    var argument = arguments [ index ];
                    if ( argument == null )
                        throw XamlParseException ( serviceProvider, $"Localize is missing argument index { index }" );

                    ResolveBindingSource ( argument, serviceProvider );
                }
            }

            SetupBinding ( binding, serviceProvider );

            return binding;
        }

        protected static void ResolveBindingSource ( BindingBase bindingBase, IServiceProvider serviceProvider )
        {
            if ( bindingBase is Binding binding && binding.Source is IMarkupExtension markupExtension )
                binding.Source = markupExtension.ProvideValue ( serviceProvider );
        }

        protected static XamlParseException XamlParseException ( IServiceProvider serviceProvider, string error, Exception innerException = null )
        {
            var line = ( (IXmlLineInfoProvider) serviceProvider.GetService ( typeof ( IXmlLineInfoProvider ) ) )?.XmlLineInfo;

            return new XamlParseException ( error, line, innerException );
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
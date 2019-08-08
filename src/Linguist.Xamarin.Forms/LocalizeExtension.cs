using System;
using System.Collections.Generic;
using System.Globalization;

using Xamarin.Forms;
using Xamarin.Forms.Internals;

namespace Linguist.Xamarin.Forms
{
    [ Preserve ( AllMembers = true ) ]
    public partial class Localize : ResourceMarkupExtension
    {
        [ TypeConverter ( typeof ( LocalizerTypeConverter ) ) ]
        public ILocalizer Localizer { get; set; }

        public string Key { get; set; }

        [ TypeConverter ( typeof ( BindingSyntax.TypeConverter ) ) ]
        public BindingBase KeyPath { get; set; }

        public Type Type { get; set; }

        protected override void SetupBinding ( Binding binding, IServiceProvider serviceProvider )
        {
            var bindings = new List < BindingBase > ( );
            var resource = new Resource ( ) { Localizer = Localizer,
                                              Key       = Key?.ToString ( ) };

            binding.Converter          = this;
            binding.ConverterParameter = resource;

            if ( KeyPath != null )
            {
                ResolveBindingSource ( KeyPath, serviceProvider );

                bindings.Add ( KeyPath );
                resource.Key = null;
            }
            else if ( Key == null )
            {
                resource.Key = Auto.GenerateKey ( serviceProvider, out var localizer );

                if ( resource.Localizer == null )
                    resource.Localizer = localizer;
            }

            if ( resource.Localizer == null )
                resource.Localizer = Auto.GetLocalizer ( serviceProvider );

            if ( resource.Localizer == null )
                throw XamlParseException ( serviceProvider, "Missing Localize.Localizer on root element, or not set before first {Localize}." );

            if ( arguments != null )
                foreach ( var argument in arguments )
                    bindings.Add ( argument );

            serviceProvider.EmulateMultiBinding ( binding, bindings );
        }

        protected override object ProvideResource ( object [ ] values, Type targetType, object parameter, CultureInfo culture )
        {
            var resource = (Resource) parameter;

            return ProvideResource ( resource.Localizer, culture, resource.Key, values, Type ?? targetType );
        }

        private class Resource
        {
            public ILocalizer Localizer { get; set; }
            public string     Key       { get; set; }
        }
    }
}
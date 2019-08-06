using System;
using System.ComponentModel;
using System.Globalization;
using System.Windows.Data;

namespace Linguist.WPF
{
    public static class BindingSyntax
    {
        public static BindingBase From ( object value )
        {
            return value as BindingBase ?? Parse ( value?.ToString ( ) );
        }

        public static BindingBase Parse ( string path )
        {
            path = ParseSource ( path, out var source );

            var binding = new Binding ( path );

            if ( source is RelativeSource relativeSource )
                binding.RelativeSource = relativeSource;
            else if ( source is string elementName )
                binding.ElementName = elementName;

            return binding;
        }

        private static string ParseSource ( string path, out object source )
        {
            source = null;

            if ( path.Length < 2 )
                return path;

            if ( path [ 0 ] == '#' )
            {
                path   = ParseSourceToken ( path, out var name );
                source = name;
            }
            else if ( path [ 0 ] == '@' )
            {
                path = ParseSourceToken ( path, out var relativeSource );

                switch ( relativeSource )
                {
                    case "self"   : source = RelativeSource.Self;            break;
                    case "parent" : source = RelativeSource.TemplatedParent; break;
                    default       : throw new ArgumentException ( $"Unknown relative source @{ relativeSource }", nameof ( path ) );
                }
            }

            return path;
        }

        private static string ParseSourceToken ( string path, out string token )
        {
            var dot = path.IndexOf ( '.' );

            if ( dot >= 0 )
            {
                token = path.Substring ( 1, dot - 1 );
                return path.Substring ( dot + 1 );
            }

            token = path.Substring ( 1 );
            return ".";
        }

        public class TypeConverter : System.ComponentModel.TypeConverter
        {
            public override bool CanConvertFrom ( ITypeDescriptorContext context, Type sourceType )
            {
                return sourceType == typeof ( string ) || sourceType == typeof ( BindingBase );
            }
 
            public override object ConvertFrom ( ITypeDescriptorContext context, CultureInfo culture, object value )
            {
                if ( value is BindingBase binding ) return binding;
                if ( value is string      path    ) return Parse ( path );

                return base.ConvertFrom ( context, culture, value );
            }
        }
    }
}
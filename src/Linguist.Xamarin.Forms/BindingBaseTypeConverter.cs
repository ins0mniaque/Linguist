using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Linguist.Xamarin.Forms
{
    [ TypeConversion ( typeof ( BindingBase ) ) ]
    public class BindingBaseTypeConverter : TypeConverter
    {
        public override object ConvertFromInvariantString ( string path )
        {
            path = ParseSource ( path, out var source );

            return new Binding ( path ) { Source = source };
        }

        private static string ParseSource ( string path, out object source )
        {
            source = null;

            if ( path.Length < 2 )
                return path;

            if ( path [ 0 ] == '#' )
            {
                path = ParseSourceToken ( path, out var name );

                source = new ReferenceExtension ( ) { Name = name };
            }

            // TODO: Enable when RelativeBindingSource is released
            // else if ( path [ 0 ] == '@' )
            // {
            //     path = ParseSourceToken ( path, out var relativeSource );
            //
            //     switch ( relativeSource )
            //     {
            //         case "self"   : source = RelativeBindingSource.Self;            break;
            //         case "parent" : source = RelativeBindingSource.TemplatedParent; break;
            //         default       : throw new ArgumentException ( $"Unknown relative source @{ relativeSource }", nameof ( path ) );
            //     }
            // }

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
    }
}
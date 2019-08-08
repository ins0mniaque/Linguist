using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Linguist.Xamarin.Forms
{
    [ TypeConversion ( typeof ( ILocalizer ) ) ]
    public class LocalizerTypeConverter : TypeConverter
    {
        public override object ConvertFromInvariantString ( string path )
        {
            return Localizer.Load ( path );
        }
    }
}
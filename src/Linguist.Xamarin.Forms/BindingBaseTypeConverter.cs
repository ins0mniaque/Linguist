using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Linguist.Xamarin.Forms
{
    [ TypeConversion ( typeof ( BindingBase ) ) ]
    public class BindingBaseTypeConverter : TypeConverter
    {
        public override object ConvertFromInvariantString ( string path )
        {
            return new Binding ( path );
        }
    }
}
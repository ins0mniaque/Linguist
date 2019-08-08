using System.Globalization;

namespace Linguist
{
    public interface IPluralizer
    {
        string GetFormat ( CultureInfo culture, string name, params object [ ] args );
    }
}
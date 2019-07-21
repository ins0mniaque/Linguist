using System.Windows.Data;

namespace Localizer.WPF
{
    public class StyleResource : MultiBinding
    {
        public StyleResource ( IResourceMarkupExtension resourceMarkupExtension )
        {
            resourceMarkupExtension.SetupBinding ( this, null );
        }
    }
}
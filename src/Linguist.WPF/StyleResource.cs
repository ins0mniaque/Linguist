using System.Windows.Data;

namespace Linguist.WPF
{
    public class StyleResource : MultiBinding
    {
        public StyleResource ( IResourceMarkupExtension resourceMarkupExtension )
        {
            resourceMarkupExtension.SetupBinding ( this, null );
        }
    }
}
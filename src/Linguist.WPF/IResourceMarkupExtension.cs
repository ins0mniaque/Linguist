using System;
using System.Windows.Data;

namespace Linguist.WPF
{
    public interface IResourceMarkupExtension
    {
        void SetupBinding ( MultiBinding binding, IServiceProvider serviceProvider );
    }
}
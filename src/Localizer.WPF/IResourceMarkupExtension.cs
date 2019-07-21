using System;
using System.Windows.Data;

namespace Localizer.WPF
{
    public interface IResourceMarkupExtension
    {
        void SetupBinding ( MultiBinding binding, IServiceProvider serviceProvider );
    }
}
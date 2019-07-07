using System;
using System.Runtime.InteropServices;
using System.Threading;

using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.TextTemplating.VSHost;

namespace Localizer
{
    using Task = System.Threading.Tasks.Task;

    [ Guid ( PackageGuidString ) ]
    [ PackageRegistration ( UseManagedResourcesOnly = true, AllowsBackgroundLoading = true ) ]
    [ InstalledProductRegistration ( PackageName, PackageDetails, PackageVersion ) ]
    [ ProvideMenuResource  ( "Menus.ctmenu", 1 ) ]
    [ ProvideCodeGenerator ( typeof ( LocalizerCodeGenerator         ), LocalizerCodeGenerator        .Name, LocalizerCodeGenerator        .Description, true ) ]
    [ ProvideCodeGenerator ( typeof ( InternalLocalizerCodeGenerator ), InternalLocalizerCodeGenerator.Name, InternalLocalizerCodeGenerator.Description, true ) ]
    [ ProvideUIContextRule ( "DF9405E1-FFE4-4C7D-A491-6FECEA978857",
                             name       : "UI Context",
                             expression : "resx | resw",
                             termNames  : new [ ] { "resx", "resw" },
                             termValues : new [ ] { "HierSingleSelectionName:.resx$",
                                                    "HierSingleSelectionName:.resw$" } ) ]
    public sealed class LocalizerPackage : AsyncPackage
    {
        public const string PackageName       = "Localizer";
        public const string PackageDetails    = "";
        public const string PackageVersion    = "1.0";
        public const string PackageGuidString = "97313391-C7DF-45DB-817D-BE297DE8BD35";

        protected override async Task InitializeAsync ( CancellationToken cancellationToken, IProgress < ServiceProgressData > progress )
        {
            await base.InitializeAsync ( cancellationToken, progress );

            await JoinableTaskFactory.SwitchToMainThreadAsync ( cancellationToken );

            await ApplyCustomTool.InitializeAsync ( this );
        }
    }
}
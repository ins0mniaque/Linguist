using System;
using System.Runtime.InteropServices;
using System.Threading;

using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.TextTemplating.VSHost;

namespace Linguist.VisualStudio
{
    using Task = System.Threading.Tasks.Task;

    [ Guid ( GuidString ) ]
    [ PackageRegistration ( UseManagedResourcesOnly = true, AllowsBackgroundLoading = true ) ]
    [ InstalledProductRegistration ( "#110", "#112", Version, IconResourceID = 400 ) ]
    [ ProvideMenuResource  ( "Menus.ctmenu", 1 ) ]
    [ ProvideCodeGenerator ( typeof ( LinguistCodeGenerator         ), LinguistCodeGenerator        .Name, LinguistCodeGenerator        .Description, true ) ]
    [ ProvideCodeGenerator ( typeof ( InternalLinguistCodeGenerator ), InternalLinguistCodeGenerator.Name, InternalLinguistCodeGenerator.Description, true ) ]
    [ ProvideUIContextRule ( "DF9405E1-FFE4-4C7D-A491-6FECEA978857",
                             name       : "UI Context",
                             expression : "resx | resw",
                             termNames  : new [ ] { "resx", "resw" },
                             termValues : new [ ] { "HierSingleSelectionName:.resx$",
                                                    "HierSingleSelectionName:.resw$" } ) ]
    public sealed class LinguistPackage : AsyncPackage
    {
        public const string Version    = "0.9.1.2";
        public const bool   Preview    = true;
        public const string GuidString = "97313391-C7DF-45DB-817D-BE297DE8BD35";

        protected override async Task InitializeAsync ( CancellationToken cancellationToken, IProgress < ServiceProgressData > progress )
        {
            await base.InitializeAsync ( cancellationToken, progress );

            await JoinableTaskFactory.SwitchToMainThreadAsync ( cancellationToken );

            await ApplyCustomTool.InitializeAsync ( this );
        }
    }
}
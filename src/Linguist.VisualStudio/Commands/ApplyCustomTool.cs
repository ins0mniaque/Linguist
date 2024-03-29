﻿using System;
using System.ComponentModel.Design;

using EnvDTE;

using Microsoft.VisualStudio.ComponentModelHost;
using Microsoft.VisualStudio.Shell;

using NuGet.VisualStudio;

namespace Linguist.VisualStudio
{
    using Task = System.Threading.Tasks.Task;

    internal sealed class ApplyCustomTool
    {
        public static readonly Guid CommandSet = new Guid ( "79EF3C41-702D-4334-9A18-85CC8693758E" );

        public const int PublicCommandId   = 0x0100;
        public const int InternalCommandId = 0x0101;

        private static DTE dte;

        private ApplyCustomTool ( AsyncPackage package, OleMenuCommandService commandService )
        {
            commandService = commandService ?? throw new ArgumentNullException ( nameof ( commandService ) );
            commandService.AddCommand ( CreateMenuCommand ( PublicCommandId,   PublicExecute   ) );
            commandService.AddCommand ( CreateMenuCommand ( InternalCommandId, InternalExecute ) );
        }

        private static MenuCommand CreateMenuCommand ( int commandID, EventHandler invokeHandler )
        {
            return new OleMenuCommand ( invokeHandler, new CommandID ( CommandSet, commandID ) )
                   {
                       // NOTE: This will defer visibility control to the VisibilityConstraints section in the .vsct file
                       Supported = false
                   };
        }

        public static ApplyCustomTool Instance { get; private set; }

        public static async Task InitializeAsync ( AsyncPackage package )
        {
            await ThreadHelper.JoinableTaskFactory.SwitchToMainThreadAsync ( package.DisposalToken );

            dte = await package.GetServiceAsync ( typeof ( DTE ) ) as DTE ??
                  throw new ServiceUnavailableException ( typeof ( DTE ) );

            var commandService = await package.GetServiceAsync ( typeof ( IMenuCommandService ) ) as OleMenuCommandService ??
                                 throw new ServiceUnavailableException ( typeof ( OleMenuCommandService ) );

            Instance = new ApplyCustomTool ( package, commandService );
        }

        private void PublicExecute ( object sender, EventArgs e )
        {
            ThreadHelper.ThrowIfNotOnUIThread ( );

            SetCustomTool ( LinguistCodeGenerator.Name );
        }

        private void InternalExecute ( object sender, EventArgs e )
        {
            ThreadHelper.ThrowIfNotOnUIThread ( );

            SetCustomTool ( InternalLinguistCodeGenerator.Name );
        }

        private void SetCustomTool ( string customTool )
        {
            ThreadHelper.ThrowIfNotOnUIThread ( );

            var item = dte.SelectedItems.Item ( 1 ).ProjectItem;
            if ( item == null )
                return;

            var project = item.ContainingProject;

            var package = "Linguist";
            if ( project.HasReference ( "PresentationFramework" ) )
                package = "Linguist.WPF";

            if ( ! project.HasReference ( package ) )
            {
                var componentModel = (IComponentModel) Package.GetGlobalService ( typeof ( SComponentModel ) );
                var installer      = componentModel.GetService < IVsPackageInstaller2 > ( );

                installer.InstallLatestPackage ( null, project, package, LinguistPackage.Preview, false );
            }

            item.Properties.Item ( "CustomTool" ).Value = customTool;
        }
    }
}
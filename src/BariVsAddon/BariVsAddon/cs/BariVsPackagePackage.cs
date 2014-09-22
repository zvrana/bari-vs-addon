using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using EnvDTE;
using BariVsAddon.BariExtension;
using Microsoft.VisualStudio;
using Microsoft.VisualStudio.Shell.Interop;
using Microsoft.VisualStudio.OLE.Interop;
using Microsoft.VisualStudio.Shell;
using Commands = BariVsAddon.BariExtension.Commands;

namespace BariVsAddon
{
    // This attribute tells the PkgDef creation utility (CreatePkgDef.exe) that this class is
    // a package.
    [PackageRegistration(UseManagedResourcesOnly = true)]
    // This attribute is used to register the information needed to show this package
    // in the Help/About dialog of Visual Studio.
    [ProvideAutoLoad(UIContextGuids80.SolutionExists)]
    [InstalledProductRegistration("#110", "#112", "1.0", IconResourceID = 400)]
    [Guid(GuidList.GuidBariVsPackagePkgString)]
    public sealed class BariVsPackagePackage : Package, IOleCommandTarget, IVsServiceProvider
    {
        private KeyboardHook keyboardHook;
        private SolutionWatcher solutionWatcher;
        private Commands commands;

        /// <summary>
        /// Initialization of the package; this method is called right after the package is sited, so this is the place
        /// where you can put all the initialization code that rely on services provided by VisualStudio.
        /// </summary>
        protected override void Initialize()
        {
            Debug.WriteLine("Entering Initialize() of: {0}", this);
            base.Initialize();

            commands = new Commands(this);

            RegisterPriorityCommandTarget();
            RegisterKeyboardHook();
            RegisterFileSystemWatcher();
        }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);

            if (keyboardHook != null)
            {
                keyboardHook.Dispose();
                keyboardHook = null;
            }

            if (solutionWatcher != null)
            {
                solutionWatcher.Dispose();
                solutionWatcher = null;
            }
        }

        private void HandleKeyPressed(Keys keyCode)
        {
            if (keyCode == Keys.Cancel)
            {
                var solutionInfo = new SolutionInfo(GetDte());
                if (solutionInfo.IsBariSolution)
                commands.CancelAnyPreviousBariAction();   
            }
        }

        private void RegisterKeyboardHook()
        {
            keyboardHook = new KeyboardHook(HandleKeyPressed);
        }

        private void RegisterFileSystemWatcher()
        {
            var solutionInfo = new SolutionInfo(GetDte());

            var bariDir = solutionInfo.BariWorkingDirectory;
            if (bariDir == null) return;

            var srcDir = Path.Combine(bariDir, "src");

            if (!Directory.Exists(srcDir)) return;

            solutionWatcher = new SolutionWatcher(srcDir);
            solutionWatcher.Changed += SolutionWatcherOnChanged;
        }

        private void SolutionWatcherOnChanged(object sender, EventArgs eventArgs)
        {
            commands.IsBuildNeeded = true;
        }

        private void RegisterPriorityCommandTarget()
        {
            var vsRegisterPriorityCommandTarget =
                (IVsRegisterPriorityCommandTarget) GetService(typeof (SVsRegisterPriorityCommandTarget));
            if (vsRegisterPriorityCommandTarget == null) return;
            uint registerCookie;
            vsRegisterPriorityCommandTarget.RegisterPriorityCommandTarget(0, this, out registerCookie);
        }

        int IOleCommandTarget.Exec(ref Guid pguidCmdGroup, uint nCmdID, uint nCmdexecopt, IntPtr pvaIn, IntPtr pvaOut)
        {
            var actionMap = new Dictionary<VSConstants.VSStd97CmdID, Action>
                                {
                                    {VSConstants.VSStd97CmdID.BuildSln, commands.ExecuteBariBuild},
                                    {VSConstants.VSStd97CmdID.RebuildSln, commands.ExecuteBariRebuild},
                                    {VSConstants.VSStd97CmdID.CleanSln, commands.ExecuteBariClean},
                                    {VSConstants.VSStd97CmdID.StartNoDebug, commands.ExecuteStartWithoutDebugger},
                                    {VSConstants.VSStd97CmdID.Start, commands.ExecuteStartWithDebugger},
                                    {VSConstants.VSStd97CmdID.CancelBuild, commands.CancelAnyPreviousBariAction},
                                    {VSConstants.VSStd97CmdID.Stop, commands.StopDebugger}
                                };

            var allowedWhenDebugging = new HashSet<VSConstants.VSStd97CmdID>
                                            {
                                                VSConstants.VSStd97CmdID.Stop,
                                                VSConstants.VSStd97CmdID.Start  // this means 'Continue' when debugging...
                                            };

            if (pguidCmdGroup == VSConstants.GUID_VSStandardCommandSet97)
            {
                var vsStd97CmdID = ToVSStd97CmdID(nCmdID);
                if (vsStd97CmdID.HasValue)
                {
                    Action action;
                    if (actionMap.TryGetValue(vsStd97CmdID.Value, out action))
                    {
                        var solutionInfo = new SolutionInfo(GetDte());
                        if (solutionInfo.IsBariSolution)
                        {
                            if (!allowedWhenDebugging.Contains(vsStd97CmdID.Value))
                            {
                                if (commands.IsDebugging()) return VSConstants.S_OK;
                            }
                            action();
                            return VSConstants.S_OK;
                        }
                    }
                }
                return (int)Microsoft.VisualStudio.OLE.Interop.Constants.OLECMDERR_E_NOTSUPPORTED;
            }
            return (int)Microsoft.VisualStudio.OLE.Interop.Constants.OLECMDERR_E_UNKNOWNGROUP;
        }

        private static VSConstants.VSStd97CmdID? ToVSStd97CmdID(uint nCmdID)
        {
            if (nCmdID > Int32.MaxValue)
            {
                return null;
            }
            var iCmdId = (int)nCmdID;
            if (!typeof(VSConstants.VSStd97CmdID).IsEnumDefined(iCmdId))
            {
                return null;
            }
            var cmdID = (VSConstants.VSStd97CmdID)iCmdId;
            return cmdID;
        }

        public DTE GetDte()
        {
            return GetService<DTE>();
        }

        public T GetService<T>()
        {
            return (T) GetService(typeof (T));
        }
    }
}


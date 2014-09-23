using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using EnvDTE;
using Microsoft.VisualStudio.Shell.Interop;
using Process = System.Diagnostics.Process;

namespace BariVsAddon.BariExtension
{
    public class Commands
    {
        private readonly IVsServiceProvider owner;
        private BariShell bariShell;
        private bool isBuildNeeded;
        private Process debuggedProcess;

        public Commands(IVsServiceProvider owner)
        {
            this.owner = owner;
        }

        private DTE GetDte()
        {
            return owner.GetDte();
        }

        private void ShowBuildStatus()
        {
            //var dte = GetDte();
            // dte.StatusBar.Progress(true, "Building...");
        }

        private void HideBuildStatus(bool cancelled)
        {
            //var dte = GetDte();
            // dte.StatusBar.Progress(false);
        }

        public void ExecuteBariBuild()
        {
            ShowBuildStatus();
            ExecuteBariBuild(HideBuildStatus);
        }

        private void ExecuteBariBuild(Action<bool> after)
        {
            ExecuteBariAction("build", after);
        }

        public void ExecuteBariRebuild()
        {
            ExecuteBariAction("rebuild", HideBuildStatus);
        }

        public void ExecuteBariClean()
        {
            ExecuteBariAction("clean");
        }

        private void ExecuteBariAction(string actionName, Action<bool> after = null)
        {
            var solutionInfo = new SolutionInfo(GetDte());

            CancelAnyPreviousBariAction();

            var output = owner.GetService<SVsOutputWindow>() as IVsOutputWindow;
            var bariOutputPane = new BariOutputPane(output);
            bariOutputPane.Clear();
            bariOutputPane.WriteLine(string.Format("Executing bari {0}...\n", actionName));

            var workingDirectory = solutionInfo.BariWorkingDirectory;

            var bariConfig = solutionInfo.BariConfig;

            bariShell = new BariShell(bariConfig.BariPath, bariConfig.Goal, bariConfig.Target, workingDirectory, bariOutputPane);
            bariShell.ExecuteAsync(actionName, cancelled =>
            {
                CancelAnyPreviousBariAction();
                if (!cancelled)
                {
                    isBuildNeeded = false;
                }
                if (after != null)
                {
                    after(cancelled);
                }
            });
        }

        public void CancelAnyPreviousBariAction()
        {
            if (bariShell != null)
            {
                bariShell.CancelAll();
                bariShell = null;
            }
        }

        public void StopDebugger()
        {
            if (debuggedProcess != null)
            {
                var dte = GetDte();
                try
                {
                    dte.Debugger.TerminateAll();
                    debuggedProcess.Kill();
                }
                catch (InvalidOperationException)
                {

                }
                debuggedProcess = null;
            }
        }

        public bool IsDebugging()
        {
            var dte = GetDte();
            if (dte == null) return false;
            if (dte.Debugger == null) return false;

            return dte.Debugger.DebuggedProcesses.Count > 0;
        }

        public void ExecuteStartWithDebugger()
        {
            if (IsBuildNeeded)
            {
                var promptStopDebuggerResult = PromptStopDebugger();
                if (promptStopDebuggerResult == PromptStopDebuggerResult.Cancel) return;

                if (promptStopDebuggerResult == PromptStopDebuggerResult.StopDebuggerAndExecuteAction)
                {
                    StopDebugger();

                    ExecuteBariBuild(StartWithDebugger);
                    return;
                }
            }
            
            StartWithDebugger(false);
        }

        private void ExecuteStartWithoutDebugger(bool cancelled)
        {
            if (IsBuildNeeded)
            {
                var promptStopDebuggerResult = PromptStopDebugger();
                if (promptStopDebuggerResult == PromptStopDebuggerResult.Cancel) return;

                if (promptStopDebuggerResult == PromptStopDebuggerResult.StopDebuggerAndExecuteAction)
                {
                    StopDebugger();

                    ExecuteBariBuild(StartWithoutDebugger);
                    return;
                }
            }
            
            StartWithoutDebugger(false);
        }

        private PromptStopDebuggerResult PromptStopDebugger()
        {
            var dialog = new PromptStopDebuggerDialog();
            dialog.ShowDialog();
            return dialog.Result;
        }

        public void ExecuteStartWithoutDebugger()
        {
            ExecuteStartWithoutDebugger(false);
        }

        public void StartWithDebugger(bool cancelled)
        {
            HideBuildStatus(cancelled);

            if (cancelled) return;

            new SolutionInfo(GetDte());
            var dte = GetDte();
            if (dte.Debugger.DebuggedProcesses.Count > 0)
            {
                dte.Debugger.Go(false);
                return;
            }

            var processId = StartProcess();

            AttachDebugger(processId);
        }

        public void StartWithoutDebugger(bool cancelled)
        {
            HideBuildStatus(cancelled);

            if (!cancelled) StartProcess();
        }

        private void AttachDebugger(int processId)
        {
            var dte = GetDte();
            var dteProcess = dte.Debugger.LocalProcesses.OfType<EnvDTE.Process>().FirstOrDefault(p => p.ProcessID == processId);
            if (dteProcess != null)
            {
                dteProcess.Attach();
            }
        }

        private int StartProcess()
        {
            var solutionInfo = new SolutionInfo(GetDte());

            var solutionDir = solutionInfo.TargetWorkingDirectory;
            if (solutionDir != null)
            {
                var startupProjectName = ((Array)solutionInfo.Solution.SolutionBuild.StartupProjects).Cast<string>().First();
                var startupProject = solutionInfo.Solution.Projects.Item(startupProjectName);
                var configurationManager = startupProject.ConfigurationManager;
                var activeConfiguration = configurationManager.ActiveConfiguration;
                var startParameters = StartParameters.FromProperties(activeConfiguration.Properties);

                var exeName = Path.Combine(solutionDir, GetExeName(startParameters, startupProject));
                try
                {
                    var processStartInfo = new ProcessStartInfo(exeName)
                    {
                        WorkingDirectory = solutionDir,
                        Arguments = startParameters.StartArguments,
                        UseShellExecute = false
                    };

                    debuggedProcess = new Process { StartInfo = processStartInfo };
                    debuggedProcess.Start();
                    var processId = debuggedProcess.Id;
                    return processId;
                }
                catch (Exception ex)
                {
                    MessageBox.Show(
                        string.Format(
                            "Failed to start '{0}'.{1}" +
                            "Check Debug\\Properties\\Debug\\Startup action.{1}{1}" +
                            "{2}", exeName, Environment.NewLine, ex),
                        "Error",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Error);
                }
            }
            return -1;
        }

        private static string GetExeName(StartParameters startParameters, Project startupProject)
        {
            if (startParameters.StartAction == StartAction.Program)
            {
                return Path.GetFileName(startParameters.StartProgram);
            }

            if (startParameters.StartAction == StartAction.StartupProject)
            {
                return string.Format("{0}.exe", Path.GetFileNameWithoutExtension(startupProject.FileName));
            }
            return null;
        }

        public bool IsBuildNeeded
        {
            get { return isBuildNeeded; }
            set { isBuildNeeded = value; }
        }
    }
}

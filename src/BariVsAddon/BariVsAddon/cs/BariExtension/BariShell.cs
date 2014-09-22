using System;
using System.Diagnostics;
using System.Threading.Tasks;

namespace BariVsAddon.BariExtension
{
    public class BariShell
    {
        private readonly string bariPath;
        private readonly BariOutputPane bariOutputPane;
        private readonly string productName;
        private readonly string workingDirectory;
        private bool isCancellationRequested;
        private string goal;

        public BariShell(string bariPath, string goal, string productName, string workingDirectory, BariOutputPane bariOutputPane)
        {
            this.bariPath = bariPath;
            this.bariOutputPane = bariOutputPane;
            this.goal = goal;
            this.productName = productName;
            this.workingDirectory = workingDirectory;
        }

        public void Execute(string actionName, Action<bool> after = null)
        {
            var arguments = string.Format("--target {0} {1} {2}", goal, actionName, productName);
            ShowOutput(string.Format("{0} {1}", bariPath, arguments));
            var processStartInfo = new ProcessStartInfo(
                bariPath,
                arguments)
                                       {
                                           RedirectStandardOutput = true,
                                           RedirectStandardError = true,
                                           UseShellExecute = false,
                                           CreateNoWindow = true,
                                           WorkingDirectory = workingDirectory,
                                       };

            var proc = new Process {StartInfo = processStartInfo};
            proc.OutputDataReceived += (sendingProcess, outLine)
                                       => ShowOutput(outLine.Data);

            proc.Start();
            proc.BeginOutputReadLine();
            var cancelled = false;

            while (true)
            {
                proc.WaitForExit(100);
                if (isCancellationRequested)
                {
                    proc.Kill();
                    ShowOutput("Build cancelled.");
                    cancelled = true;
                    break;
                }
                if (proc.HasExited) break;
            }

            if (after != null)
            {
                after(cancelled);
            }
        }

        private void ShowOutput(string data)
        {
            bariOutputPane.WriteLine(string.Format("{0}", data));
        }

        public void ExecuteAsync(string actionName, Action<bool> after)
        {
            isCancellationRequested = false;
            Task.Factory.StartNew(() => Execute(actionName, after));
        }

        public void CancelAll()
        {
            isCancellationRequested = true;
        }
    }
}
using System;
using System.Text.RegularExpressions;
using BariVsAddon.BariExtension.Wrappers;
using Microsoft.VisualStudio;
using Microsoft.VisualStudio.Shell.Interop;

namespace BariVsAddon.BariExtension
{
    public class BariOutputPane
    {
        private readonly IVsOutputWindow outputWindow;

        //private readonly ProxyFactory proxyFactory = new ProxyFactory();
        private readonly WrapperFactory wrapperFactory = new WrapperFactory();

        public BariOutputPane(IVsOutputWindow outputWindow)
        {
            this.outputWindow = wrapperFactory.WrapThrowOnError(outputWindow);
        }

        public void WriteLine(string s)
        {
            if (outputWindow == null) return;

            var pane = GetBariOutputPane();
            if (pane == null) return;

            pane.OutputString(s);
            pane.OutputString(Environment.NewLine);
            pane.Activate();
        }

        private ParsedMessage ToParsedMessage(string s)
        {
            var matches = Regex.Match(s, @"(?<filename>[\w\\.]+)\((?<line>\d+),(?<column>\d+)\)(: (?<message>[^\[]*))?");
            if (matches.Groups["filename"] != null)
            {
                return new ParsedMessage(
                    matches.Groups["filename"].Value,
                    Convert.ToInt32(matches.Groups["line"].Value),
                    Convert.ToInt32(matches.Groups["column"].Value),
                    matches.Groups["message"].Value
               );
            }
            return null;
        }

        private IVsOutputWindowPane GetBariOutputPane()
        {
            var generalPaneGuid = VSConstants.GUID_BuildOutputWindowPane;
            IVsOutputWindowPane pane;

            // fetch the pane wrapped in error handling:
            outputWindow.GetPane(ref generalPaneGuid, out pane);
            if (pane == null)
            {
                // TODO: hogy kell comban truet mondani???
                const int theTrue = 1;
                outputWindow.CreatePane(generalPaneGuid, "Build", theTrue, theTrue);

                outputWindow.GetPane(ref generalPaneGuid, out pane);
            }
            return wrapperFactory.WrapThrowOnError(pane);
        }

        public void Clear()
        {
            var pane = GetBariOutputPane();
            if (pane == null) return;

            pane.Clear();
        }
    }
}

using System.ComponentModel;
using Microsoft.VisualStudio;
using Microsoft.VisualStudio.Shell.Interop;

namespace BariVsAddon.BariExtension.Wrappers
{
    class VsOutputWindowPaneWrapper : IVsOutputWindowPane
    {
        private readonly IVsOutputWindowPane target;

        public VsOutputWindowPaneWrapper(IVsOutputWindowPane target)
        {
            this.target = target;
        }

        public int OutputString(string pszOutputString)
        {
            var result = target.OutputString(pszOutputString);

            if (ErrorHandler.Failed(result))
            {
                throw new Win32Exception(result);
            }

            return result;
        }

        public int Activate()
        {
            var result = target.Activate();

            if (ErrorHandler.Failed(result))
            {
                throw new Win32Exception(result);
            }

            return result;
        }

        public int Hide()
        {
            var result = target.Hide();

            if (ErrorHandler.Failed(result))
            {
                throw new Win32Exception(result);
            }

            return result;
        }

        public int Clear()
        {
            var result = target.Clear();

            if (ErrorHandler.Failed(result))
            {
                throw new Win32Exception(result);
            }

            return result;
        }

        public int FlushToTaskList()
        {
            var result = target.FlushToTaskList();

            if (ErrorHandler.Failed(result))
            {
                throw new Win32Exception(result);
            }

            return result;
        }

        public int OutputTaskItemString(string pszOutputString, VSTASKPRIORITY nPriority, VSTASKCATEGORY nCategory, string pszSubcategory, int nBitmap, string pszFilename, uint nLineNum, string pszTaskItemText)
        {
            var result = target.OutputTaskItemString(pszOutputString, nPriority, nCategory, pszSubcategory, nBitmap, pszFilename, nLineNum, pszTaskItemText);

            if (ErrorHandler.Failed(result))
            {
                throw new Win32Exception(result);
            }

            return result;
        }

        public int OutputTaskItemStringEx(string pszOutputString, VSTASKPRIORITY nPriority, VSTASKCATEGORY nCategory, string pszSubcategory, int nBitmap, string pszFilename, uint nLineNum, string pszTaskItemText, string pszLookupKwd)
        {
            var result = target.OutputTaskItemStringEx(pszOutputString, nPriority, nCategory, pszSubcategory, nBitmap, pszFilename, nLineNum, pszTaskItemText, pszLookupKwd);

            if (ErrorHandler.Failed(result))
            {
                throw new Win32Exception(result);
            }

            return result;
        }

        public int GetName(ref string pbstrPaneName)
        {
            var result = target.GetName(ref pbstrPaneName);

            if (ErrorHandler.Failed(result))
            {
                throw new Win32Exception(result);
            }

            return result;
        }

        public int SetName(string pszPaneName)
        {
            var result = target.SetName(pszPaneName);

            if (ErrorHandler.Failed(result))
            {
                throw new Win32Exception(result);
            }

            return result;
        }

        public int OutputStringThreadSafe(string pszOutputString)
        {
            var result = target.OutputStringThreadSafe(pszOutputString);

            if (ErrorHandler.Failed(result))
            {
                throw new Win32Exception(result);
            }

            return result;
        }
    }
}

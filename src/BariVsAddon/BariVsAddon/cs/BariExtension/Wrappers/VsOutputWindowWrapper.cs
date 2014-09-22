using System;
using System.ComponentModel;
using Microsoft.VisualStudio;
using Microsoft.VisualStudio.Shell.Interop;

namespace BariVsAddon.BariExtension.Wrappers
{
    class VsOutputWindowWrapper: IVsOutputWindow
    {
        private readonly IVsOutputWindow target;

        public VsOutputWindowWrapper(IVsOutputWindow target)
        {
            this.target = target;
        }

        public int GetPane(ref Guid rguidPane, out IVsOutputWindowPane ppPane)
        {
            var result = target.GetPane(ref rguidPane, out ppPane);

            if (ErrorHandler.Failed(result))
            {
                throw new Win32Exception(result);
            }

            return result;
        }

        public int CreatePane(ref Guid rguidPane, string pszPaneName, int fInitVisible, int fClearWithSolution)
        {
            var result = target.CreatePane(ref rguidPane, pszPaneName, fInitVisible, fClearWithSolution);

            if (ErrorHandler.Failed(result))
            {
                throw new Win32Exception(result);
            }

            return result;
        }

        public int DeletePane(ref Guid rguidPane)
        {
            var result = target.DeletePane(ref rguidPane);

            if (ErrorHandler.Failed(result))
            {
                throw new Win32Exception(result);
            }

            return result;
        }
    }
}

using Microsoft.VisualStudio.Shell.Interop;

namespace Microsoft.BariVsPackage.BariExtension.Wrappers
{
    public class WrapperFactory
    {
        public T WrapThrowOnError<T>(T target) where T : class
        {
            if (target == null) return null;
            object obj = null;
            if (target is IVsOutputWindow) obj = new VsOutputWindowWrapper((IVsOutputWindow)target) as T;
            if (target is IVsOutputWindowPane) obj = new VsOutputWindowPaneWrapper((IVsOutputWindowPane)target) as T;
            return (T)obj;
            // TODO: use dynamic proxy
            /*
                        var wrapper = new ThrowOnErrorWrapper<T>(target);
                        return proxyFactory.CreateProxy<T>(wrapper);
             */
        }
    }
}

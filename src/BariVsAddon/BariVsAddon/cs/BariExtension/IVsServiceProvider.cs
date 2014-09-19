using EnvDTE;

namespace Microsoft.BariVsPackage.BariExtension
{
    public interface IVsServiceProvider
    {
        DTE GetDte();
        T GetService<T>();
    }
}

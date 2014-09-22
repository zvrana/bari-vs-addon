using EnvDTE;

namespace BariVsAddon.BariExtension
{
    public interface IVsServiceProvider
    {
        DTE GetDte();
        T GetService<T>();
    }
}

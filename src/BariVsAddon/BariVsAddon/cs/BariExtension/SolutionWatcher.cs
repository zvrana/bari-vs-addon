using System;
using System.IO;

namespace Microsoft.BariVsPackage.BariExtension
{
    public class SolutionWatcher: IDisposable
    {
        private FileSystemWatcher watcher;

        public SolutionWatcher(string srcDir)
        {
            watcher = new FileSystemWatcher(srcDir)
                          {
                              EnableRaisingEvents = true,
                              IncludeSubdirectories = true,
                              InternalBufferSize = 64*1024, // this is max
                              Filter = "*.cs",
                              NotifyFilter = NotifyFilters.LastAccess | NotifyFilters.LastWrite
                                             | NotifyFilters.FileName | NotifyFilters.DirectoryName | NotifyFilters.Size
                          };
            watcher.Changed += FileSystemChanged;
            watcher.Deleted += FileSystemChanged;
            watcher.Created += FileSystemChanged;
            watcher.Renamed += FileSystemChanged;
        }

        private void FileSystemChanged(object sender, FileSystemEventArgs e)
        {
            if (Changed != null)
            {
                Changed(this, EventArgs.Empty);
            }
        }

        public event EventHandler Changed;

        public void Dispose()
        {
            watcher.Dispose();
            watcher = null;
        }
    }
}
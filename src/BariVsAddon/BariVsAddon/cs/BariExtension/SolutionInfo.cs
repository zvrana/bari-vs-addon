using System.IO;
using EnvDTE;
using JetBrains.Annotations;

namespace BariVsAddon.BariExtension
{
    public class SolutionInfo
    {
        public SolutionInfo(DTE dte)
        {
            if (dte != null)
            {
                Solution = dte.Solution;
            }
        }

        [CanBeNull]
        public Solution Solution { get; private set; }

        [CanBeNull]
        public string TargetWorkingDirectory
        {
            get
            {
                if (Solution == null) return null;

                var solutionDir = Path.GetDirectoryName(Solution.FileName);
                if (solutionDir == null) return null;

                return Path.Combine(solutionDir, TargetName);
            }
        }

        [CanBeNull]
        public string BariWorkingDirectory
        {
            get
            {
                if (Solution == null) return null;

                var solutionDir = Path.GetDirectoryName(Solution.FileName);
                if (solutionDir == null) return null;

                var parentDir = Directory.GetParent(solutionDir).FullName;
                return parentDir;
            }
        }

        public string TargetName
        {
            get
            {
                var bariSolutionConfig = BariConfig;
                if (bariSolutionConfig == null) return null;

                return bariSolutionConfig.Target;
            }
        }

        public bool IsBariSolution
        {
            get { return BariConfig != null; }
        }

        public BariSolutionConfig BariConfig
        {
            get
            {
                if (Solution == null) return null;

                var yamlFileName = Path.ChangeExtension(Solution.FileName, "yaml");
                return BariSolutionConfig.FromFile(yamlFileName);
            }
        }
    }
}
using System.IO;
using System.Linq;

namespace BariVsAddon.BariExtension
{
    public class BariSolutionConfig
    {
        private const string BariPathKey = "bari-path";
        private const string GoalKey = "goal";
        private const string TargetKey = "target";
        private const string StartupPathKey = "startup-path";

        private static readonly string[] AllKeys = new[] {BariPathKey, GoalKey, TargetKey, StartupPathKey};

        public static BariSolutionConfig FromFile(string yamlFileName)
        {
            if (!File.Exists(yamlFileName)) return null;

            var lines = File.ReadAllLines(yamlFileName)
                .Where(l => !l.StartsWith(" "))
                .Select(l => l.Split(new[] {':'}, 2))
                .Where(l => l.Count() == 2)
                .ToDictionary(l => l[0].Trim(), l => l[1].Trim());

            if (AllKeys.All(lines.ContainsKey))
            {
                return new BariSolutionConfig
                           {
                               BariPath = lines[BariPathKey],
                               Goal = lines[GoalKey],
                               Target = lines[TargetKey],
                               StartupPath = lines[StartupPathKey]
                           };
            }

            return null;
        }

        public string BariPath { get; private set; }

        public string Goal { get; private set; }

        public string Target { get; private set; }

        public string StartupPath { get; private set; }
    }
}
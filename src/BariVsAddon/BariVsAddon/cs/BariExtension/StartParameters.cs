using EnvDTE;

namespace BariVsAddon.BariExtension
{
    public class StartParameters
    {
        public static StartParameters FromProperties(Properties properties)
        {
            var result = new StartParameters
                             {
                                 StartAction = (StartAction) properties.Item("StartAction").Value,
                                 StartWorkingDirectory = properties.Item("StartWorkingDirectory").Value.ToString(),
                                 StartProgram = properties.Item("StartProgram").Value.ToString(),
                                 StartArguments = properties.Item("StartArguments").Value.ToString()
                             };
            return result;
        }

        public StartAction StartAction { get; private set; }
        public string StartWorkingDirectory { get; private set; }
        public string StartProgram { get; private set; }
        public string StartArguments { get; private set; }
    }
}
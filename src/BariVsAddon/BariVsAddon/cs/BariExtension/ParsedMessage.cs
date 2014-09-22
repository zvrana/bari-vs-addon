namespace BariVsAddon.BariExtension
{
    public class ParsedMessage
    {
        public ParsedMessage(string filename, int line, int column, string message)
        {

            Filename = filename;
            Line = line;
            Column = column;
            Message = message;
        }

        public string Filename { get; private set; }

        public int Line { get; private set; }

        public int Column { get; private set; }

        public string Message { get; private set; }
    }
}
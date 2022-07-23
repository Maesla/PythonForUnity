using System.Text;

namespace MicrokernelSystem.UI
{
    /// <summary>
    /// This class provides functionality to format a string with rich text in a console display
    /// </summary>
    public class ConsoleFormatter
    {
        private const string prefixConsoleLine = ">>> ";

        public string Format(string text)
        {
            return Format(text, string.Empty, string.Empty);
        }

        public string FormatError(string text)
        {
            return Format(text, "<color=red>", "</color>");
        }

        private string Format(string text, string richTextStart, string richTextEnd)
        {
            var lines = text.Split('\n');
            StringBuilder sb = new StringBuilder(text.Length * 2);

            foreach (string line in lines)
            {
                sb.AppendLine($"{prefixConsoleLine}{richTextStart}{line}{richTextEnd}");
            }
            return sb.ToString();
        }
    }
}

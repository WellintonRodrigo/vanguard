using System.Text.RegularExpressions;

namespace Vanguard.DataCollector.Helpers
{
    public static class TextHelper
    {
        public static string Clean(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
                return string.Empty;

            return Regex.Replace(value, @"\s+", " ").Trim();
        }

    }
}

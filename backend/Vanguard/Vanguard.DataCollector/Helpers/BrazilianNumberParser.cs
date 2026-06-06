using System.Globalization;

namespace Vanguard.DataCollector.Helpers
{
    public static class BrazilianNumberParser
    {
        private static readonly CultureInfo Culture = new("pt-BR");

        public static bool TryParseDecimal(
            string value,
            out decimal result)
        {
            result = 0;

            if (string.IsNullOrWhiteSpace(value))
                return false;

            var cleanValue = TextHelper.Clean(value)
                .Replace("R$", "")
                .Replace("%", "")
                .Trim();

            return decimal.TryParse(
                cleanValue,
                NumberStyles.Number,
                Culture,
                out result);
        }
    }
}

using System.Text.RegularExpressions;
using Vanguard.DataCollector.Helpers;

namespace Vanguard.DataCollector.Normalizers
{
    public static class MarketNormalizer
    {
        // Regex genérico que identifica delimitadores comuns em títulos de cotações (- ou / ou termos de mercado)
        private static readonly Regex MarketExtractorRegex = new(
            @"(?:-|\/|(?i)Esalq|(?i)Cepea|(?i)BM&F|(?i)B3)\s*(?<market>.+)$",
            RegexOptions.Compiled);

        public static string Normalize(string rawTitle, string targetCommodity)
        {
            if (string.IsNullOrWhiteSpace(rawTitle))
                return string.Empty;

            // 1. Tenta isolar o mercado/praça após o delimitador principal
            var match = MarketExtractorRegex.Match(rawTitle);
            string extractedMarket = match.Success ? match.Groups["market"].Value : rawTitle;

            // 2. Remove a palavra da commodity se ela ainda persistir na string de mercado
            if (!string.IsNullOrWhiteSpace(targetCommodity))
            {
                extractedMarket = Regex.Replace(extractedMarket, Regex.Escape(targetCommodity), "", RegexOptions.IgnoreCase);
            }

            // 3. Limpa caracteres residuais e prefixos
            extractedMarket = extractedMarket.Replace("Indicador", "", StringComparison.OrdinalIgnoreCase)
                                            .Replace("em Casca", "", StringComparison.OrdinalIgnoreCase)
                                            .Trim()
                                            .TrimStart('-', '/', ' ')
                                            .Trim();

            return TextHelper.Clean(extractedMarket);
        }
    }
}
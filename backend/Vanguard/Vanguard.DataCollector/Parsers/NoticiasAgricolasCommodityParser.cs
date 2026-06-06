using HtmlAgilityPack;
using System.Globalization;
using System.Reflection.Metadata;
using System.Text.RegularExpressions;
using Vanguard.DataCollector.Models;
using Vanguard.Domain.Entities;

namespace Vanguard.DataCollector.Parsers
{
    // pega o HTML da página de cotações do site e extrai a data e o preço da commodity.
    public class NoticiasAgricolasCommodityParser
    {
        public CommodityPrice? Parse(
            string html,
            CommoditySource source)
        {
            var doc = new HtmlDocument();
            doc.LoadHtml(html);
            var cotacoes = doc.DocumentNode.SelectNodes("//div[contains(@class, 'cotacao')]");

            if (cotacoes is null)
                return null;

            foreach (var cotacao in cotacoes)
            {
                var title = CleanText(
                    cotacao.SelectSingleNode(".//h2/a")?.InnerText ?? string.Empty);

                if (!title.Contains(source.Commodity, StringComparison.OrdinalIgnoreCase))
                    continue;

                var table = cotacao.SelectSingleNode(".//table");

                var firstRow = table?
                    .SelectSingleNode(".//tbody/tr");

                var columns = firstRow?
                    .SelectNodes("./td");

                if (columns is null || columns.Count < 3)
                    continue;

                var dateText = CleanText(columns[0].InnerText);
                var priceText = CleanText(columns[1].InnerText);
                var variationText = CleanText(columns[2].InnerText);

                if (!DateTime.TryParseExact(
                        dateText,
                        "dd/MM/yyyy",
                        new CultureInfo("pt-BR"),
                        DateTimeStyles.None,
                        out var referenceDate))
                {
                    continue;
                }

                if (!TryParseDecimal(priceText, out var priceBrl))
                    continue;

                decimal? dailyVariation = TryParseDecimal(
                    variationText,
                    out var parsedVariation)
                        ? parsedVariation
                        : null;

                return new CommodityPrice
                {
                    Source = source.Name,
                    Commodity = source.Commodity,
                    Unit = source.Unit,
                    PriceBrl = priceBrl,
                    PriceUsd = null,
                    DailyVariationPercent = dailyVariation,
                    MonthlyVariationPercent = null,
                    ReferenceDate = DateTime.SpecifyKind(referenceDate.Date, DateTimeKind.Utc),
                    CollectedAt = DateTime.UtcNow
                };
            }

            return null;
        }

        private static string CleanText(string value)
        {
            return Regex.Replace(value, @"\s+", " ").Trim();
        }

        private static decimal? TryParseDecimalValue(string value)
        {
            return TryParseDecimal(value, out var result)
                ? result
                : null;
        }

        private static bool TryParseDecimal(
            string value,
            out decimal result)
        {
            var cleanValue = CleanText(value)
                .Replace("%", "")
                .Replace("R$", "")
                .Trim();

            return decimal.TryParse(
                cleanValue,
                NumberStyles.Number,
                new CultureInfo("pt-BR"),
                out result);
        }

    }
}

using System.Globalization;
using HtmlAgilityPack;
using Vanguard.Domain.Entities;

namespace Vanguard.DataCollector.Parsers

{
    public class CepeaCommodityParser
    {
        public IReadOnlyCollection<CommodityPrice> PriceSoja(string html)
        {
            var doc = new HtmlDocument();
            doc.LoadHtml(html);

            var text = doc.DocumentNode.InnerText;

            var lines = text
                .Split('\n')
                .Select(x => x.Trim())
                .Where(x => !string.IsNullOrEmpty(x))
                .ToList();

            var prices = new List<CommodityPrice>();

            for (var i = 0; i < lines.Count; i++) 
            {
            if(!DateTime.TryParseExact(
                lines[i],
                "dd/MM/yyyy",
                new CultureInfo("pt-BR"),
                DateTimeStyles.None,
                out DateTime refrenceDate))

                { 
                    continue;
                }
                if (i + 4 >= lines.Count)
                    continue;

                var priceBrl = ParseDecimal(lines[i + 1]);
                var dailyVariation = ParsePercent(lines[i + 2]);
                var monthlyVariation = ParsePercent(lines[i + 3]);
                var priceUsd = ParseDecimal(lines[i + 4]);

                prices.Add(new CommodityPrice
                {
                    Source = "CEPEA",
                    Commodity = "Soja",
                    Unit = "Saca 60kg",
                    PriceBrl = priceBrl,
                    PriceUsd = priceUsd,
                    DailyVariationPercent = dailyVariation,
                    MonthlyVariationPercent = monthlyVariation,
                    ReferenceDate = DateTime.SpecifyKind(refrenceDate.Date, DateTimeKind.Utc),
                    CollectedAt = DateTime.UtcNow
                });

                break;
            }
            return prices;
        }

        private static decimal ParseDecimal(string value)
        {
            return decimal.Parse(
                value.Replace("%", "").Trim(),
                new CultureInfo("pt-BR"));
        }

        private static decimal ParsePercent(string value)
        {
            return ParseDecimal(value);
        }
    }
}

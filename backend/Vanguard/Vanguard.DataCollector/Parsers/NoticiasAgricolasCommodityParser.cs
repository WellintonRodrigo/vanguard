using HtmlAgilityPack;
using System.Globalization;
using System.Reflection.Metadata;
using System.Text.RegularExpressions;
using Vanguard.DataCollector.Models;
using Vanguard.Domain.Entities;
using Vanguard.DataCollector.Helpers;

namespace Vanguard.DataCollector.Parsers
{
    // pega o HTML da página de cotações do site e extrai a data e o preço da commodity.
    public class NoticiasAgricolasCommodityParser
    {
        public IReadOnlyCollection<CommodityPrice> Parse(
            string html,
            CommoditySource source)
        {

            var document = new HtmlDocument();
            document.LoadHtml(html);

            var prices = new List<CommodityPrice>();

            var cotacoes = document.DocumentNode
                .SelectNodes("//div[contains(@class, 'cotacao')]");

            if (cotacoes is null)
                return prices;

            foreach (var cotacao in cotacoes)
            {
                var title = TextHelper.Clean(
                    cotacao.SelectSingleNode(".//h2/a")?.InnerText ?? string.Empty);

                if (string.IsNullOrWhiteSpace(title))
                    continue;

                var table = cotacao.SelectSingleNode(".//table");

                var firstRow = table?
                    .SelectSingleNode(".//tbody/tr");

                var columns = firstRow?
                    .SelectNodes("./td");
                
                if (columns is null || columns.Count < 3)
                    continue;

                var dateText = TextHelper.Clean(columns[0].InnerText);
                var priceText = TextHelper.Clean(columns[1].InnerText);
                var variationText = TextHelper.Clean(columns[2].InnerText);

                if (!DateTime.TryParseExact(
                        dateText,
                        "dd/MM/yyyy",
                        new CultureInfo("pt-BR"),
                        DateTimeStyles.None,
                        out var referenceDate))
                {
                    continue;
                }

                
                if (!BrazilianNumberParser.TryParseDecimal(priceText, out var priceBrl)) 
                {
                    Console.WriteLine($"ERRO AO CONVERTER PREÇO: '{priceText}'");
                    continue;
                }
                    

                decimal? dailyVariation = BrazilianNumberParser.TryParseDecimal(
                    variationText,
                    out var parsedVariation)
                        ? parsedVariation
                        : null;
               
                prices.Add(new CommodityPrice
                {
                    Source = source.Name,
                    Commodity = title,
                    Unit = source.Unit,
                    PriceBrl = priceBrl,
                    PriceUsd = null,
                    DailyVariationPercent = dailyVariation,
                    MonthlyVariationPercent = null,
                    ReferenceDate = DateTime.SpecifyKind(referenceDate.Date, DateTimeKind.Utc),
                    CollectedAt = DateTime.UtcNow
                });
            }

            return prices;
        }

    }
}

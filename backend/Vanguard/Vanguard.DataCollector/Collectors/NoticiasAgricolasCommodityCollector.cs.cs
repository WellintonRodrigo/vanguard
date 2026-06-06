using System.Diagnostics;
using Vanguard.DataCollector.Collectors.Interfaces;
using Vanguard.DataCollector.Configuration;
using Vanguard.DataCollector.Models;
using Vanguard.DataCollector.Parsers;
using Vanguard.Domain.Entities;

namespace Vanguard.DataCollector.Collectors
{
    public class NoticiasAgricolasCommodityCollector : ICommodityCollector
    {
        // Coletor generoco para as commodities do site NoticiasAgricolas, ele pode ser configurado para coletar tanto soja quanto milho, ou outras commodities que o site venha a adicionar no futuro, desde que a estrutura da página seja similar e mutiplas fonte de dados.

        private readonly HttpClient _httpClient;
        private readonly NoticiasAgricolasCommodityParser _parser;

        public NoticiasAgricolasCommodityCollector(
            HttpClient httpClient,
            NoticiasAgricolasCommodityParser parser)
        {
            _httpClient = httpClient;
            _parser = parser;
        }

        public async Task<IReadOnlyCollection<CommodityPrice>> CollectAsync(
            CancellationToken cancellationToken = default)
        {
            var prices = new List<CommodityPrice>();

            var sources = CommoditySourceCatalog.sources
                .Where(x=> x.Enabled && x.Name== "NoticiasAgricolas")
                .ToList();

            foreach (var source in sources) 
            {
                Console.WriteLine($"Coletando {source.Commodity} - {source.Url}");

                using var request = new HttpRequestMessage(
               HttpMethod.Get,
               source.Url);

                request.Headers.UserAgent.ParseAdd(
                    "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 Chrome/120.0 Safari/537.36");

                request.Headers.Accept.ParseAdd(
                    "text/html,application/xhtml+xml,application/xml;q=0.9,*/*;q=0.8");

                request.Headers.AcceptLanguage.ParseAdd(
                    "pt-BR,pt;q=0.9,en-US;q=0.8,en;q=0.7");

                var response = await _httpClient.SendAsync(
                    request,
                    cancellationToken);

                if (!response.IsSuccessStatusCode)
                {
                    Console.WriteLine(
        $"Falha ao coletar {source.Commodity} - Status: {(int)response.StatusCode} - {response.ReasonPhrase} - URL: {source.Url}");

                    continue;
                }
                

                var html = await response.Content.ReadAsStringAsync(
                    cancellationToken);

                if (CollectorSettings.SalveHtmlDebug)
                {

                    var debugDirectry = Path.Combine(
                        Directory.GetCurrentDirectory(), "Debug");

                    Directory.CreateDirectory(debugDirectry);

                    var fileName = $"{source.Name}_{source.Commodity}_{DateTime.UtcNow:yyyyMMddHHmmss}.html";

                    var filePath = Path.Combine(debugDirectry, fileName);

                    await File.WriteAllTextAsync(filePath, html, cancellationToken);

                    Console.WriteLine($"HTML salvo em: {filePath}");
                }
                var parsedPrice = _parser.Parse(html, source);

                if (!parsedPrice.Any())
                {
                    Console.WriteLine(
                    $"Parser não encontrou preços para {source.Commodity} - {source.Url}");
                    continue;
                }
                    prices.AddRange(parsedPrice);               
            }
            return prices;
        }
    }
}

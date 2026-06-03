using Vanguard.DataCollector.Collectors.Interfaces;
using Vanguard.DataCollector.Parsers;
using Vanguard.Domain.Entities;

namespace Vanguard.DataCollector.Collectors
{
    public class CepeaCommodityCollector : ICommodityCollector
    {
        private readonly CepeaCommodityParser _parser;
        private readonly HttpClient _httpClient;

        public CepeaCommodityCollector(CepeaCommodityParser parser, HttpClient httpClient)
        {
            _parser = parser;
            _httpClient = httpClient;
        }
        public async Task<IReadOnlyCollection<CommodityPrice>> CollectAsync(
            CancellationToken cancellationToken = default)
        {
            const string url = "https://cepea.org.br/br/indicador/soja.aspx";

            using var request = new HttpRequestMessage(HttpMethod.Get, url);

            request.Headers.AcceptLanguage.ParseAdd("pt-BR,pt;q=0.9,en-US;q=0.8,en;q=0.7");

            var response = await _httpClient.SendAsync(request, cancellationToken);

            response.EnsureSuccessStatusCode();


            var html  = await response.Content.ReadAsStringAsync(cancellationToken);

            return _parser.PriceSoja(html);
        }
    }
}

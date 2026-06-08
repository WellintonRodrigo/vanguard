using System.Diagnostics;
using Vanguard.DataCollector.Health.Enuns;
using Vanguard.DataCollector.Health.Interfaces;
using Vanguard.DataCollector.Models;
using Vanguard.DataCollector.Parsers;

namespace Vanguard.DataCollector.Health.HealthCheckers
{
    public class NoticiasAgricolasHealthChecker : ICollectorHealthChecker
    {
        private readonly HttpClient _httpClient;
        private readonly NoticiasAgricolasCommodityParser _parser;

        public NoticiasAgricolasHealthChecker(
            HttpClient httpClient,
            NoticiasAgricolasCommodityParser parser)
        {
            _httpClient = httpClient;
            _parser = parser;
        }

        public async Task<IReadOnlyCollection<CollectorHealthResult>> CheckAsync(
            CancellationToken cancellationToken = default)
        {
            var result = new List<CollectorHealthResult>();

            var sources = CommoditySourceCatalog.sources
                .Where(x=> x.Enabled && x.Name == "NoticiasAgricolas")
                .ToList();

            foreach(var source in sources)
            {
                var stopwatch = Stopwatch.StartNew();

                try
                {
                    using var request = new HttpRequestMessage(HttpMethod.Get, source.Url);

                    request.Headers.UserAgent.ParseAdd(
                  "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 Chrome/120.0 Safari/537.36");

                    request.Headers.Accept.ParseAdd(
                        "text/html,application/xhtml+xml,application/xml;q=0.9,*/*;q=0.8");

                    request.Headers.AcceptLanguage.ParseAdd(
                        "pt-BR,pt;q=0.9,en-US;q=0.8,en;q=0.7");

                    var response = await _httpClient.SendAsync(request, cancellationToken);

                    stopwatch.Stop();

                    if (!response.IsSuccessStatusCode)
                    {
                        result.Add(new CollectorHealthResult
                        {
                            Source = source.Name,
                            Commodity = source.Commodity,
                            Url = source.Url,
                            Status = CollectorHealthStatus.Unhealthy,
                            HttpStatusCode = (int)response.StatusCode,
                            RecordsFound = 0,
                            ResponseTimeMs = stopwatch.ElapsedMilliseconds,
                            ErrorMessage = response.ReasonPhrase
                        });
                        continue;
                    }

                    var html = await response.Content.ReadAsStringAsync(
                        cancellationToken);

                    var parsedPrices = _parser.Parse(html, source);

                    var status = parsedPrices.Any()
                        ? CollectorHealthStatus.Healthy
                        : CollectorHealthStatus.Unhealthy;

                    result.Add(new CollectorHealthResult
                    {
                        Source = source.Name,
                        Commodity = source.Commodity,
                        Url = source.Url,
                        Status = status,
                        HttpStatusCode = (int)response.StatusCode,
                        RecordsFound = parsedPrices.Count,
                        ResponseTimeMs = stopwatch.ElapsedMilliseconds,
                        ErrorMessage = parsedPrices.Any()
                        ? null
                        : "Parser não encontrou registros na página."
                    });
                }
                catch (Exception ex)
                {
                    stopwatch.Stop();
                    result.Add(new CollectorHealthResult
                    {
                        Source = source.Name,
                        Commodity = source.Commodity,
                        Url = source.Url,
                        Status = CollectorHealthStatus.Unhealthy,
                        HttpStatusCode = null,
                        RecordsFound = 0,
                        ResponseTimeMs = stopwatch.ElapsedMilliseconds,
                        ErrorMessage = ex.Message
                    });
                }
            }
            return result;
        }
    }
}

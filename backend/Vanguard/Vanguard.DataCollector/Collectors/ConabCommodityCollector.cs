using System.Globalization;
using System.Text.Json;
using Vanguard.Domain.Entities;
using Vanguard.Domain.Interfaces;

namespace Vanguard.DataCollector.Collectors;

public class ConabCommodityCollector
{
    private readonly HttpClient _httpClient;
    private readonly ICommodityPriceRepository _repository;

    public ConabCommodityCollector(HttpClient httpClient, ICommodityPriceRepository repository)
    {
        _httpClient = httpClient;
        _repository = repository;
    }

    public async Task CollectAsync(CancellationToken cancellationToken = default)
    {
        // Endpoint do portal de dados abertos / cotações da CONAB
        var requestUrl = "https://www.conab.gov.br/api/cotacoes/diarias"; // Exemplo de rota de dados abertos da CONAB

        try
        {
            var response = await _httpClient.GetAsync(requestUrl, cancellationToken);

            if (!response.IsSuccessStatusCode)
                return;

            var jsonContent = await response.Content.ReadAsStringAsync(cancellationToken);

            // Mapeamento e normalização dos dados recebidos da CONAB
            var collectedPrices = ParseConabData(jsonContent);

            if (collectedPrices.Any())
            {
                foreach (var price in collectedPrices)
                {
                    // Verifica duplicação no banco usando a interface ICommodityPriceRepository que já possui ExistsAsync
                    var exists = await _repository.ExistsAsnyc(price.Commodity, price.ReferenceDate, price.Source, cancellationToken);
                    if (!exists)
                    {
                        await _repository.InsertManyAsync(new[] { price }, cancellationToken);
                    }
                }
            }
        }
        catch (Exception ex)
        {
            // O log do DataCollectionWorker irá capturar e registar a falha mantendo a saúde do sistema sob controlo
            throw new InvalidOperationException($"Falha ao coletar dados da CONAB: {ex.Message}", ex);
        }
    }

    private List<CommodityPrice> ParseConabData(string json)
    {
        var prices = new List<CommodityPrice>();

        // Estrutura de parse para normalizar os dados da CONAB no padrão do Vanguard
        // (Será ajustada conforme o schema exato do endpoint da CONAB)

        return prices;
    }
}
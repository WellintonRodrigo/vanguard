using Vanguard.DataCollector.Collectors.Interfaces;
using Vanguard.Domain.Entities;

namespace Vanguard.DataCollector.Collectors
{
    public class CepeaCommodityCollector : ICommodityCollector
    {
        public Task<IReadOnlyCollection<CommodityPrice>> CollectAsync(CancellationToken cancellationToken = default)
        {
            IReadOnlyCollection<CommodityPrice> prices = 
                [
                new CommodityPrice{
                    Source = "CEPEA",
                    Commodity = "Soja",
                    Unit = "seca 60kg",
                    PriceBrl = 150.00m,
                    PriceUsd = 30.00m,
                    DailyVariationPercent = 0.5m,
                    MonthlyVariationPercent = 2.0m,
                    ReferenceDate = DateTime.UtcNow.Date,
                }
              ];
            return Task.FromResult(prices);
        }
    }
}

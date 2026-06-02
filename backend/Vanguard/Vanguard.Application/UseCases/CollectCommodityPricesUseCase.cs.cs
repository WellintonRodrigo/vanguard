using Vanguard.DataCollector.Collectors.Interfaces;
using Vanguard.Domain.Entities;
using Vanguard.Domain.Interfaces;

namespace Vanguard.Application.UseCases
{
    public class CollectCommodityPricesUseCase
    {
        private readonly IEnumerable<ICommodityCollector> _collectors;
        private readonly ICommodityPriceRepository _repository;

        public CollectCommodityPricesUseCase(
            IEnumerable<ICommodityCollector>
            collectors, ICommodityPriceRepository repository)
        {
            _collectors = collectors;
            _repository = repository;
        }

        public async Task<int> ExecuteAsync(CancellationToken cancellationToken = default)
        {
            var allPrices = new List<CommodityPrice>();

            foreach (var collector in _collectors)
            {
                var prices = await collector.CollectAsync(cancellationToken);
                allPrices.AddRange(prices);
            }

            if (!allPrices.Any())
                return 0;

            await _repository.InsertManyAsync(allPrices, cancellationToken);

            return allPrices.Count;

        }
}
    }
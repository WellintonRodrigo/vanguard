using Vanguard.DataCollector.Collectors.Interfaces;
using Vanguard.DataCollector.Models;
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
                Console.WriteLine($"Executando collector: {collector.GetType().Name}");

                var prices = await collector.CollectAsync(cancellationToken);
                allPrices.AddRange(prices);
            }

            var newPrices = new List<CommodityPrice>();
                

            foreach (var Price in allPrices) 
            {
                var alreadyExists = await _repository.ExistsAsnyc(
                   Price.Commodity,
                   Price.ReferenceDate,
                   Price.Source,
                   cancellationToken);

                if (!alreadyExists)
                    newPrices.Add(Price);
            }


            if (!newPrices.Any())
                return 0;

            await _repository.InsertManyAsync(newPrices, cancellationToken);

            return newPrices.Count;

        }
}
    }
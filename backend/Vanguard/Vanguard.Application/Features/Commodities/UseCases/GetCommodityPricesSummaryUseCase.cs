using Vanguard.Application.Features.Commodities.DTOs;
using Vanguard.Domain.Interfaces;

namespace Vanguard.Application.Features.Commodities.UseCases
{
    public class GetCommodityPricesSummaryUseCase
    {
        private readonly ICommodityPriceRepository _repository;

        public GetCommodityPricesSummaryUseCase(
            ICommodityPriceRepository repository)
        {
            _repository = repository;
        }

        public async Task<CommodityPricesSummaryDto> ExecuteAsync(
         CancellationToken cancellationToken = default)
        {
            var commodities = await _repository.GetCommoditiesAsync(cancellationToken);
            var sources = await _repository.GetSourcesAsync(cancellationToken);
            var totalRecords = await _repository.CountAsync(cancellationToken);
            var lastCollection = await _repository.GetLastCollectionDateAsync(cancellationToken);

            return new CommodityPricesSummaryDto
            {
                TotalRecords = (int)totalRecords,
                Commodities = commodities,
                TotalCommodities = commodities.Count,
                Sources = sources,
                TotalSources = sources.Count,
                LastCollection = lastCollection
            };
        }
    }
}

using Vanguard.Domain.Entities;
using Vanguard.Domain.Interfaces;

namespace Vanguard.Application.Features.Commodities.UseCases
{
    public class GetAllCommodityPricesUseCase
    {
        private readonly ICommodityPriceRepository _repository;

        public GetAllCommodityPricesUseCase(ICommodityPriceRepository repository)
        {
            _repository = repository;
        }

        public async Task<IReadOnlyCollection<CommodityPrice>> ExecuteAsync(
       CancellationToken cancellationToken = default)
        {
            return await _repository.GetAllAsync(cancellationToken);
        }
    }
}

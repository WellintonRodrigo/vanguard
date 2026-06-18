using Vanguard.Domain.Interfaces;

namespace Vanguard.Application.Features.Commodities.UseCases
{
    public class GetCommoditiesUseCase
    {
        private readonly ICommodityPriceRepository _repository;

        public GetCommoditiesUseCase(ICommodityPriceRepository repository)
        {
            _repository = repository;
        }

        public async Task<List<string>> ExecuteAsync(
       CancellationToken cancellationToken = default)
        {
            return await _repository.GetCommoditiesAsync(cancellationToken);
        }
    }
}

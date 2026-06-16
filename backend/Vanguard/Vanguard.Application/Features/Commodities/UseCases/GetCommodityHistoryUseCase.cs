using Vanguard.Domain.Entities;
using Vanguard.Domain.Interfaces;

namespace Vanguard.Application.Features.Commodities.UseCases
{
    public class GetCommodityHistoryUseCase
    {
        private readonly ICommodityPriceRepository _repositor;

        public GetCommodityHistoryUseCase(ICommodityPriceRepository repositor)
        {
            _repositor = repositor;
        }

        public async Task<IReadOnlyCollection<CommodityPrice>> ExecuteAsync(
        string commodity,
        int days = 30,
        CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrWhiteSpace(commodity))
                return [];

            if(days <= 0)
                days = 30;

            return await _repositor.GetHistoryAsync(commodity, days, cancellationToken);
        }
    }
}

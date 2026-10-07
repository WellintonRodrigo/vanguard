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
        string? commodity=null,
        int days = 30,
        CancellationToken cancellationToken = default)
        {
                if(days <= 0)
                days = 30;

            var targetCommodity = string.IsNullOrWhiteSpace(commodity) ? null : commodity.Trim();

            return await _repositor.GetHistoryAsync(targetCommodity, days, cancellationToken);
        }
    }
}

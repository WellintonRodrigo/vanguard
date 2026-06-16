using Vanguard.Application.Features.Commodities.DTOs;
using Vanguard.Application.Features.Commodities.Services;
using Vanguard.Domain.Interfaces;

namespace Vanguard.Application.Features.Commodities.UseCases
{
    public class GetLatestCommodityPriceUseCase
    {
        private readonly ICommodityPriceRepository _repository;
        private readonly DataFreshnessService _freshnessService;

        public GetLatestCommodityPriceUseCase(
       ICommodityPriceRepository repository,
       DataFreshnessService freshnessService)
        {
            _repository = repository;
            _freshnessService = freshnessService;
        }

        public async Task<CommodityPriceLatestDto?> ExecuteAsync(
        string? commodity = null,
        CancellationToken cancellationToken = default)
        {
            var price = string.IsNullOrWhiteSpace(commodity)
                ?await _repository.GetLatestAsync(cancellationToken)
                :await _repository.GetLatestByCommodityAsync(commodity, cancellationToken);

            if(price is null)
                return null;

            return new CommodityPriceLatestDto
            {
                Commodity = price.Commodity,
                Source = price.Source,
                Market = price.Market,
                Unit = price.Unit,
                PriceBuy = price.PriceBrl,
                PriceSell = price.PriceBrl,
                DailyVariationPercent = price.DailyVariationPercent,
                MonthlyVariationPercent = price.MonthlyVariationPercent,
                ReferenceDate = price.ReferenceDate,
                CollectedAt = price.CollectedAt,
                Freshness = _freshnessService.Calculate(price.ReferenceDate)
            };
        }
    }
}

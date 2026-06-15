namespace Vanguard.Application.Features.Commodities.DTOs
{
    public class CommodityPriceLatestDto
    {
        public string Commodity { get; set; } = string.Empty;
        public string Source { get; set; } = string.Empty;
        public string Market { get; set; } = string.Empty;
        public string Unit { get; set; } = string.Empty;

        public decimal PriceBuy { get; set; }
        public decimal? PriceSell { get; set; }

        public decimal? DailyVariationPercent { get; set; }
        public decimal? MonthlyVariationPercent { get; set; }

        public DateTime ReferenceDate { get; set; }
        public DateTime CollectedAt { get; set; }

        public DataFreshnessDto Freshness { get; set; } = new();
    }
}

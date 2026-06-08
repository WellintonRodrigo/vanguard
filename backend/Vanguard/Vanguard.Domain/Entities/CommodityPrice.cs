namespace Vanguard.Domain.Entities
{
    public class CommodityPrice
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();

        public string Source { get; set; } = string.Empty;

        public string Commodity { get; set; } = string.Empty;

        public string Unit { get; set; } = string.Empty;

        public decimal PriceBrl { get; set; }

        public decimal? PriceUsd { get; set; }

        public decimal? DailyVariationPercent { get; set; }

        public decimal? MonthlyVariationPercent { get; set; }

        public DateTime ReferenceDate { get; set; }

        public DateTime CollectedAt { get; set; } = DateTime.UtcNow;
    }
}

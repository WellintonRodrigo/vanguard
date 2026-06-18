namespace Vanguard.Application.Features.Commodities.DTOs
{
    public class CommodityPricesSummaryDto
    {
        public int TotalRecords { get; set; }

        public int TotalCommodities { get; set; }

        public List<string> Commodities { get; set; } = [];

        public int TotalSources { get; set; }

        public List<string> Sources { get; set; } = [];

        public DateTime? LastCollection { get; set; }
    }
}

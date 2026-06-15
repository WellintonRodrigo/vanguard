using Vanguard.Domain.Enums;

namespace Vanguard.Application.Features.Commodities.DTOs
{
    public class DataFreshnessDto
    {
        public DataFreshnessStatus Status { get; set; }
        public int DaysWithoutUpdate { get; set; }
        public string Message { get; set; } = string.Empty;
    }
}

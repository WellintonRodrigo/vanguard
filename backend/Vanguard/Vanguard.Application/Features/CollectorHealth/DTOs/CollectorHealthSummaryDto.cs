namespace Vanguard.Application.Features.CollectorHealth.DTOs
{
    public class CollectorHealthSummaryDto
    {
        public string Source { get; set; } = string.Empty;

        public int TotalChecks { get; set; }

        public int HealthyChecks { get; set; }

        public int WarningChecks { get; set; }

        public int CriticalChecks { get; set; }

        public decimal UptimePercent { get; set; }

        public double AverageResponseTimeMs { get; set; }
    }
}

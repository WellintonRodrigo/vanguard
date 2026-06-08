using Vanguard.DataCollector.Health.Enuns;

namespace Vanguard.DataCollector.Models
{
    public class CollectorHealthResult
    {
        public string Source { get; set; } = string.Empty;

        public string Commodity { get; set; } = string.Empty;

        public string Url { get; set; } = string.Empty;

        public CollectorHealthStatus Status { get; set; }

        public int? HttpStatusCode { get; set; }

        public int RecordsFound { get; set; }

        public long ResponseTimeMs { get; set; }

        public string? ErrorMessage { get; set; }

        public DateTime CheckedAt { get; set; } = DateTime.UtcNow;
    }
}

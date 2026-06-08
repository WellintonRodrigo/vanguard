namespace Vanguard.Domain.Entities
{
    public class CollectorHealthLog
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();

        public string Source { get; set; } = string.Empty;

        public string Commodity { get; set; } = string.Empty;

        public string Status { get; set; } = string.Empty;

        public int? HttpStatusCode { get; set; }

        public int RecordsFound { get; set; }

        public long ResponseTimeMs { get; set; }

        public string? ErrorMessage { get; set; }

        public DateTime CheckedAt { get; set; }
    }
}

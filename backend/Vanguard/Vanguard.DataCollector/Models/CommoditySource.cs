using MongoDB.Bson;
using Vanguard.DataCollector.Health.Enuns;

namespace Vanguard.DataCollector.Models
{
    public class CommoditySource
    {
        public string Id { get; set; } = ObjectId.GenerateNewId().ToString();

        public string SourceKey { get; set; } = string.Empty;

        public string Name { get; set; } = string.Empty;

        public string Commodity { get; set; } = string.Empty;

        public string Url { get; set; } = string.Empty;

        public string Unit { get; set; } = string.Empty;

        public bool IsEnabled { get; set; } = true;

        public CollectorHealthStatus HealthStatus { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public DateTime? LastExecutionAt { get; set; }

        public DateTime? LastSuccessAt { get; set; }
    }
}

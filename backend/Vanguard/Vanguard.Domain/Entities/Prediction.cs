using Vanguard.Domain.Enums;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;


namespace Vanguard.Domain.Entities
{
    public class Prediction
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();

        public string TargetProduct { get; set; } = string.Empty;

        public decimal ImpactProbability { get; set; }
        [BsonRepresentation(BsonType.String)]
        public Trend Trend { get; set; }

        public decimal EstimatedVariationPercent { get; set; }

        public string Insight { get; set; } = string.Empty;

        public DateTime PredictionDate { get; set; }
    }
}

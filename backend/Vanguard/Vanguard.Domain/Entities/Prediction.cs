using System;
using System.Collections.Generic;
using System.Text;
using Vanguard.Domain.Enums;

namespace Vanguard.Domain.Entities
{
    public class Prediction
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();

        public string TargetProduct { get; set; } = string.Empty;

        public decimal ImpactProbability { get; set; }

        public Trend Trend { get; set; }

        public decimal EstimatedVariationPercent { get; set; }

        public string Insight { get; set; } = string.Empty;

        public DateTime PredictionDate { get; set; }
    }
}

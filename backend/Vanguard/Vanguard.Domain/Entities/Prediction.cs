using System;
using System.Collections.Generic;
using System.Text;

namespace Vanguard.Domain.Entities
{
    internal class Prediction
    {
        public Guid Id { get; set; }

        public string TargetProduct { get; set; } = string.Empty;

        public decimal ImpactProbability { get; set; }

        public string Trend { get; set; } = string.Empty;

        public decimal EstimatedVariationPercent { get; set; }

        public string Insight { get; set; } = string.Empty;

        public DateTime PredictionDate { get; set; }
    }
}

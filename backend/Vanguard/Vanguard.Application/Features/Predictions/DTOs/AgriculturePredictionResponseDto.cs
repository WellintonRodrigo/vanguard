using System;
using System.Collections.Generic;
using System.Text;

namespace Vanguard.Application.Features.Predictions.DTOs
{
    public class AgriculturePredictionResponseDto
    {
        public string Product { get; set; } = string.Empty;

        public string Location { get; set; } = string.Empty;

        public decimal ImpactProbability { get; set; }

        public string Trend { get; set; } = string.Empty;

        public decimal EstimatedVariationPercent { get; set; }

        public string Insight { get; set; } = string.Empty;

        public DateTime PredictionDate { get; set; }
    }
}

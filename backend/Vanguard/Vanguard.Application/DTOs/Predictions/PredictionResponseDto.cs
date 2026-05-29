using System;
using System.Collections.Generic;
using System.Text;

namespace Vanguard.Application.DTOs.Predictions
{
    public class PredictionResponseDto
    {
        public string Product { get; set; } = string.Empty;
        public double Probability { get; set; }

        public double Confidence { get; set; }

        public string Trend { get; set; } = string.Empty;

        public DateTime AnalyzedAt { get; set; }
    }
}

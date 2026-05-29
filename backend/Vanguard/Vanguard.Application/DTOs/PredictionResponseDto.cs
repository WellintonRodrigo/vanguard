using System;
using System.Collections.Generic;
using System.Text;

namespace Vanguard.Application.DTOs
{
    public class PredictionResponseDto
    {
        public double Probability { get; set; }

        public double Confidence { get; set; }

        public string Trend { get; set; } = string.Empty;

        public DateTime AnalyzedAt { get; set; }
    }
}

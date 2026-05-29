using System;
using System.Collections.Generic;
using System.Text;
using Vanguard.Application.DTOs.Predictions;

namespace Vanguard.Application.Features.Predictions.Services
{
    public class PredictionService
    {
        public PredictionResponseDto Analyze (string question)
        {
            return new PredictionResponseDto
            {
                Probability = 0.75,
                Confidence = 0.85,
                Trend = "Upward",
                AnalyzedAt = DateTime.UtcNow
            };
        }
    }
}

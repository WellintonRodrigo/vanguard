using System;
using System.Collections.Generic;
using System.Text;
using Vanguard.Application.DTOs.Predictions;
using Vanguard.Domain.Entities;
using Vanguard.Domain.Enums;
using Vanguard.Domain.Interfaces;

namespace Vanguard.Application.Features.Predictions.Services
{
    public class PredictionService
    {
        private readonly IPredictionRepository _predictionRepository;

        public PredictionService(IPredictionRepository predictionRepository)
        {
            _predictionRepository = predictionRepository;
        }


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
        public async Task SalvarTesteAsync()
        {
            var predicition = new Prediction
            {
                Id = Guid.NewGuid().ToString(),
                TargetProduct = "Tomate",
                ImpactProbability = 75,
                Trend = Trend.up,
                EstimatedVariationPercent = 12,
                Insight = "Chuvas intensas podem impactar a oferta",
                PredictionDate = DateTime.UtcNow
            };
            await _predictionRepository.CreateAsync(predicition);
        }
    }
}

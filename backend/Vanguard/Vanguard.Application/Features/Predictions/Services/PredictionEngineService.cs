using Vanguard.Application.Features.Insights;
using Vanguard.Application.Features.Predictions.DTOs;
using Vanguard.Application.Interfaces;
using Vanguard.Domain.Entities;
using Vanguard.Domain.Interfaces;
using Vanguard.Domain.Enums;

namespace Vanguard.Application.Features.Predictions.Services
{
    public class PredictionEngineService
    {
        private readonly IWeatherProvider _weatherProvider;
        private readonly InsightTemplateService _insightTemplateService;
        private readonly IPredictionRepository _predictionRepository;
        public PredictionEngineService(
            IWeatherProvider weatherProvider,
            InsightTemplateService insightTemplateService,
            IPredictionRepository predictionRepository
            )
        {
            _weatherProvider = weatherProvider; 
            _insightTemplateService = insightTemplateService;
            _predictionRepository = predictionRepository;
        }

        public async Task<AgriculturePredictionResponseDto> AnalyzeAgricultureAsync(
            AgriculturePredictionRequestDto requestDto)
        {
            //BUSCAR CLIMA

            var weather = await _weatherProvider.GetWeatherAsync(
                latitude: requestDto.Latitude,
                longitude: requestDto.Longitude,
                location: requestDto.Location);
           
            //CALCULAR HEURÍSTICA
            var result = CalculateClimateImpact(weather.Rain);
           
            //GERAR INSIGHTS
            var insight = _insightTemplateService.Generate(
            result.InsightKey,
            requestDto.Product,
            requestDto.Location);
           
            //CRIAR PREDICTION
            var prediction = new Prediction
            {
                TargetProduct = requestDto.Product,
                ImpactProbability = result.Probability,
                Trend = result.Trend,
                EstimatedVariationPercent = result.EstimatedVariationPercent,
                Insight = insight,
                PredictionDate = DateTime.UtcNow,
            };

            await _predictionRepository.CreateAsync(prediction);

            return new AgriculturePredictionResponseDto
            {
                Product = requestDto.Product,
                Location = requestDto.Location,
                ImpactProbability = result.Probability,
                Trend = result.Trend.ToString(),
                EstimatedVariationPercent = result.EstimatedVariationPercent,
                Insight = insight,
                PredictionDate = DateTime.UtcNow

            };
        }

        public async Task<AgriculturePredictionResponseDto> AnalyzaAgricultureForecastAsync(
            AgriculturePredictionRequestDto requestDto)
        {
            var forecast = await _weatherProvider.GetWeatherForecastAsync(
                latitude: requestDto.Latitude,
                longitude: requestDto.Longitude,
                location: requestDto.Location);

            var totalRain = forecast.DailyForecasts
                .Sum(day => day.PrecipitationSum);

            var criticalRainDays =forecast.DailyForecasts
                .Count(day => day.PrecipitationSum >= 20);

            var result = CalculateForecastImpact(
                totalRain,
                criticalRainDays);

            var insight = _insightTemplateService.Generate(
                result.InsightKey,
                requestDto.Product,
                requestDto.Location);

            var prediction = new Prediction
            {
                TargetProduct = requestDto.Product,
                ImpactProbability = result.Probability,
                Trend = result.Trend,
                EstimatedVariationPercent = result.EstimatedVariationPercent,
                Insight = insight,
                PredictionDate = DateTime.UtcNow,
            };

            await _predictionRepository.CreateAsync(prediction);

            return new AgriculturePredictionResponseDto
            {
                Product = requestDto.Product,
                Location = requestDto.Location,
                ImpactProbability = result.Probability,
                Trend = result.Trend.ToString(),
                EstimatedVariationPercent = result.EstimatedVariationPercent,
                Insight = insight,
                PredictionDate = DateTime.UtcNow
            };
                

        }

        private static ClimateImpactResult CalculateForecastImpact(
            decimal totalRain,
            int criticalRainDays)
        {
            if ((totalRain >= 60 || criticalRainDays >= 2))
            {
                return new ClimateImpactResult(
                    InsightKey: "heavyRainForecast",
                    Probability:88,
                    EstimatedVariationPercent: 10,
                    Trend: Trend.up);
            }

            if(totalRain >= 10)
            {
                return new ClimateImpactResult(
                    InsightKey: "frecastModerateRain",
                    Probability: 70,
                    EstimatedVariationPercent: 6,
                    Trend: Trend.up);
            }

            if(totalRain < 10)
            {
                return new ClimateImpactResult(
                    InsightKey: "forecastLowRain",
                    Probability: 32,
                    EstimatedVariationPercent: 1,
                    Trend: Trend.stable);
            }

            return new ClimateImpactResult(
                InsightKey: "forecastStable",
                Probability: 40,
                EstimatedVariationPercent: 2,
                Trend: Trend.stable);
        }

        private static ClimateImpactResult CalculateClimateImpact(decimal rain)
        {
            if (rain >= 30)
            {
                return new ClimateImpactResult(
                    InsightKey: "heavyRain",
                    Probability: 85,
                    EstimatedVariationPercent: 8,
                   Trend: Trend.up);
            }

            if (rain >= 10)
            {
                return new ClimateImpactResult(
                    InsightKey: "moderateRain",
                    Probability: 65,
                    EstimatedVariationPercent: 5,
                   Trend: Trend.up);
            }

            if (rain < 5)
            {
                return new ClimateImpactResult(
                    InsightKey: "lowRain",
                    Probability: 30,
                    EstimatedVariationPercent: 1,
                   Trend: Trend.stable);
            }

            return new ClimateImpactResult(
                InsightKey: "stable",
                Probability: 35,
                EstimatedVariationPercent: 0,
               Trend: Trend.stable);
        }

        private sealed record ClimateImpactResult(
            string InsightKey,
            decimal Probability,
            decimal EstimatedVariationPercent,
            Trend Trend);
    
    }
}

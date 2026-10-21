using Vanguard.Application.Features.Insights;
using Vanguard.Application.Features.Predictions.DTOs;
using Vanguard.Application.Interfaces;
using Vanguard.Domain.Entities;
using Vanguard.Domain.Enums;
using Vanguard.Domain.Interfaces;

namespace Vanguard.Application.Features.Predictions.Services
{
    public class PredictionEngineService
    {
        private readonly IWeatherProvider _weatherProvider;
        private readonly InsightTemplateService _insightTemplateService;
        private readonly IPredictionRepository _predictionRepository;
        private readonly ICommodityPriceRepository _commodityPriceRepository;
        public PredictionEngineService(
            IWeatherProvider weatherProvider,
            InsightTemplateService insightTemplateService,
            IPredictionRepository predictionRepository,
            ICommodityPriceRepository commodityPriceRepository
            )
        {
            _weatherProvider = weatherProvider; 
            _insightTemplateService = insightTemplateService;
            _predictionRepository = predictionRepository;
            _commodityPriceRepository = commodityPriceRepository;
        }

        public async Task<AgriculturePredictionResponseDto> AnalyzeAgricultureAsync(
            AgriculturePredictionRequestDto requestDto)
        {
            //BUSCAR CLIMA ATUAL
            var weather = await _weatherProvider.GetWeatherAsync(
                latitude: requestDto.Latitude,
                longitude: requestDto.Longitude,
                location: requestDto.Location);

            //BUSCAR COTAÇÃO DE COMMODITY
            var latestPrice = await _commodityPriceRepository.GetLatestByCommodityAsync(
                requestDto.Product);
            decimal priceVariation = latestPrice?.DailyVariationPercent ?? 0m;

            //CALCULAR HEURÍSTICA
            var result = CalculateClimateImpact(weather.Rain, priceVariation);
           
            //GERAR INSIGHTS
            var insight = _insightTemplateService.Generate(
                result.InsightKey,
                requestDto.Product,
                requestDto.Location);
           
            //CRIAR PREDICTION
            var prediction = new Prediction
            {
                TargetProduct = requestDto.Product,
                ImpactProbability = (decimal)result.Probability,
                Trend = result.Trend,
                EstimatedVariationPercent = (decimal)result.EstimatedVariationPercent,
                Insight = insight,
                PredictionDate = DateTime.UtcNow,
            };

            await _predictionRepository.CreateAsync(prediction);

            return new AgriculturePredictionResponseDto
            {
                Product = requestDto.Product,
                Location = requestDto.Location,
                ImpactProbability = (double)result.Probability,
                Trend = result.Trend.ToString(),
                EstimatedVariationPercent = (double)result.EstimatedVariationPercent,
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

            var criticalRainDays = forecast.DailyForecasts
                .Count(day => day.PrecipitationSum >= 20);

            var result = CalculateForecastImpact(
                (double)totalRain,
                criticalRainDays);

            var insight = _insightTemplateService.Generate(
                result.InsightKey,
                requestDto.Product,
                requestDto.Location);

            var prediction = new Prediction
            {
                TargetProduct = requestDto.Product,
                ImpactProbability = (decimal)result.Probability,
                Trend = result.Trend,
                EstimatedVariationPercent = (decimal)result.EstimatedVariationPercent,
                Insight = insight,
                PredictionDate = DateTime.UtcNow,
            };

            await _predictionRepository.CreateAsync(prediction);

            return new AgriculturePredictionResponseDto
            {
                Product = requestDto.Product,
                Location = requestDto.Location,
                ImpactProbability = (double)result.Probability,
                Trend = result.Trend.ToString(),
                EstimatedVariationPercent = (double)result.EstimatedVariationPercent,
                Insight = insight,
                PredictionDate = DateTime.UtcNow
            };
        }

        private static ClimateImpactResult CalculateForecastImpact(
            double rainMm,
            int criticalRainDays,
            double priceVariation = 0.0)
        {
            double variation = Math.Abs(priceVariation);

            if (rainMm >= 60.0 || criticalRainDays >= 2)
            {
                return new ClimateImpactResult("forecastHeavyRain", 88.0, 10.0, Trend.up);
            }

            if (rainMm < 5.0)
            {
                return new ClimateImpactResult("droughtRisk", 70.0, 5.0, Trend.up);
            }

            if (rainMm >= 10.0)
            {
                return new ClimateImpactResult("forecastModerateRain", 60.0, 4.0, Trend.up);
            }

            return new ClimateImpactResult("forecastStable", 35.0, 0.0, Trend.stable);
        }

        private static ClimateImpactResult CalculateClimateImpact(decimal rain, decimal priceVariation = 0m)
        {
            double rainMm = (double)rain;
            double variation = Math.Abs((double)priceVariation);

            // Cenário 1: Chuva Extrema + Volatilidade no Preço
            if (rainMm >= 30.0 && variation >= 2.0)
            {
                return new ClimateImpactResult("extremeCombinedRisk", 90.0, 10.0, Trend.up);
            }

            // Seca severa
            if (rainMm < 2.0)
            {
                return new ClimateImpactResult("droughtRisk", 75.0, 6.0, Trend.up);
            }

            // Volatilidade isolada no mercado
            if (variation >= 3.0)
            {
                return new ClimateImpactResult("marketVolatility", 65.0, 5.0, Trend.up);
            }

            // Chuva moderada
            if (rainMm >= 10.0)
            {
                return new ClimateImpactResult("forecastModerateRain", 50.0, 3.0, Trend.up);
            }

            // Chuva alta
            if (rainMm >= 30.0)
            {
                return new ClimateImpactResult("heavyRain", 85.0, 8.0, Trend.up);
            }

            // Baixa chuva
            if (rainMm < 5.0)
            {
                return new ClimateImpactResult("forecastLowRain", 30.0, 1.0, Trend.stable);
            }

            return new ClimateImpactResult("forecastStable", 35.0, 0.0, Trend.stable);
        }

        // Tornado público para evitar erro de acessibilidade se usado fora desta classe
        public sealed record ClimateImpactResult(
            string InsightKey,
            double Probability,
            double EstimatedVariationPercent,
            Trend Trend);
    }
}

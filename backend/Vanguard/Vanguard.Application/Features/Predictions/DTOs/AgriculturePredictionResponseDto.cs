using Vanguard.Application.Features.Weather.DTOs;

namespace Vanguard.Application.Features.Predictions.DTOs
{
    public class AgriculturePredictionResponseDto
    {
        public string Product { get; set; } = string.Empty;

        public string Location { get; set; } = string.Empty;

        public double ImpactProbability { get; set; }

        public string Trend { get; set; } = string.Empty;

        public double EstimatedVariationPercent { get; set; }

        public string Insight { get; set; } = string.Empty;

        public DateTime PredictionDate { get; set; }
        public decimal TotalRainAccumulated { get;  set; }
        public int CriticalDaysCount { get;  set; }
        public List<DailyWeatherDto> DailyForecasts { get; set; } =new();
    }
}

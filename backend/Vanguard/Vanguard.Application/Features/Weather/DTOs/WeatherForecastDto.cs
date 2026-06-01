using System;
using System.Collections.Generic;
using System.Text;

namespace Vanguard.Application.Features.Weather.DTOs
{
    public class WeatherForecastDto
    {
        public string Location { get; set; } = string.Empty;

        public List<DailyWeatherDto> DailyForecasts { get; set; } = [];
    }
}

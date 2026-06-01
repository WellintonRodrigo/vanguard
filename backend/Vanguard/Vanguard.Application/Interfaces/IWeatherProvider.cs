using System;
using System.Collections.Generic;
using System.Text;
using Vanguard.Application.Features.Weather.DTOs;

namespace Vanguard.Application.Interfaces
{
    public interface IWeatherProvider
    {
        Task<WeatherDto> GetWeatherAsync(
            double latitude,
            double longitude,
            string location);

        Task<WeatherForecastDto> GetWeatherForecastAsync(
            double latitude,
            double longitude,
            string location,
            int forecastDays = 7);

        
    }
}

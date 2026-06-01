using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using Vanguard.Application.Features.Weather.DTOs;
using Vanguard.Application.Interfaces;


namespace Vanguard.Infrastructure.ExternalService.OpenMeteo
{
    public class OpenMeteoWeatherProvider : IWeatherProvider
    {
        private readonly HttpClient _httpClient;

        public OpenMeteoWeatherProvider(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<WeatherDto> GetWeatherAsync(double latitude, double longitude, string location)
        {
            var url =
            $"forecast?latitude={latitude.ToString(CultureInfo.InvariantCulture)}" +
            $"&longitude={longitude.ToString(CultureInfo.InvariantCulture)}" +
            $"&current=temperature_2m,rain";

            var response = await _httpClient.GetAsync(url) ;
            
            //response.EnsureSuccessStatusCode();

            var json = await response.Content.ReadAsStringAsync();

            var result = JsonSerializer.Deserialize<OpenMeteoResponseDto>(json,
                new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

            if (result?.Current is null)
                throw new InvalidOperationException($"Não foi possível mapear os dados climáticos da Open-Meteo. JSON recebido: {json}");
            
            return new WeatherDto
            {
                Location = location,
                Temperature = result.Current.Temperature2m,
                Rain = result.Current.Rain,
                Date = result.Current.Time
            };

        }

        public async Task<WeatherForecastDto> GetWeatherForecastAsync(double latitude, double longitude, string location, int forecastDays = 7)
        {
            var url =
        $"forecast?latitude={latitude.ToString(CultureInfo.InvariantCulture)}" +
        $"&longitude={longitude.ToString(CultureInfo.InvariantCulture)}" +
        $"&daily=precipitation_sum,temperature_2m_max,temperature_2m_min" +
        $"&forecast_days={forecastDays}" +
        $"&timezone=America%2FSao_Paulo";

            var response = await _httpClient.GetAsync(url);

            var json = await response.Content.ReadAsStringAsync();

            if(!response.IsSuccessStatusCode)
                throw new InvalidOperationException($"Erro ao obter previsão do tempo da Open-Meteo. Status Code: {response.StatusCode}, JSON recebido: {json}");

            var result = JsonSerializer.Deserialize<OpenMeteoForecastResponseDto>(json,
                new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

            if(result?.Daily is null)
                throw new InvalidOperationException($"Não foi possível mapear os dados climáticos da Open-Meteo. JSON recebido: {json}");

            var dailyForecasts = result.Daily.Time
                .Select ((Date, index) => new DailyWeatherDto
                {
                    Date = Date,
                    PrecipitationSum = result.Daily.PrecipitationSum[index],
                    TemperatureMax = result.Daily.TemperatureMax[index],
                    TemperatureMin = result.Daily.TemperatureMin[index]
                })
                .ToList();

            return new WeatherForecastDto
            {
                Location = location,
                DailyForecasts = dailyForecasts

            };
        }

        public class OpenMeteoForecastResponseDto
        {
            [JsonPropertyName("daily")]
            public OpenMeteoDailyDto? Daily { get; set; }
        }

        public class OpenMeteoDailyDto
        {
            [JsonPropertyName("time")]
            public List<DateTime> Time { get; set; } = [];

            [JsonPropertyName("precipitation_sum")]
            public List<decimal> PrecipitationSum { get; set; } = [];

            [JsonPropertyName("temperature_2m_max")]
            public List<decimal> TemperatureMax { get; set; } = [];

            [JsonPropertyName("temperature_2m_min")]
            public List<decimal> TemperatureMin { get; set; } = [];
        }
    }
}

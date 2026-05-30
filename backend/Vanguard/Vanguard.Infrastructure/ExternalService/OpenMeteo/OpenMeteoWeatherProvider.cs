using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Text.Json;
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
    }
}

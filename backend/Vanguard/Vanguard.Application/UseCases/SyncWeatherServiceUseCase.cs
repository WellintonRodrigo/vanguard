using System;
using Vanguard.Application.Interfaces;
using Vanguard.Domain.Interfaces;

namespace Vanguard.Application.UseCases
{
    internal class SyncWeatherServiceUseCase
    {
        private readonly IWeatherProvider _weatherProvider;
        private readonly IWeatherRepository _weatherRepository;

        public SyncWeatherServiceUseCase(IWeatherProvider weatherProvider, IWeatherRepository weatherRepository)
        {
            _weatherProvider = weatherProvider;
            _weatherRepository = weatherRepository;
        }

        public async Task ExecuteAsync(string location, double latitude, double longitude)
        {
            var weatherDto = await _weatherProvider.GetWeatherAsync(latitude, longitude, location);
            var weatherLog = new Domain.Entities.WeatherLog
            {
                Id = Guid.NewGuid(),
                Location = location,
                Date = DateTime.UtcNow,
                PrecipitationMm = weatherDto.Rain,
                TemperatureMax = weatherDto.Temperature,
                TemperatureMin = weatherDto.Temperature,
                ExtremeEvent = weatherDto.Rain>50
            };
            await _weatherRepository.CreateAsync(weatherLog);
        }
    }
}

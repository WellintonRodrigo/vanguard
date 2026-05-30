using System;
using System.Collections.Generic;
using System.Text;
using Vanguard.Domain.Entities;
using Vanguard.Domain.Interfaces;

namespace Vanguard.Infrastructure.Persistence.Repositories
{
    public class WeatherRepository : IWeatherRepository
    {
        Task IWeatherRepository.CreateAsync(WeatherLogcs weatherLog)
        {
            throw new NotImplementedException();
        }

        Task<List<WeatherLogcs>> IWeatherRepository.GetByDateRangeAsync(DateTime startDate, DateTime endDate)
        {
            throw new NotImplementedException();
        }

        Task<List<WeatherLogcs>> IWeatherRepository.GetByLocationAsync(string location)
        {
            throw new NotImplementedException();
        }
    }
}

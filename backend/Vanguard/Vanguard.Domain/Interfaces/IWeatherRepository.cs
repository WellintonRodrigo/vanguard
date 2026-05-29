using System;
using System.Collections.Generic;
using System.Text;
using Vanguard.Domain.Entities;

namespace Vanguard.Domain.Interfaces
{
    public interface IWeatherRepository
    {
        Task CreateAsync(WeatherLogcs weatherLog);

        Task<List<WeatherLogcs>> GetByLocationAsync(
            string location);

        Task<List<WeatherLogcs>> GetByDateRangeAsync(
            DateTime startDate,
            DateTime endDate);
    }
}

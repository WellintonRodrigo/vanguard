using System;
using System.Collections.Generic;
using System.Text;
using Vanguard.Domain.Entities;

namespace Vanguard.Domain.Interfaces
{
    public interface IWeatherRepository
    {
        Task CreateAsync(WeatherLog weatherLog);

        Task<List<WeatherLog>> GetByLocationAsync(
            string location);

        Task<List<WeatherLog>> GetByDateRangeAsync(
            DateTime startDate,
            DateTime endDate);
    }
}

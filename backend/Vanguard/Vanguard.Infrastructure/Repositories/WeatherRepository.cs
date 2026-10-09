using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Text;
using Vanguard.Domain.Entities;
using Vanguard.Domain.Interfaces;

namespace Vanguard.Infrastructure.Repositories
{
    public class WeatherRepository : IWeatherRepository
       
    {
        public readonly IMongoCollection<WeatherLog> _weatherLogsCollection;

        public WeatherRepository(IMongoDatabase database)
        {
            _weatherLogsCollection = database.GetCollection<WeatherLog>("WeatherLogs");
        }
        async Task IWeatherRepository.CreateAsync(WeatherLog weatherLog)
        {
            await _weatherLogsCollection.InsertOneAsync(weatherLog);
        }

       async Task<List<WeatherLog>> IWeatherRepository.GetByDateRangeAsync(DateTime startDate, DateTime endDate)
        {
            return await _weatherLogsCollection
            .Find(x => x.Date >= startDate && x.Date <= endDate)
            .SortByDescending(x => x.Date)
            .ToListAsync();
        }

        async Task<List<WeatherLog>> IWeatherRepository.GetByLocationAsync(string location)
        {
            return await _weatherLogsCollection
                .Find(log => log.Location == location)
                .SortByDescending(x => x.Date)
                .ToListAsync();
        }
    }
}

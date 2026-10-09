using Microsoft.AspNetCore.Mvc;
using Vanguard.Domain.Interfaces;

namespace Vanguard.API.Controllers;

    [ApiController]
[Route("api/weather")]
public class WeatherController : ControllerBase
{
    private readonly IWeatherRepository _weatherRepository;

    public WeatherController(IWeatherRepository weatherRepository)
    {
        _weatherRepository = weatherRepository;
    }

    [HttpGet("history")]
    public async Task<IActionResult> GetHistory([FromQuery] string? location, [FromQuery] int days = 30)
    {
        if (string.IsNullOrWhiteSpace(location))
        {
            var endDate = DateTime.UtcNow;
            var startDate = endDate.AddDays(-days);
            var logs = await _weatherRepository.GetByDateRangeAsync(startDate, endDate);
            return Ok(logs);
        }

        var locationLogs = await _weatherRepository.GetByLocationAsync(location);
        return Ok(locationLogs);

    }
} 

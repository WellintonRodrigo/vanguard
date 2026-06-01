using System;
using System.Collections.Generic;
using System.Text;

namespace Vanguard.Application.Features.Weather.DTOs
{
    public class DailyWeatherDto
    {
        public DateTime Date { get; set; }

        public decimal PrecipitationSum { get; set; }

        public decimal TemperatureMax { get; set; }

        public decimal TemperatureMin { get; set; }


    }
}

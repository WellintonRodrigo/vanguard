using System;
using System.Collections.Generic;
using System.Text;

namespace Vanguard.Application.Features.Weather.DTOs
{
    public class WeatherDto
    {
        public string Location { get; set; } = string.Empty;

        public decimal Temperature { get; set; }

        public decimal Rain { get; set; }

        public DateTime Date { get; set; }
    }
}

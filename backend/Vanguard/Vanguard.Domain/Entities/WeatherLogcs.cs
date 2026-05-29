using System;
using System.Collections.Generic;
using System.Text;

namespace Vanguard.Domain.Entities
{
    public class WeatherLogcs
    {
        public Guid Id { get; set; }

        public string Location { get; set; } = string.Empty;

        public DateTime Date { get; set; }

        public decimal PrecipitationMm { get; set; }

        public decimal TemperatureMin { get; set; }

        public decimal TemperatureMax { get; set; }

        public bool ExtremeEvent { get; set; }
    }
}

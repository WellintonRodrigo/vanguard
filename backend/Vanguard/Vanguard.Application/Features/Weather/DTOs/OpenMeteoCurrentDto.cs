using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace Vanguard.Application.Features.Weather.DTOs
{
    public class OpenMeteoCurrentDto
    {
        [JsonPropertyName("time")]
        public DateTime Time { get; set; }
        
        [JsonPropertyName("temperature_2m")]
        public decimal Temperature2m { get; set; }
        
        [JsonPropertyName("rain")]
        public decimal Rain { get; set; }
    }
}

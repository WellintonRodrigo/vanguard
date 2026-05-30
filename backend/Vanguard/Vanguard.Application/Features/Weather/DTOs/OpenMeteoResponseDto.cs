using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace Vanguard.Application.Features.Weather.DTOs
{
    public class OpenMeteoResponseDto
    {
        [JsonPropertyName("current")]
        public OpenMeteoCurrentDto? Current { get; set; }
    }
}

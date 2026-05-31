using System;
using System.Collections.Generic;
using System.Text;

namespace Vanguard.Application.Features.Predictions.DTOs
{
    public class AgriculturePredictionRequestDto
    {
         public string Product { get; set; } = string.Empty;

    public string Location { get; set; } = string.Empty;

    public double Latitude { get; set; }

    public double Longitude { get; set; }
    }
}

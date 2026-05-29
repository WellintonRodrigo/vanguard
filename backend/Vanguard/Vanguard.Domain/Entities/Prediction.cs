using System;
using System.Collections.Generic;
using System.Text;

namespace Vanguard.Domain.Entities
{
    internal class Prediction
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();

        public string Question { get; set; } = string.Empty;

        public double Probability { get; set; }

        public double Confidence { get; set; }

        public string Trend { get; set; } = string.Empty;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}

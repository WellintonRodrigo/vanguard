using System;
using System.Collections.Generic;
using System.Text;

namespace Vanguard.DataCollector.Models
{
    public class CommoditySource
    {
        public string Name { get; set; } = string.Empty;

        public string Commodity { get; set; } = string.Empty;

        public string Url { get; set; } = string.Empty;

        public string Unit { get; set; } = string.Empty;

        public bool Enabled { get; set; }
    }
}

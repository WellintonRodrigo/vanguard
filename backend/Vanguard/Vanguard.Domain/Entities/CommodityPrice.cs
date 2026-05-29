using System;
using System.Collections.Generic;
using System.Text;

namespace Vanguard.Domain.Entities
{
    public class CommodityPrice
    {
        public Guid Id { get; set; }

        public string Product { get; set; } = string.Empty;

        public string Category { get; set; } = string.Empty;

        public decimal PricePerKg { get; set; }

        public DateTime QuoteDate { get; set; }

        public string Region { get; set; } = string.Empty;
    }
}

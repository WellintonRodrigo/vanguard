using System;
using System.Collections.Generic;
using System.Text;
using Vanguard.Domain.Entities;

namespace Vanguard.DataCollector.Collectors.Interfaces
{
    public interface ICommodityCollector
    {
        Task<IReadOnlyCollection<CommodityPrice>> CollectAsync(
            CancellationToken cancellationToken = default);
    }
}

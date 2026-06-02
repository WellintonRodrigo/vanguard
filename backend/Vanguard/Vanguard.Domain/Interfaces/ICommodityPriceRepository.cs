using System;
using System.Collections.Generic;
using System.Text;
using Vanguard.Domain.Entities;

namespace Vanguard.Domain.Interfaces
{
  public interface ICommodityPriceRepository
    {
        Task InsertManyAsync(
        IReadOnlyCollection<CommodityPrice> prices,
        CancellationToken cancellationToken = default);
    }
}

using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using Vanguard.Domain.Entities;

namespace Vanguard.Domain.Interfaces
{
  public interface ICommodityPriceRepository
    {
        Task InsertManyAsync(
        IReadOnlyCollection<CommodityPrice> prices,
        CancellationToken cancellationToken = default);

        Task<bool> ExistsAsnyc(
            string commodity,
            DateTime referenceDate,
            string source,
            CancellationToken cancellationToken = default);

        // =======================
        // CONSULTAS
        // =======================

        Task<IReadOnlyCollection<CommodityPrice>> GetAllAsync(
        CancellationToken cancellationToken = default);

        Task<CommodityPrice?> GetLatestAsync(
            CancellationToken cancellationToken = default);

        Task<CommodityPrice?> GetLatestByCommodityAsync(
            string commodity,
            CancellationToken cancellationToken = default);

        Task<IReadOnlyCollection<CommodityPrice>> GetHistoryAsync(
            string commodity,
            int days,
            CancellationToken cancellationToken = default);

        Task<List<string>> GetCommoditiesAsync(
           CancellationToken cancellationToken = default);

        Task<List<string>> GetSourcesAsync(
        CancellationToken cancellationToken = default);

        Task<long> CountAsync(
            CancellationToken cancellationToken = default);

        Task<DateTime?> GetLastCollectionDateAsync(
            CancellationToken cancellationToken = default);
    }
}

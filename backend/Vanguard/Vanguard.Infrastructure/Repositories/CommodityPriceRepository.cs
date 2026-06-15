using MongoDB.Driver;
using Vanguard.Domain.Entities;
using Vanguard.Domain.Interfaces;
using Vanguard.Infrastructure.Persistence.Context;

namespace Vanguard.Infrastructure.Repositories
{
    public class CommodityPriceRepository : ICommodityPriceRepository
    {
        private readonly IMongoCollection<CommodityPrice> _collection;

        public CommodityPriceRepository(MongoDbContext context)
        {
            _collection = context.Database.GetCollection<CommodityPrice>("commodity_prices");
        }
        //Esse método verifica se já existe preço da mesma commodity, fonte e data.
        public async Task<bool> ExistsAsnyc(
            string commodity,
            DateTime referenceDate,
            string source, CancellationToken cancellationToken = default)
        {
            var startDate = referenceDate.Date;
            var endDate = referenceDate.Date.AddDays(1);

            var filter = Builders<CommodityPrice>.Filter.And(
                Builders<CommodityPrice>.Filter.Eq(x => x.Commodity, commodity),
                Builders<CommodityPrice>.Filter.Eq(x => x.Source, source),
                Builders<CommodityPrice>.Filter.Gte(x => x.ReferenceDate, startDate),
                Builders<CommodityPrice>.Filter.Lt(x => x.ReferenceDate, endDate)
                );

            return await _collection
                .Find(filter)
                .AnyAsync(cancellationToken);
        }

        public async Task InsertManyAsync(
            IReadOnlyCollection<CommodityPrice> prices, CancellationToken cancellationToken = default)
        {
           await _collection.InsertManyAsync(prices, cancellationToken: cancellationToken);
        }

        public async Task<CommodityPrice?> GetLatestAsync(
             CancellationToken cancellationToken = default)
        {
            return await _collection
                .Find(Builders<CommodityPrice>.Filter.Empty)
                .SortByDescending(x => x.ReferenceDate)
                .ThenByDescending(x => x.CollectedAt)
                .FirstOrDefaultAsync(cancellationToken);
        }

        public async Task<CommodityPrice?> GetLatestByCommodityAsync(
           string commodity, CancellationToken cancellationToken = default)
        {
            var filter = Builders<CommodityPrice>.Filter.Eq(
                x => x.Commodity, commodity);

            return await _collection
                .Find(filter)
                .SortByDescending(x => x.ReferenceDate)
                .ThenByDescending(x => x.CollectedAt)
                .FirstOrDefaultAsync(cancellationToken);
        }

        public async Task<IReadOnlyCollection<CommodityPrice>> GetHistoryAsync(
            string commodity, int days, CancellationToken cancellationToken = default)
        {
            var fromDate = DateTime.UtcNow.Date.AddDays(-days);

            var filter = Builders<CommodityPrice>.Filter.And(
                Builders<CommodityPrice>.Filter.Eq(x => x.Commodity, commodity),
                Builders<CommodityPrice>.Filter.Gte(x => x.ReferenceDate , fromDate)
                );

            return await _collection
                 .Find(filter)
                 .SortByDescending(x => x.ReferenceDate)
                 .ThenByDescending(x => x.CollectedAt)
                 .ToListAsync(cancellationToken);
        }

        public async Task<IReadOnlyCollection<CommodityPrice>> GetAllAsync(
            CancellationToken cancellationToken = default)
        {
            return await _collection
        .Find(Builders<CommodityPrice>.Filter.Empty)
        .SortByDescending(x => x.ReferenceDate)
        .ThenByDescending(x => x.CollectedAt)
        .ToListAsync(cancellationToken);
        }
    }
}

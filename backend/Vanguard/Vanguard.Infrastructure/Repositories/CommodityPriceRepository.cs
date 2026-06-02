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
        public async Task InsertManyAsync(
            IReadOnlyCollection<CommodityPrice> prices, CancellationToken cancellationToken = default)
        {
           await _collection.InsertManyAsync(prices, cancellationToken: cancellationToken);
        }
    }
}

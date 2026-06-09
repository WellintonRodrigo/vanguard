using MongoDB.Driver;
using Vanguard.Domain.Entities;
using Vanguard.Domain.Interfaces;
using Vanguard.Infrastructure.Persistence.Context;

namespace Vanguard.Infrastructure.Repositories
{
    public class CollectorHealthLogRepository : ICollectorHealthLogRepository
    {
        private readonly IMongoCollection<CollectorHealthLog> _collection;

        public CollectorHealthLogRepository(MongoDbContext context)
        {
            _collection = context.Database.GetCollection<CollectorHealthLog>
                ("collector_health_logs");
        }

        public async Task<IReadOnlyCollection<CollectorHealthLog>> GetHistoryAsync(
            CancellationToken cancellationToken = default)
        {
            return await _collection
                .Find(_ => true)
                .SortByDescending(x => x.CheckedAt)
                .Limit(100)
                .ToListAsync(cancellationToken);
        }

        public async Task InsertManyAsync(
          IReadOnlyCollection<CollectorHealthLog> logs,
          CancellationToken cancellationToken = default)
        {
           if(!logs.Any())
                return;

           await _collection.InsertManyAsync(logs, cancellationToken: cancellationToken);
        }
    }
}

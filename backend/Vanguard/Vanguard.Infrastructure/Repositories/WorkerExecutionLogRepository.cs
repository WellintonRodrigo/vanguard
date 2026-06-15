using MongoDB.Driver;
using Vanguard.Domain.Entities;
using Vanguard.Domain.Interfaces;
using Vanguard.Infrastructure.Persistence.Context;

namespace Vanguard.Infrastructure.Repositories
{
    public class WorkerExecutionLogRepository : IWorkerExecutionLogRepository
    {
        private readonly IMongoCollection<WorkerExecutionLog> _collection;

        public WorkerExecutionLogRepository(MongoDbContext context)
        {
            _collection = context.Database.GetCollection<WorkerExecutionLog>(
                "worker_execution_logs");
        }
        public async Task InsertAsync(
            WorkerExecutionLog log, CancellationToken cancellationToken = default)
        {
            await _collection.InsertOneAsync(
                document:log, options:null, cancellationToken);
        }
    }
}

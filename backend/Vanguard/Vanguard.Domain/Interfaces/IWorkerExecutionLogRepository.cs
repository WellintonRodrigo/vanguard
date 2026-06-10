

using Vanguard.Domain.Entities;

namespace Vanguard.Domain.Interfaces
{
    public interface IWorkerExecutionLogRepository
    {
        Task InsertAsync( WorkerExecutionLog log,
        CancellationToken cancellationToken = default);
    }
}

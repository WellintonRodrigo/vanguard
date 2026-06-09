using Vanguard.Domain.Entities;

namespace Vanguard.Domain.Interfaces
{
     public interface ICollectorHealthLogRepository
    {
        Task InsertManyAsync(
        IReadOnlyCollection<CollectorHealthLog> logs,
        CancellationToken cancellationToken = default);

        Task<IReadOnlyCollection<CollectorHealthLog>>GetHistoryAsync(
    CancellationToken cancellationToken = default);

        
    }
}

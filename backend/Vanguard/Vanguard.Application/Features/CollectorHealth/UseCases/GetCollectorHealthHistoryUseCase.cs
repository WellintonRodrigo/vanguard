using Vanguard.Domain.Entities;
using Vanguard.Domain.Interfaces;

namespace Vanguard.Application.Features.CollectorHealth.UseCases
{
    public class GetCollectorHealthHistoryUseCase
    {
        private readonly ICollectorHealthLogRepository _repository;

        public GetCollectorHealthHistoryUseCase(ICollectorHealthLogRepository repository)
        {
            _repository = repository;
        }

        public async Task<IReadOnlyCollection<CollectorHealthLog>> ExecuteAsync(
            CancellationToken cancellationToken)
        {
            return await _repository.GetHistoryAsync(cancellationToken);
        }
    }
}

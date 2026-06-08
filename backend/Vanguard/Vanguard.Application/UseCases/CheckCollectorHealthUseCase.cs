using Vanguard.DataCollector.Health.Interfaces;
using Vanguard.DataCollector.Models;


namespace Vanguard.Application.UseCases
{
    public class CheckCollectorHealthUseCase
    {
        private readonly ICollectorHealthChecker _Checker;

        public CheckCollectorHealthUseCase(ICollectorHealthChecker checker)
        {
            _Checker = checker;
        }

        public async Task<IReadOnlyCollection<CollectorHealthResult>> ExecuteAsync(
            CancellationToken cancellationToken = default)
        {
            return await _Checker.CheckAsync(cancellationToken);
        }
    }
}

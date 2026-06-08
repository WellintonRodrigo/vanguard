using Vanguard.DataCollector.Health.Interfaces;
using Vanguard.DataCollector.Models;
using Vanguard.Domain.Entities;
using Vanguard.Domain.Interfaces;


namespace Vanguard.Application.UseCases
{
    public class CheckCollectorHealthUseCase
    {
        private readonly ICollectorHealthChecker _Checker;

        private readonly ICollectorHealthLogRepository _LogRepository;

        public CheckCollectorHealthUseCase(
            ICollectorHealthChecker checker, ICollectorHealthLogRepository logRepository)
        {
            _Checker = checker;
            _LogRepository = logRepository;
        }

        public async Task<IReadOnlyCollection<CollectorHealthResult>> ExecuteAsync(
            CancellationToken cancellationToken = default)
        {
            var results = await _Checker.CheckAsync(cancellationToken);

            var logs = results.Select(result => new CollectorHealthLog
            {
                
                Source = result.Source,
                Commodity = result.Commodity,
                Status = result.Status.ToString(),
                HttpStatusCode = result.HttpStatusCode,
                RecordsFound = result.RecordsFound,
                ResponseTimeMs = result.ResponseTimeMs,
                ErrorMessage = result.ErrorMessage,
                CheckedAt = result.CheckedAt
            })
                .ToList();

            try 
            { 

            await _LogRepository.InsertManyAsync(logs, cancellationToken);
           
            }
            catch (Exception ex)

            {
               Console.WriteLine($"Erro ao salvar logs de saúde: {ex.Message}");
            }
            return results;
        }
    }
}

using Microsoft.IdentityModel.Tokens.Experimental;
using Vanguard.Application.Features.CollectorHealth.DTOs;
using Vanguard.Domain.Interfaces;

namespace Vanguard.Application.Features.CollectorHealth.UseCases
{
    public class GetCollectorHealthSummaryUseCase
    {
        private readonly ICollectorHealthLogRepository _repository;

        public GetCollectorHealthSummaryUseCase(ICollectorHealthLogRepository repository)
        {
            _repository = repository;
        }

        public async Task<IReadOnlyCollection<CollectorHealthSummaryDto>> ExecutrAsync(
            CancellationToken cancellationToken = default)
        {
            var logs = await _repository.GetHistoryAsync(cancellationToken);
            var summary = logs
                .GroupBy(log => log.Source)
                .Select(group =>
                {
                    var totalChecks = group.Count();

                    var haelthyChecks = group.Count(x =>
                    x.Status.Equals("Healthy", StringComparison.OrdinalIgnoreCase));

                    var warningChecks = group.Count(x =>
                    x.Status.Equals("Warning", StringComparison.OrdinalIgnoreCase));

                    var criticalChecks = group.Count(x =>
                    x.Status.Equals("Unhealthy", StringComparison.OrdinalIgnoreCase));

                    var uptimePecent = totalChecks == 0
                    ? 0
                    : Math.Round(
                        ((decimal)haelthyChecks / totalChecks) * 100,
                        2);

                    var averageResponseTimeMs = group.Any()
                   ? Math.Round(group.Average(x => x.ResponseTimeMs), 2)
                   : 0;

                    return new CollectorHealthSummaryDto
                    {
                        Source = group.Key,
                        TotalChecks = totalChecks,
                        HealthyChecks = haelthyChecks,
                        WarningChecks = warningChecks,
                        CriticalChecks = criticalChecks,
                        UptimePercent = uptimePecent,
                        AverageResponseTimeMs = group.Average(x => x.ResponseTimeMs)
                    };
                })
                .ToList();

            return summary;
        }
    }
}

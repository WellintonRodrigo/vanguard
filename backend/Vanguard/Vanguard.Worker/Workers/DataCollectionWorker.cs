using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Diagnostics;
using Vanguard.Application.UseCases;
using Vanguard.Domain.Entities;
using Vanguard.Domain.Interfaces;
using Vanguard.Infrastructure.Repositories;
using Vanguard.Worker.Configuration;

namespace Vanguard.Worker.Workers
{
    public class DataCollectionWorker : BackgroundService
    {
        private readonly ILogger<DataCollectionWorker> _logger;
        private readonly IServiceProvider _serviceProvider;
        private readonly IOptions<WorkerSettings> _Settings;

        public DataCollectionWorker(
            ILogger<DataCollectionWorker> logger, IServiceProvider serviceProvider, IOptions<WorkerSettings> settings)
        {
            _logger = logger;
            _serviceProvider = serviceProvider;
            _Settings = settings;
        }

        protected override async Task ExecuteAsync(
            CancellationToken stoppingToken)
        {
            _logger.LogInformation("Vanguard Worker iniciado.");

            using var timer = new PeriodicTimer(
                TimeSpan.FromMinutes(_Settings.Value.IntervalMinutes));

            do
            {
                var startedAt = DateTime.UtcNow;

                var stopwatch = Stopwatch.StartNew();
                var intervalMinutes = _Settings.Value.IntervalMinutes;

                try
                {
                    using var scope = _serviceProvider.CreateScope();

                    var workerExecutionLogRepository =
                    scope.ServiceProvider.GetRequiredService<IWorkerExecutionLogRepository>();
                                       
                    var commodityUseCase =
                        scope.ServiceProvider.GetRequiredService<CollectCommodityPricesUseCase>();

                    var healthUseCase =
                        scope.ServiceProvider.GetRequiredService<CheckCollectorHealthUseCase>();

                    _logger.LogInformation("==============================================");
                    _logger.LogInformation(" Vanguard Worker - Novo ciclo iniciado");
                    _logger.LogInformation($" Início UTC: {startedAt}", startedAt);

                    _logger.LogInformation(" Coletando commodities...");

                    await commodityUseCase.ExecuteAsync(stoppingToken);

                    _logger.LogInformation(" Executando health check...");

                    await healthUseCase.ExecuteAsync(stoppingToken);

                    stopwatch.Stop();

                    var finiishedAt = DateTime.UtcNow;

                    await workerExecutionLogRepository.InsertAsync(
               new WorkerExecutionLog
               {
                        StartedAt = startedAt,
                        FinishedAt = finiishedAt,
                        DurationMs = stopwatch.ElapsedMilliseconds,
                        Success = true,
                        ErrorMessage = null
                    },stoppingToken);

                    _logger.LogInformation(" Ciclo concluído com sucesso");
                    _logger.LogInformation(" Duração: {DurationMs}ms", stopwatch.ElapsedMilliseconds);
                    _logger.LogInformation(" Próxima execução em {IntervalMinutes} minuto(s)", intervalMinutes);
                    _logger.LogInformation("==============================================");

                    
                }
                catch (Exception ex)
                {
                    stopwatch.Stop();

                    using var scope = _serviceProvider.CreateScope();

                     var workerExecutionLogRepository =
                scope.ServiceProvider.GetRequiredService<IWorkerExecutionLogRepository>();

                    await workerExecutionLogRepository.InsertAsync(
               new WorkerExecutionLog
               {
                        StartedAt = startedAt,
                        FinishedAt = DateTime.UtcNow,
                        DurationMs = stopwatch.ElapsedMilliseconds,
                        Success = false,
                        ErrorMessage = ex.Message
                    }, stoppingToken);

                    _logger.LogError(ex, "Erro durante o ciclo de coleta.");

                }
            }
            while (await timer.WaitForNextTickAsync(stoppingToken));
        }
    }
}

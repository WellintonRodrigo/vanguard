using Microsoft.Extensions.Options;
using System.Diagnostics;
using Vanguard.Application.UseCases; // Ajuste para os namespaces dos seus UseCases
using Vanguard.Domain.Entities;
using Vanguard.Domain.Interfaces;
using Vanguard.Worker.Configuration;

namespace Vanguard.Worker.Workers;

public class DataCollectionWorker : BackgroundService
{
    private readonly ILogger<DataCollectionWorker> _logger;
    private readonly IServiceProvider _serviceProvider;
    private readonly IOptions<WorkerSettings> _settings;

    // Tabela de coordenadas das praças agrícolas prioritárias
    private static readonly List<(string Location, double Lat, double Lon)> TargetRegions = new()
    {
        ("Joinville - SC", -26.3044, -48.8487),
        ("Sorriso - MT", -12.5425, -55.7211),
        ("Rio Verde - GO", -17.7922, -50.9189),
        ("Cascavel - PR", -24.9578, -53.4594)
    };

    public DataCollectionWorker(
        ILogger<DataCollectionWorker> logger,
        IServiceProvider serviceProvider,
        IOptions<WorkerSettings> settings)
    {
        _logger = logger;
        _serviceProvider = serviceProvider;
        _settings = settings;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation(" Vanguard Worker iniciado com sucesso.");

        // Define os intervalos (ex: 6 horas para clima e 4 horas para commodities)
        var weatherInterval = TimeSpan.FromHours(6);
        var commoditiesInterval = TimeSpan.FromHours(4);

        // Força a primeira execução imediata ao ligar o worker
        DateTime lastWeatherRun = DateTime.MinValue;
        DateTime lastCommoditiesRun = DateTime.MinValue;

        // O timer roda a cada 5 minutos verificando se deu a hora de rodar algum ciclo
        using var timer = new PeriodicTimer(TimeSpan.FromMinutes(5));

        do
        {
            var now = DateTime.UtcNow;

            // ----------------------------------------------------
            // 1. CICLO DE CLIMA (A cada 6 horas)
            // ----------------------------------------------------
            if (now - lastWeatherRun >= weatherInterval)
            {
                await RunWeatherSyncAsync(stoppingToken);
                lastWeatherRun = DateTime.UtcNow;
            }

            // ----------------------------------------------------
            // 2. CICLO DE COMMODITIES (A cada 4 horas)
            // ----------------------------------------------------
            if (now - lastCommoditiesRun >= commoditiesInterval)
            {
                await RunCommoditiesSyncAsync(stoppingToken);
                lastCommoditiesRun = DateTime.UtcNow;
            }

        } while (await timer.WaitForNextTickAsync(stoppingToken));
    }

    private async Task RunWeatherSyncAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("[WORKER] Iniciando ciclo de sincronização de clima...");

        foreach (var (location, lat, lon) in TargetRegions)
        {
            if (stoppingToken.IsCancellationRequested) break;

            var startedAt = DateTime.UtcNow;
            var stopwatch = Stopwatch.StartNew();

            try
            {
                using var scope = _serviceProvider.CreateScope();
                var weatherUseCase = scope.ServiceProvider.GetRequiredService<SyncWeatherServiceUseCase>();

                _logger.LogInformation("Sincronizando clima para: {Location} ({Lat}, {Lon})", location, lat, lon);
                await weatherUseCase.ExecuteAsync(location, lat, lon);

                stopwatch.Stop();
                _logger.LogInformation("Clima de {Location} atualizado com sucesso em {ElapsedMs}ms.", location, stopwatch.ElapsedMilliseconds);

                // 🛑 POLIDEZ DE TRÁFEGO: Aguarda 2,5 segundos antes de chamar a próxima praça agrícola
                await Task.Delay(2500, stoppingToken);
            }
            catch (Exception ex)
            {
                stopwatch.Stop();
                _logger.LogError(ex, "Erro ao sincronizar clima para {Location}.", location);
            }
        }
    }

    private async Task RunCommoditiesSyncAsync(CancellationToken stoppingToken)
    {
        var startedAt = DateTime.UtcNow;
        var stopwatch = Stopwatch.StartNew();

        _logger.LogInformation("[WORKER] Coletando cotações de commodities...");

        try
        {
            using var scope = _serviceProvider.CreateScope();

            var commodityUseCase = scope.ServiceProvider.GetRequiredService<CollectCommodityPricesUseCase>();
            var healthUseCase = scope.ServiceProvider.GetRequiredService<CheckCollectorHealthUseCase>();
            var workerExecutionLogRepository = scope.ServiceProvider.GetRequiredService<IWorkerExecutionLogRepository>();

            await commodityUseCase.ExecuteAsync(stoppingToken);
            await healthUseCase.ExecuteAsync(stoppingToken);

            stopwatch.Stop();

            await workerExecutionLogRepository.InsertAsync(new WorkerExecutionLog
            {
                StartedAt = startedAt,
                FinishedAt = DateTime.UtcNow,
                DurationMs = stopwatch.ElapsedMilliseconds,
                Success = true,
                ErrorMessage = null
            }, stoppingToken);

            _logger.LogInformation("Ciclo de commodities concluído com sucesso!");
        }
        catch (Exception ex)
        {
            stopwatch.Stop();
            _logger.LogError(ex, "Erro durante o ciclo de coleta de commodities.");
        }
    }
}
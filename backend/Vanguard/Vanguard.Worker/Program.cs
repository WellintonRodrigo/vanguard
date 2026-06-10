using Vanguard.Application.UseCases;
using Vanguard.DataCollector.Collectors;
using Vanguard.DataCollector.Collectors.Interfaces;
using Vanguard.DataCollector.Health.HealthCheckers;
using Vanguard.DataCollector.Health.Interfaces;
using Vanguard.DataCollector.Parsers;
using Vanguard.Domain.Interfaces;
using Vanguard.Infrastructure.Persistence.Configurations;
using Vanguard.Infrastructure.Persistence.Context;
using Vanguard.Infrastructure.Repositories;
using Vanguard.Worker.Configuration;
using Vanguard.Worker.Workers;

var builder = Host.CreateApplicationBuilder(args);

builder.Services.Configure<WorkerSettings>(builder.Configuration.GetSection(
        WorkerSettings.SectionName));

Console.WriteLine($"Environment: {builder.Environment.EnvironmentName}");
Console.WriteLine($"Mongo vazia: {string.IsNullOrWhiteSpace(builder.Configuration["MongoDb:ConnectionString"])}");

builder.Logging.AddFilter("System.Net.Http.HttpClient", LogLevel.Warning);

builder.Services.Configure<MongoDbSettings>(
    builder.Configuration.GetSection(MongoDbSettings.SectionName));

builder.Services.AddSingleton<MongoDbContext>();

builder.Services.AddScoped<ICommodityPriceRepository, CommodityPriceRepository>();
builder.Services.AddScoped<ICollectorHealthLogRepository, CollectorHealthLogRepository>();
builder.Services.AddScoped<CollectCommodityPricesUseCase>();
builder.Services.AddScoped<CheckCollectorHealthUseCase>();
builder.Services.AddScoped<NoticiasAgricolasCommodityParser>();
builder.Services.AddHttpClient<ICommodityCollector, NoticiasAgricolasCommodityCollector>();
builder.Services.AddHttpClient<ICollectorHealthChecker, NoticiasAgricolasHealthChecker>();
builder.Services.AddScoped<IWorkerExecutionLogRepository, WorkerExecutionLogRepository>();



builder.Services.AddHostedService<DataCollectionWorker>();

var host = builder.Build();

await host.RunAsync();

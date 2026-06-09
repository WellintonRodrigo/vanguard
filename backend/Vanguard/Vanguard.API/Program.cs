using Vanguard.Application.Features.CollectorHealth.UseCases;
using Vanguard.Application.Features.Insights;
using Vanguard.Application.Features.Predictions.Services;
using Vanguard.Application.Interfaces;
using Vanguard.Application.UseCases;
using Vanguard.DataCollector.Collectors;
using Vanguard.DataCollector.Collectors.Interfaces;
using Vanguard.DataCollector.Health.HealthCheckers;
using Vanguard.DataCollector.Health.Interfaces;
using Vanguard.DataCollector.Parsers;
using Vanguard.Domain.Interfaces;
using Vanguard.Infrastructure.ExternalService.OpenMeteo;
using Vanguard.Infrastructure.Persistence.Configurations;
using Vanguard.Infrastructure.Persistence.Context;
using Vanguard.Infrastructure.Repositories;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddScoped<PredictionService>();

builder.Configuration.GetSection("MongoDb");
builder.Services.Configure<MongoDbSettings>(
    builder.Configuration.GetSection(MongoDbSettings.SectionName));
builder.Services.AddSingleton<MongoDbContext>();

builder.Services.AddHttpClient<ICommodityCollector, NoticiasAgricolasCommodityCollector>();
builder.Services.AddHttpClient<ICollectorHealthChecker, NoticiasAgricolasHealthChecker>();
builder.Services.AddHttpClient<IWeatherProvider, OpenMeteoWeatherProvider>(
    client =>
    {
        client.BaseAddress = new Uri("https://api.open-meteo.com/v1/");
        client.Timeout = TimeSpan.FromSeconds(10);
    });

builder.Services.AddSingleton<InsightTemplateService>();
builder.Services.AddScoped<PredictionEngineService>();
builder.Services.AddScoped<ICommodityPriceRepository, CommodityPriceRepository>();
builder.Services.AddScoped<CollectCommodityPricesUseCase>();
builder.Services.AddScoped<NoticiasAgricolasCommodityParser>();
builder.Services.AddScoped<CheckCollectorHealthUseCase>();
builder.Services.AddScoped<IPredictionRepository, PredictionRepository>();
builder.Services.AddScoped<ICollectorHealthLogRepository, CollectorHealthLogRepository>();
builder.Services.AddScoped<GetCollectorHealthHistoryUseCase>();
builder.Services.AddScoped<GetCollectorHealthSummaryUseCase>();

// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();

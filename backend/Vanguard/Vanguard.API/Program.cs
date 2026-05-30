using Vanguard.Application.Features.Predictions.Services;
using Vanguard.Application.Interfaces;
using Vanguard.Domain.Interfaces;
using Vanguard.Infrastructure.ExternalService.OpenMeteo;
using Vanguard.Infrastructure.Persistence.Configurations;
using Vanguard.Infrastructure.Persistence.Context;
using Vanguard.Infrastructure.Persistence.Repositories;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddScoped<PredictionService>();
builder.Configuration.GetSection("MongoDb");
builder.Services.Configure<MongoDbSettings>(
    builder.Configuration.GetSection(MongoDbSettings.SectionName));
builder.Services.AddSingleton<MongoDbContext>();
builder.Services.AddScoped<IPredictionRepository, PredictionRepository>();
builder.Services.AddHttpClient<IWeatherProvider, OpenMeteoWeatherProvider>(
    client =>
    {
        client.BaseAddress = new Uri("https://api.open-meteo.com/v1/");
        client.Timeout = TimeSpan.FromSeconds(10);
    });

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

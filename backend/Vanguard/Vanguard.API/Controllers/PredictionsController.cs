using Microsoft.AspNetCore.Mvc;
using Vanguard.Application.Features.Insights;
using Vanguard.Application.Features.Predictions.DTOs;
using Vanguard.Application.Features.Predictions.Services;
using Vanguard.Application.Interfaces;
namespace Vanguard.API.Controllers
{
    [ApiController]
    [Route("predictions")]
    public class PredictionsController: ControllerBase
    {
        private readonly PredictionService _predictionService;
        private readonly IWeatherProvider _weatherProvider;
        private readonly InsightTemplateService _insightTemplateService;
        private readonly PredictionEngineService _predictionEngineService;
        public PredictionsController(
            PredictionService predictionService, 
            IWeatherProvider weatherProvider,
            InsightTemplateService insightTemplateService,
            PredictionEngineService predictionEngineService
            )
        {
            _predictionService = predictionService;
            _weatherProvider = weatherProvider;
            _insightTemplateService = insightTemplateService;
            _predictionEngineService = predictionEngineService;
        }

        [HttpPost("analize")]
        public IActionResult Analize([FromBody] PredictionRequestDto request)
        {
           var result = _predictionService.Analyze(request.Question);
            return Ok(result);
        }
        [HttpPost("teste")]
        public async Task<IActionResult> teste() // controller de teste para verificar se a persistência está funcionando
        {
            await _predictionService.SalvarTesteAsync();
            return Ok("Teste salvo com sucesso");
        }
        [HttpGet("weather-test")]
        public async Task<IActionResult> WeatherTest() // controller de teste para verificar se a integração com o Open-Meteo está funcionando
        {
            var result = await _weatherProvider.GetWeatherAsync(
                latitude: -26.3044,
                longitude: -48.8487,
                location: "Joinville - SC"
                );
            return Ok(result);
        }
        [HttpGet("insight-test")]// controller de teste para verificar se a geração de insights está funcionando
        public IActionResult InsightTest()
        {
            var insight = _insightTemplateService.Generate(
                key: "heavyRain",
                product: "Tomate",
                location: "Joinville - SC");

            return Ok(new { insight });
        }

        [HttpGet("forecast-test")] // controller de teste para verificar se a previsão de 7 dias agrícola está funcionando.
        public async Task<IActionResult> ForecastTest()
        {
            var result = await _weatherProvider.GetWeatherForecastAsync(
                latitude: -26.3044,
                longitude: -48.8487,
                location: "Joinville - SC");
                    
            return Ok(result);
        }

        [HttpPost("agriculture-prediction")] // Controller oficial para realizar a previsão agrícola, utilizando o PredictionEngineService para processar os dados e gerar a previsão
        public async Task<IActionResult> AgriculturePrediction(AgriculturePredictionRequestDto requestDto)
        {
            var result = await _predictionEngineService.AnalyzeAgricultureAsync(requestDto);

            return Ok(result);
        }

    }
}

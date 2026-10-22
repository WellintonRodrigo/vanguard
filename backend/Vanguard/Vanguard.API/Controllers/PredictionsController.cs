using Microsoft.AspNetCore.Mvc;
using Vanguard.Application.Features.Insights;
using Vanguard.Application.Features.Predictions.DTOs;
using Vanguard.Application.Features.Predictions.Services;
using Vanguard.Application.Interfaces;
using Vanguard.Domain.Interfaces;
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
        private readonly IPredictionRepository _predictionRepository;
        public PredictionsController(
            PredictionService predictionService, 
            IWeatherProvider weatherProvider,
            InsightTemplateService insightTemplateService,
            PredictionEngineService predictionEngineService,
            IPredictionRepository predictionRepository
            )
        {
            _predictionService = predictionService;
            _weatherProvider = weatherProvider;
            _insightTemplateService = insightTemplateService;
            _predictionEngineService = predictionEngineService;
            _predictionRepository = predictionRepository;
        }

        [HttpPost("analize")]
        public IActionResult Analize([FromBody] PredictionRequestDto request)
        {
           var result = _predictionService.Analyze(request.Question);
            return Ok(result);
        }

        [HttpPost("agriculture-prediction")] // Controller oficial para realizar a previsão agrícola, utilizando o PredictionEngineService para processar os dados e gerar a previsão
        public async Task<IActionResult> AgriculturePrediction(
            AgriculturePredictionRequestDto requestDto)
        {
            var result = await _predictionEngineService
                .AnalyzeAgricultureAsync(requestDto);

            return Ok(result);
        }

        [HttpPost("agriculture/forecast")] // Gera uma análise baseada no forecast de 7 dias e salva no banco.

        public async Task<IActionResult> AgricultureForecastPrediction(
            [FromBody] AgriculturePredictionRequestDto requestDto)
        {
            var result = await _predictionEngineService
                .AnalyzaAgricultureForecastAsync(requestDto);

            return Ok(result);
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()//Lista o histórico de todas as predições salvas no MongoDB
        {
            var predictions = await _predictionRepository.GetAllAsync();
            return Ok(predictions);
        }

        [HttpGet("product/{productName}")]
        public async Task<IActionResult> GetByProduct(string productName)
        {
            var predictions = await _predictionRepository.GetByProductAsync(productName);
            return Ok(predictions);
        }

    }
}

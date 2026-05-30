using Microsoft.AspNetCore.Mvc;
using Vanguard.Application.DTOs.Predictions;
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
        public PredictionsController(PredictionService predictionService, IWeatherProvider weatherProvider)
        {
            _predictionService = predictionService;
            _weatherProvider = weatherProvider;
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
    }
}

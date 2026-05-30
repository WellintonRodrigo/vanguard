using Microsoft.AspNetCore.Mvc;
using Vanguard.Application.DTOs.Predictions;
using Vanguard.Application.Features.Predictions.Services;
namespace Vanguard.API.Controllers
{
    [ApiController]
    [Route("predictions")]
    public class PredictionsController: ControllerBase
    {
        private readonly PredictionService _predictionService;
        public PredictionsController(PredictionService predictionService)
        {
            _predictionService = predictionService;
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
    }
}

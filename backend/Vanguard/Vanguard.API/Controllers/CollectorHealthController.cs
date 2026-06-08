using Microsoft.AspNetCore.Mvc;
using Vanguard.Application.UseCases;

namespace Vanguard.API.Controllers
{
    [ApiController]
    [Route("collector")]
    public class CollectorHealthController : ControllerBase
    {
        private readonly CheckCollectorHealthUseCase _healthUseCase;

        public CollectorHealthController(CheckCollectorHealthUseCase healthUseCase)
        {
            _healthUseCase = healthUseCase;
        }

        [HttpGet("health")]
        public async Task<IActionResult> GetHealthAsync(CancellationToken cancellationToken)
        {
            var healthResults = await _healthUseCase.ExecuteAsync(cancellationToken);
            return Ok(healthResults);
        }
    }
}
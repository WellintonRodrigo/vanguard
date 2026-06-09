using Microsoft.AspNetCore.Mvc;
using Vanguard.Application.Features.CollectorHealth.UseCases;
using Vanguard.Application.UseCases;

namespace Vanguard.API.Controllers
{
    [ApiController]
    [Route("collector")]
    public class CollectorHealthController : ControllerBase
    {
        private readonly CheckCollectorHealthUseCase _healthUseCase;
        private readonly GetCollectorHealthHistoryUseCase _historyUseCase;
        private readonly GetCollectorHealthSummaryUseCase _summaryUseCase;


        public CollectorHealthController(CheckCollectorHealthUseCase healthUseCase,
            GetCollectorHealthHistoryUseCase historyUseCase,
            GetCollectorHealthSummaryUseCase summaryUseCase)
        {
            _healthUseCase = healthUseCase;
            _historyUseCase = historyUseCase;
            _summaryUseCase = summaryUseCase;
        }

        [HttpGet("health")]
        public async Task<IActionResult> GetHealthAsync(CancellationToken cancellationToken)
        {
            var healthResults = await _healthUseCase.ExecuteAsync(cancellationToken);
            return Ok(healthResults);
        }

        [HttpGet("history")]
        public async Task<IActionResult> History(
        CancellationToken cancellationToken)
        {
            var result = await _historyUseCase.ExecuteAsync(
                cancellationToken);

            return Ok(result);
        }

        [HttpGet("summary")]
        public async Task<IActionResult> Summary(
            CancellationToken cancellationToken)
        { 
            var result = await _summaryUseCase.ExecutrAsync(
                cancellationToken);

            return Ok(result);
        }
    }
}
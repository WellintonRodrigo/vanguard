using Microsoft.AspNetCore.Mvc;
using Vanguard.Application.Features.Commodities.UseCases;
using Vanguard.Application.UseCases;


namespace Vanguard.API.Controllers
{
    [ApiController]
    [Route("commodity-prices")]
    public class CommodityPricesController : ControllerBase
    {
        private readonly CollectCommodityPricesUseCase _collectUseCase;
        private readonly GetAllCommodityPricesUseCase _getAllUseCase;
        private readonly GetLatestCommodityPriceUseCase _getLatestUseCase;
        private readonly GetCommodityHistoryUseCase _getHistoryUseCase;
        private readonly GetCommoditiesUseCase _getCommoditiesUseCase;
        private readonly GetCommodityPricesSummaryUseCase _getSummaryUseCase;

        public CommodityPricesController(
            CollectCommodityPricesUseCase collectUseCase,
            GetAllCommodityPricesUseCase getAllUseCase,
            GetLatestCommodityPriceUseCase getLatestUseCase,
            GetCommodityHistoryUseCase getHistoryUseCase,
            GetCommoditiesUseCase getCommoditiesUse,
            GetCommodityPricesSummaryUseCase getSummaryUseCase)
        {
            _collectUseCase = collectUseCase;
            _getAllUseCase = getAllUseCase;
            _getHistoryUseCase = getHistoryUseCase;
            _getLatestUseCase = getLatestUseCase;
            _getCommoditiesUseCase = getCommoditiesUse;
            _getSummaryUseCase = getSummaryUseCase;
        }

        [HttpPost("collect")]

        public async Task<IActionResult> Collect(CancellationToken cancellationToken)
        {
            var collectedCount = await _collectUseCase.ExecuteAsync(cancellationToken);
            return Ok(new
            {
                message = "Coleta de preços das commodities realizada com sucesso.",
                collectedCount
            });
        }

        // Consultas de commoditeis

        [HttpGet]
        public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
        {
            var price = await _getAllUseCase.ExecuteAsync(cancellationToken);
            return Ok(price);
        }

        [HttpGet("latest")]
        public async Task<IActionResult> GetLatest([FromQuery] string? commodity,
    CancellationToken cancellationToken)
        {
            var result = await _getLatestUseCase.ExecuteAsync(
                commodity,
                cancellationToken);

            if (result is null)
            {
                return NotFound(new
                {
                    message = "Nenhum preço encontrado."
                });
            }
            return Ok(result);
        }

        [HttpGet("history")]
        public async Task<IActionResult> GetHistory([FromQuery] string commodity,
            [FromQuery] int days = 30,
            CancellationToken cancellationToken = default)
        {
            var prices = await _getHistoryUseCase.ExecuteAsync(
                commodity,
                days,
                cancellationToken);

            return Ok(prices);
        }

        [HttpGet("commodities")]
        public async Task<IActionResult> GetCommodities(
        CancellationToken cancellationToken)
        {
            var commodities = await _getCommoditiesUseCase.ExecuteAsync(
                cancellationToken);

            return Ok(commodities);
        }

        [HttpGet("summary")]
        public async Task<IActionResult> GetSummary(
        CancellationToken cancellationToken)
        {
            var summary = await _getSummaryUseCase.ExecuteAsync(cancellationToken);

            return Ok(summary);
        }
    }
}

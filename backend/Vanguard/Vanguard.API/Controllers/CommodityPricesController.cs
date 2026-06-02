using Microsoft.AspNetCore.Mvc;
using Vanguard.Application.UseCases;


namespace Vanguard.API.Controllers
{
    [ApiController]
    [Route("commodity-prices")]
    public class CommodityPricesController : ControllerBase
    {
        private readonly CollectCommodityPricesUseCase _useCase;

        public CommodityPricesController(CollectCommodityPricesUseCase useCase)
        {
            _useCase = useCase;
        }

        [HttpPost("collect")]

        public async Task<IActionResult> Collect(CancellationToken cancellationToken)
        {
       var collectedCount= await _useCase.ExecuteAsync(cancellationToken);
            return Ok(new {
                message = "Commodity prices collected successfully.", collectedCount});
        }
    }
}

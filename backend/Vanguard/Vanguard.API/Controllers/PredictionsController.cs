using Microsoft.AspNetCore.Mvc;
using Vanguard.Application.DTOs;

namespace Vanguard.API.Controllers
{
    [ApiController]
    [Route("predictions")]
    public class PredictionsController: ControllerBase
    {
        [HttpPost("analize")]
        public IActionResult Analize([FromBody] PredictionRequestDto request)
        {
            var response = new
            {
                probability = 78,
                confidence = 91,
                trend = "UP",
                analyzedAt = DateTime.UtcNow
            };
            return Ok(response);
        }

    }
}

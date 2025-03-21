using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using PharmaStock.Services;

namespace PharmaStock.Controllers
{
    [ApiController]
    [Route("api/predicciones")]
    
    public class PredictionController : ControllerBase
    {
        private readonly PredictionService _predictionService;

        public PredictionController(PredictionService predictionService)
        {
            _predictionService = predictionService;
        }

        [HttpGet]
        public async Task<IActionResult> GetPrediction()
        {
            await _predictionService.FetchAndBroadcastPrediction();
            return Ok(new { message = "Predicciones enviadas a los clientes" });
        }
    }
}

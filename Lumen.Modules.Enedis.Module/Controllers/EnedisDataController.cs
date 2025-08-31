using Lumen.Modules.Enedis.Business.Interfaces;

using Microsoft.AspNetCore.Mvc;

using System.Net;

namespace Lumen.Modules.Enedis.Module.Controllers {
    public class Payload {
        public string cookie { get; set; }
    }

    [ApiController]
    [Route("[controller]")]
    public class EnedisDataController(ILogger<EnedisDataController> logger, IEnedisApi enedisApi) : ControllerBase {
        private static DateTime? LastRun = null;

        [HttpPost("queryDataFromEnedis")]
        public async Task<IActionResult> QueryDataFromEnedis([FromBody] Payload payload) {
            if (LastRun is not null && LastRun.Value.AddHours(1) > DateTime.UtcNow) {
                logger.LogInformation("Skipping QueryDataFromEnedis() to prevent spamming Enedis API");
                return NoContent();
            }

            logger.BeginScope("Getting data from the Enedis API ...");

            try {
                await enedisApi.QueryConsumptionData(payload.cookie, EnedisModule.PersonNumber, EnedisModule.PrmNumber);

                LastRun = DateTime.UtcNow;

                return Accepted();
            } catch (Exception ex) {
                logger.LogError(ex, "Error when querying Enedis data");
                return StatusCode((int)HttpStatusCode.InternalServerError);
            }
        }
    }
}

using Metrics.Models;
using Metrics.Models.Request;
using Metrics.Models.Response;
using Metrics.Services.Client;
using Microsoft.AspNetCore.Mvc;

namespace Metrics.Controllers
{
    [Route("api/cpu")]
    [ApiController]
    public class CpuMetricsController : ControllerBase
    {
        private IHttpClientFactory _httpClientFactory;
        private AgentPool _agentPool;
        private IMetricsAgentClient _metricsAgentClient;

        public CpuMetricsController(IHttpClientFactory httpClientFactory,
            AgentPool agentPool,
            IMetricsAgentClient metricsAgentClient)
        {
            _httpClientFactory = httpClientFactory;
            _agentPool = agentPool;
            _metricsAgentClient = metricsAgentClient;
        }

        //[HttpGet("agent/{agentId}/from/{fromTime}/to/{toTime}")]
        [HttpGet("get-all-by-id")]
        public ActionResult<CpuMetricsPesponse> GetMetricsFromAgent(
            [FromQuery] int agentId, [FromQuery] TimeSpan fromTime, [FromQuery] TimeSpan toTime)
        {
            return Ok( _metricsAgentClient.GetCpuMetrics(new CpuMetricsRequest
            {
                AgentId = agentId,
                FromTime = fromTime,
                ToTime = toTime
            })); 
        }

        //[HttpGet("all/from/{fromTime}/to/{toTime}")]
        [HttpGet("get-all")]
        public IActionResult GetMetricsFromAll(
            [FromQuery] TimeSpan fromTime,[FromQuery] TimeSpan toTime)
        {
            return Ok();
        }
    }
}

using AutoMapper;
using MetricsAgent.Models.Request;
using MetricsAgent.Models;
using MetricsAgent.Services;
using MetricsAgent.Services.Impl;
using Microsoft.AspNetCore.Mvc;
using MetricsAgent.Models.Dto;

namespace MetricsAgent.Controllers
{
    [Route("api/metrics/network")]
    [ApiController]
    public class NetworkMetricsController : ControllerBase
    {
        private readonly ILogger<NetworkMetricsController> _logger;
        private readonly INetworkMetricsRepository _networkMetricsRepository;
        private readonly IMapper _mapper;

        public NetworkMetricsController(ILogger<NetworkMetricsController> logger,
            INetworkMetricsRepository networkMetricsRepository,
            IMapper mapper)
        {
            _logger = logger;
            _networkMetricsRepository = networkMetricsRepository;
            _mapper = mapper;
        }

        [HttpGet("from/{fromTime}/to/{toTime}")]
        public ActionResult<IList<NetworkMetricDto>> GetNetworkMetics([FromRoute] TimeSpan fromTime,
            [FromRoute] TimeSpan toTime)
        {
            _logger.LogInformation("Get cpu metrics call");
            return Ok(_networkMetricsRepository.GetByTimePeriod(fromTime, toTime).Select(metric =>
           _mapper.Map<NetworkMetricDto>(metric)).ToList());
        }

        [HttpGet("all")]
        public ActionResult<IList<NetworkMetricDto>> GetAllNetworkMetrics()
        {
            return Ok(_networkMetricsRepository.GetAll().Select(metric =>
            _mapper.Map<NetworkMetricDto>(metric)).ToList());
        }
    }
}

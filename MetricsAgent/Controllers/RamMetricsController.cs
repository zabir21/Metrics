using AutoMapper;
using MetricsAgent.Models.Dto;
using MetricsAgent.Models.Request;
using MetricsAgent.Models;
using MetricsAgent.Services;
using MetricsAgent.Services.Impl;
using Microsoft.AspNetCore.Mvc;

namespace MetricsAgent.Controllers
{
    [Route("api/metrics/ram")]
    [ApiController]
    public class RamMetricsController : ControllerBase
    {
        private readonly ILogger<RamMetricsController> _logger;
        private readonly IRamMetricsRepository _ramMetricsRepository;
        private readonly IMapper _mapper;

        public RamMetricsController(ILogger<RamMetricsController> logger,
            IRamMetricsRepository ramMetricsRepository,
            IMapper mapper)
        {
            _logger = logger;
            _ramMetricsRepository = ramMetricsRepository;
            _mapper = mapper;
        }

        [HttpGet("from/{fromTime}/to/{toTime}")]
        public ActionResult<IList<RamMetricDto>> GetRamMetrics(
            [FromRoute] TimeSpan fromTime, [FromRoute] TimeSpan toTime)
        {
            _logger.LogInformation("Get cpu metrics call");

            return Ok(_ramMetricsRepository.GetByTimePeriod(fromTime, toTime).Select(metric =>
            _mapper.Map<RamMetricDto>(metric)).ToList());
        }

        [HttpGet("all")]
        public ActionResult<IList<RamMetricDto>> GetAllRamMetrics()
        {
            return Ok(_ramMetricsRepository.GetAll().Select(metric =>
            _mapper.Map<RamMetricDto>(metric)).ToList());
        }
    }
}

using AutoMapper;
using MetricsAgent.Models.Request;
using MetricsAgent.Models;
using MetricsAgent.Services;
using Microsoft.AspNetCore.Mvc;
using MetricsAgent.Models.Dto;
using MetricsAgent.Services.Impl;

namespace MetricsAgent.Controllers
{
    [Route("api/metrics/hdd")]
    [ApiController]
    public class HddMetricsController : ControllerBase
    {
        private readonly ILogger<HddMetricsController> _logger;
        private readonly IHddMetricsRepository _hddMetricsRepository;
        private readonly IMapper _mapper;

        public HddMetricsController(IHddMetricsRepository hddMetricsRepository,
            ILogger<HddMetricsController> logger,
            IMapper mapper)
        {
            _hddMetricsRepository = hddMetricsRepository;
            _logger = logger;
            _mapper = mapper;
        }

        [HttpGet("from/{fromTime}/to/{toTime}")]
        public ActionResult<IList<HddMetrics>> GetHddMetrics(
            [FromRoute] TimeSpan fromTime, [FromRoute] TimeSpan toTime)
        {
            _logger.LogInformation("Get hdd metrics call.");

            return Ok(_hddMetricsRepository.GetByTimePeriod(fromTime, toTime)
                .Select(metric => _mapper.Map<HddMetricDto>(metric)).ToList());
        }

        [HttpGet("all")]
        public ActionResult<IList<HddMetricDto>> GetAllHddMetrics()
        {
            return Ok(_hddMetricsRepository.GetAll().Select(metric =>
            _mapper.Map<HddMetricDto>(metric)).ToList());
        }
    }
}

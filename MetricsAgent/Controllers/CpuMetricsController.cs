using AutoMapper;
using MetricsAgent.Models.Dto;
using MetricsAgent.Models.Response;
using MetricsAgent.Services;
using Microsoft.AspNetCore.Mvc;

namespace MetricsAgent.Controllers
{
    [Route("api/metrics/cpu")]
    [ApiController]
    public class CpuMetricsController : ControllerBase
    {
        private readonly ILogger<CpuMetricsController> _logger;
        private readonly ICpuMetricsRepository _cpuMetricsRepository;
        private readonly IMapper _mapper;

        public CpuMetricsController(ILogger<CpuMetricsController> logger,
            ICpuMetricsRepository cpuMetricsRepository,
            IMapper mapper) 
        {
            _logger = logger;
            _cpuMetricsRepository = cpuMetricsRepository;
            _mapper = mapper;
        }

        [HttpGet("from/{fromTime}/to/{toTime}")]
        public ActionResult<GetCpuMetricResponse> GetCpuMetrics(
           [FromRoute] TimeSpan fromTime, [FromRoute] TimeSpan toTime)
        {
            _logger.LogInformation("Get cpu metrics call.");

            return Ok(new GetCpuMetricResponse
            {
                Metrics = _cpuMetricsRepository.GetByTimePeriod(fromTime, toTime)
                        .Select(metric => _mapper.Map<CpuMetricDto>(metric)).ToList()
            });
        }

        [HttpGet("all")]
        public ActionResult<IList<CpuMetricDto>> GetAllCpuMetrics() 
        {
            return Ok(_cpuMetricsRepository.GetAll()
                   .Select(metric => _mapper.Map<CpuMetricDto>(metric)).ToList());
        }
    }
}

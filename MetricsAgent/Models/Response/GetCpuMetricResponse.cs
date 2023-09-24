using MetricsAgent.Models.Dto;

namespace MetricsAgent.Models.Response
{
    public class GetCpuMetricResponse
    {
        public List<CpuMetricDto> Metrics { get; set; }
    }
}

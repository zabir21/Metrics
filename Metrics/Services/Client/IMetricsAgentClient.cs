using Metrics.Models.Request;
using Metrics.Models.Response;

namespace Metrics.Services.Client
{
    public interface IMetricsAgentClient
    {
        CpuMetricsPesponse GetCpuMetrics(CpuMetricsRequest request);
    }
}

using System.Text.Json.Serialization;

namespace Metrics.Models.Response
{
    public class CpuMetricsPesponse
    {
        public int AgentId { get; set; }
        [JsonPropertyName("metrics")]
        public CpuMetric[] Metrics { get; set; }
    }
}

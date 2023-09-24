using System.Text.Json.Serialization;

namespace Metrics.Models
{
    public class CpuMetric
    {
        [JsonPropertyName("time")]
        public int Time { get; set; }
        [JsonPropertyName("value")]
        public int Value { get; set; }
           
    }
}

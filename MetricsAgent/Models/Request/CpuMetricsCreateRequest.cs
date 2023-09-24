namespace MetricsAgent.Models.Request
{
    public class CpuMetricsCreateRequest
    {
        public int Value { get; set; }
        public TimeSpan Time { get; set; }
    }
}

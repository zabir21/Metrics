namespace MetricsAgent.Models.Request
{
    public class RamMetricsCreateRequest
    {
        public int Value { get; set; }
        public TimeSpan Time { get; set; }
    }
}

namespace MetricsAgent.Models.Request
{
    public class NetworkMetricsCreateRequest
    {
        public int Value { get; set; }
        public TimeSpan Time { get; set; }
    }
}

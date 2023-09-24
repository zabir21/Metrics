namespace MetricsAgent.Models.Request
{
    public class HddMetricsCreateRequest
    {
        public int Value { get; set; }
        public TimeSpan Time { get; set; }
    }
}

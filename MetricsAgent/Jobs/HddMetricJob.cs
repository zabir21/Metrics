using MetricsAgent.Models;
using MetricsAgent.Services;
using Quartz;
using System.Diagnostics;

namespace MetricsAgent.Jobs
{
    public class HddMetricJob : IJob
    {
        private PerformanceCounter _hddCounter;
        private IServiceScopeFactory _serviceScopeFactory;

        public HddMetricJob(IServiceScopeFactory serviceScopeFactory)
        {
            _serviceScopeFactory = serviceScopeFactory;
            _hddCounter = new PerformanceCounter("PhysicalDisk", "Disk Write Bytes/sec", "_Total");
        }

        public Task Execute(IJobExecutionContext context)
        {
            using (IServiceScope serviceScope = _serviceScopeFactory.CreateScope())
            {
                var hddMetricsRepository = serviceScope.ServiceProvider.GetService<IHddMetricsRepository>();
                try
                {
                    var hddSpeed = _hddCounter.NextValue();
                    var time = TimeSpan.FromSeconds(DateTimeOffset.UtcNow.ToUnixTimeSeconds());
                    Debug.WriteLine($"{time} > {hddSpeed}");
                    hddMetricsRepository.Create(new HddMetrics
                    {
                        Value = (int)hddSpeed,
                        Time = (long)time.TotalSeconds
                    });
                }
                catch
                {

                }
            }

            return Task.CompletedTask;
        }
    }
}
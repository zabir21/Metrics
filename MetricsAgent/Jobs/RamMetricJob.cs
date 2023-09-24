using MetricsAgent.Models;
using MetricsAgent.Services;
using Quartz;
using System.Diagnostics;

namespace MetricsAgent.Jobs
{
    public class RamMetricJob : IJob
    {
        private PerformanceCounter _cpuCounter;
        private IServiceScopeFactory _serviceScopeFactory;

        public RamMetricJob(IServiceScopeFactory serviceScopeFactory)
        {
            _serviceScopeFactory = serviceScopeFactory;
            _cpuCounter = new PerformanceCounter("Memory", "Available MBytes", null);
        }

        public Task Execute(IJobExecutionContext context)
        {
            using (IServiceScope serviceScope = _serviceScopeFactory.CreateScope())
            {
                var ramMetricsRepository = serviceScope.ServiceProvider.GetService<IRamMetricsRepository>();
                try
                {
                    var ramMemory = _cpuCounter.NextValue();
                    var time = TimeSpan.FromSeconds(DateTimeOffset.UtcNow.ToUnixTimeSeconds());
                    Debug.WriteLine($"{time} > {ramMemory}");
                    ramMetricsRepository.Create(new RamMetrics
                    {
                        Value = (int)ramMemory,
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

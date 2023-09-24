using MetricsAgent.Services;
using MetricsAgent.Services.Impl;
using Quartz;
using System.Diagnostics;

namespace MetricsAgent.Jobs
{
    public class CpuMetricJob : IJob
    {
        private PerformanceCounter _cpuCounter;
        private IServiceScopeFactory _serviceScopeFactory;

        public CpuMetricJob(IServiceScopeFactory serviceScopeFactory)
        {
            _serviceScopeFactory = serviceScopeFactory;
            _cpuCounter = new PerformanceCounter("Processor", "% Processor Time", "_Total");
        }

        public Task Execute(IJobExecutionContext context)
        {
            using (IServiceScope serviceScope = _serviceScopeFactory.CreateScope())
            {
                var cpuMetricsRepository = serviceScope.ServiceProvider.GetService<ICpuMetricsRepository>();
                try
                {
                    var cpuUsageInPercents = _cpuCounter.NextValue();
                    var time = TimeSpan.FromSeconds(DateTimeOffset.UtcNow.ToUnixTimeSeconds());
                    Debug.WriteLine($"{time} > {cpuUsageInPercents}");
                    cpuMetricsRepository.Create(new Models.CpuMetricDtO
                    {
                        Value = (int)cpuUsageInPercents,
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

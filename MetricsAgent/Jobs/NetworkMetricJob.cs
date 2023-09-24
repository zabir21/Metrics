using MetricsAgent.Models;
using MetricsAgent.Services;
using Quartz;
using System.Diagnostics;

namespace MetricsAgent.Jobs
{
    public class NetworkMetricJob : IJob
    {
        private PerformanceCounter _networkCounter;
        private IServiceScopeFactory _serviceScopeFactory;

        public NetworkMetricJob(IServiceScopeFactory serviceScopeFactory)
        {
            _serviceScopeFactory = serviceScopeFactory;

            PerformanceCounterCategory performanceCounterCategory = new PerformanceCounterCategory("Network Interface");
            string instance = performanceCounterCategory.GetInstanceNames()[0];
            _networkCounter = new PerformanceCounter("Network Interface", "Bytes Sent/sec", instance);
        }

        public Task Execute(IJobExecutionContext context)
        {
            using (IServiceScope serviceScope = _serviceScopeFactory.CreateScope())
            {
                var netMetricsRepository = serviceScope.ServiceProvider.GetService<INetworkMetricsRepository>();
                try
                {
                    var lan = _networkCounter.NextValue();
                    var time = TimeSpan.FromSeconds(DateTimeOffset.UtcNow.ToUnixTimeSeconds());
                    Debug.WriteLine($"{time} > {lan}");
                    netMetricsRepository.Create(new NetworkMetrics
                    {
                        Value = (int)lan,
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

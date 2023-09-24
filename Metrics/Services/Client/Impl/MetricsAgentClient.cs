using Metrics.Models;
using Metrics.Models.Request;
using Metrics.Models.Response;
using Newtonsoft.Json;

namespace Metrics.Services.Client.Impl
{
    public class MetricsAgentClient : IMetricsAgentClient
    {
        private readonly AgentPool _agentPool;
        private readonly HttpClient _httpClient;
        public MetricsAgentClient(HttpClient httpClient, AgentPool agentPool) 
        {
            _agentPool = agentPool;
            _httpClient = httpClient;
        }


        public CpuMetricsPesponse GetCpuMetrics(CpuMetricsRequest request)
        {
            AgentInfo agentInfo = _agentPool.Get().FirstOrDefault(agent => agent.AgentId == request.AgentId);
            if (agentInfo == null)
            {
                return null;
            }

            string requestStr = $"{agentInfo.AgentAddress}api/metrics/cpu/from/{request.FromTime.ToString("dd\\.hh\\:mm\\:ss")}/to/{request.ToTime.ToString("dd\\.hh\\:mm\\:ss")}";
            HttpRequestMessage httpRequestMessage = new HttpRequestMessage(HttpMethod.Get, requestStr);
            httpRequestMessage.Headers.Add("Accept", "application/json");

            HttpResponseMessage response = _httpClient.Send(httpRequestMessage);
            if (response.IsSuccessStatusCode)
            {
                string responseStr = response.Content.ReadAsStringAsync().Result;
                CpuMetricsPesponse cpuMetricsPesponse =
                    (CpuMetricsPesponse)JsonConvert.DeserializeObject(responseStr, typeof(CpuMetricsPesponse));
                cpuMetricsPesponse.AgentId = request.AgentId;
                return cpuMetricsPesponse;
            }
            else
            {
                throw new Exception("The HTTP status code of the response was not expected");
            }
        }
    }
}

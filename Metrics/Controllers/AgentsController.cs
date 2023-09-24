using Metrics.Models;
using Microsoft.AspNetCore.Mvc;

namespace Metrics.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AgentsController : ControllerBase
    {
        private readonly AgentPool _agentPool;
        public AgentsController(AgentPool agentPool)
        {
            _agentPool = agentPool;
        }

        [HttpPost("register")]
        public IActionResult RegisterAgent([FromBody] AgentInfo agentInfo)
        {
            if (agentInfo != null) 
            {
                _agentPool.Add(agentInfo);
            }
            return Ok();
        }

        [HttpPut("enable/{agentId}")]
        public IActionResult EnableAgentById([FromRoute] int agentId) 
        {
            if (_agentPool.Agents.ContainsKey(agentId))
                _agentPool.Agents[agentId].Enable = true;
            return Ok();
        }

        [HttpPut("disable/{agentId}")]
        public IActionResult DisableAgentById([FromRoute] int agentId) 
        {
            if (_agentPool.Agents.ContainsKey(agentId))
                _agentPool.Agents[agentId].Enable = false;
            return Ok();
        }

        [HttpGet("get")]
        public ActionResult<AgentInfo[]> GetAllAgents()
        {
            return Ok(_agentPool.Get());
        }
    }
}

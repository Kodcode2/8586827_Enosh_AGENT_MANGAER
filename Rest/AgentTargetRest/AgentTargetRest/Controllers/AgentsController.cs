using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using AgentTargetRest.Models;
using AgentTargetRest.Services;
using AgentTargetRest.Dto;

namespace AgentTargetRest.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AgentsController(IAgentService _agentService) : ControllerBase
    {

        [HttpGet("get-agents")]
        public async Task<ActionResult<List<AgentModel>>> GetAgents()
        {
            return Ok(await _agentService.GetAgentsAsync());
        }

        [HttpGet("get-agent{id}")]
        public async Task<ActionResult<AgentModel>> GetAgentModel(long id)
        {
            try
            {              
                return Ok(await _agentService.GetAgentModelAsync(id));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


        [HttpPut("update-agent{id}")]
        public async Task<IActionResult> PutAgentModel(long id, AgentModel agent)
        {
            try
            {
                await _agentService.UpdateAgentAsync(id, agent);
                return Ok(agent);
            } catch (Exception ex)
            {
                return BadRequest(ex.Message);  
            }
        }

        [HttpPost("create-agent")]
        public async Task<ActionResult<AgentModel>> PostAgentModel(AgentDto agentDto)
        {
            try
            {
                await _agentService.PostAgentModel(agentDto);
                return Created("success", agentDto);
            }catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("delete-agent{id}")]
        public async Task<IActionResult> DeleteAgentModel(long id)
        {
            try
            {
                await _agentService.DeleteAgentModelAsync(id);
                return NoContent();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}

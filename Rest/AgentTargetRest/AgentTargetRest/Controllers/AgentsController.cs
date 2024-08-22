﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using AgentTargetRest.Models;
using AgentTargetRest.Services;
using AgentTargetRest.Dto;

namespace AgentTargetRest.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class AgentsController(IAgentService _agentService) : ControllerBase
    {

        [HttpGet]
        public async Task<ActionResult<List<AgentModel>>> GetAgents()
        {
            return Ok(await _agentService.GetAgentsAsync());
        }

        [HttpGet("get-agent/{id}")]
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


        [HttpPut("update-agent/{id}")]
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

        [HttpPost]
        public async Task<ActionResult<IdDto>> PostAgentModel([FromBody] AgentDto agentDto)
        {
            try
            {
               
                return Created("success", await _agentService.CreateAgentModel(agentDto));
              // return CreatedAtAction(nameof(PostAgentModel), agentDto);
            }catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("delete-agent/{id}")]
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

        [HttpPut("{id}/pin")]
        public async Task<ActionResult<TargetModel>> PinAsync(PinDto pinDto, long id)
        {
            try
            {
                return Ok(await _agentService.Pin(pinDto, id));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("{id}/move")]
        public async Task<ActionResult<TargetModel>> MoveAsync(DirectionsDto directions, long id)
        {
            try
            {
                return Ok(await _agentService.MoveAgent(id, directions));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        //public async Task<>
    }
}

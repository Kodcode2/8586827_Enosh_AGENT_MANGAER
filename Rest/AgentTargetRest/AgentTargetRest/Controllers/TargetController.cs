using AgentTargetRest.Dto;
using AgentTargetRest.Models;
using AgentTargetRest.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AgentTargetRest.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TargetController(ITargetService targetService) : ControllerBase
    {

        [HttpGet("get-targets")]
        public async Task<ActionResult<List<TargetModel>>> GetTarget()
        {
            return Ok(await targetService.GetTargetsAsync());
        }

        [HttpGet("get-target{id}")]
        public async Task<ActionResult<TargetModel>> GetTargetModel(long id)
        {
            try
            {
                return Ok(await targetService.GetTargetModelAsync(id));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

       /* [HttpPut("update-target{id}")]
        public async Task<ActionResult<TargetModel>> PutTargetModel(long id, [FromBody] TargetModel target)
        {
            try
            {
                var a = await targetService.UpdateTargetAsync(id, target);
                return Ok(a);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }*/

        [HttpPost("create-target")]
        public async Task<ActionResult<TargetModel>> PostTargetModel(TargetDto targetDto)
        {
            try
            {
                await targetService.PostTargetModel(targetDto);
                return Created("success", targetDto);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("delete-target{id}")]
        public async Task<IActionResult> DeleteTargetModel(long id)
        {
            try
            {
                await targetService.DeleteTargetModelAsync(id);
                return NoContent();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }
    }
}

using AgentTargetRest.Dto;
using AgentTargetRest.Models;
using AgentTargetRest.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AgentTargetRest.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class TargetsController(ITargetService targetService) : ControllerBase
    {

        [HttpGet]
        public async Task<ActionResult<List<TargetModel>>> GetTargetAsync()
        {
            return Ok(await targetService.GetTargetsAsync());
        }

        [HttpGet("get-target/{id}")]
        public async Task<ActionResult<TargetModel>> GetTargetModelAsync(long id)
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

        [HttpPut("update-target/{id}")]
        public async Task<ActionResult<TargetModel>> PutTargetModelAsync(long id, [FromBody] TargetModel target)
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
        }

        [HttpPost]
        public async Task<ActionResult<long>> CreateAsync([FromBody] TargetDto targetDto)
        {
            try
            { 
                var t = await targetService.CreateTargetModel(targetDto);
                return Created("success", t);
            }
            catch (Exception ex)
            {
                var x = 0;
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("delete-target/{id}")]
        public async Task<IActionResult> DeleteTargetModelAsync(long id)
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

        [HttpPut("{id}/pin")]
        public async Task<ActionResult<TargetModel>> PinAsync(PinDto pinDto , long id)
        {
            try
            {
                return Ok(await targetService.PinAsync(pinDto, id));
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
                return Ok(await targetService.MoveTargetAsync(id, directions));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}

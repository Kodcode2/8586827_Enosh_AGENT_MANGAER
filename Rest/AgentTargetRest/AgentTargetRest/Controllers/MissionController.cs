using AgentTargetRest.Models;
using AgentTargetRest.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AgentTargetRest.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MissionController(IMissionService missionService) : ControllerBase
    {
        [HttpPost("update")]
        public async Task<ActionResult> Create(MissionModel mission)
        {
            try
            {
                await missionService.CreateMission(mission);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest();
            }
        }
    }
}

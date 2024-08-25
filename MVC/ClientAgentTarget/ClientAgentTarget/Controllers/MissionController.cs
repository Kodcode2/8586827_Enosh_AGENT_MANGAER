using ClientAgentTarget.Models;
using ClientAgentTarget.Services;
using Microsoft.AspNetCore.Mvc;

namespace ClientAgentTarget.Controllers
{
    public class MissionController(IMissionService missionService, IAgentService agentService, ITargetService targetService) : Controller
    {
        public async Task<IActionResult> Index()
        {
            return View(await missionService.GetAllMissions());
        }

        [HttpGet]
        public async Task<IActionResult> GetAllAgents()
        {
            try
            {
                return View(await agentService.GetAllAgents());
            }
            catch (Exception ex)
            {
                return View(ex.Message);
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetAllTargets()
        {
            return View(await targetService.GetAllTargets());
        }
    }
}

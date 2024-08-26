using ClientAgentTarget.Services;
using Microsoft.AspNetCore.Mvc;

namespace ClientAgentTarget.Controllers
{
    public class AgentController(IAgentService agentService) : Controller
    {
        public async Task<IActionResult> Index()
        {
            return View(await agentService.CreateAgentsVM());
        }

        public async Task<IActionResult> AgentDetails(long agentId)
        {
            return View(await agentService.Details(agentId));
        }
    }
}

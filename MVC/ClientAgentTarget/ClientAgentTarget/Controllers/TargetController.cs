using ClientAgentTarget.Services;
using Microsoft.AspNetCore.Mvc;

namespace ClientAgentTarget.Controllers
{
    public class TargetController(ITargetService targetService) : Controller
    {
        public async Task<IActionResult> Index()
        {
            return View(await targetService.CreateTargetVM());
        }

        public async Task<IActionResult> TargetDetails(long targetId)
        {
            return View(await targetService.Details(targetId));
        }
    }
}

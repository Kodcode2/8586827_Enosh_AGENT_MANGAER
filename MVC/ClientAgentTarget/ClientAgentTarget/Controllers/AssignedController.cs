using ClientAgentTarget.Services;
using ClientAgentTarget.ViewModel;
using Microsoft.AspNetCore.Mvc;

namespace ClientAgentTarget.Controllers
{
    public class AssignedController(IGeneralService generalService) : Controller
    {
        public async Task<IActionResult> Index()
        {
            return View(await generalService.CreateAssigned());
        }
        
        public async Task<IActionResult> Confirm(long MissionId)
        {
            var conf = await generalService.ConfirmedAssinged(MissionId);
            if(conf)
            {
                return RedirectToAction("Index");
            }
            return RedirectToAction("Index", "Mission");
        }

        public async Task<IActionResult> MissionDetails(long MissionId)
        {
            return View(await generalService.Details(MissionId));
        }
    }
}

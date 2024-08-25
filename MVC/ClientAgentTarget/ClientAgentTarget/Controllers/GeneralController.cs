using ClientAgentTarget.Services;
using Microsoft.AspNetCore.Mvc;

namespace ClientAgentTarget.Controllers
{
    public class GeneralController(IGeneralService generalService) : Controller
    {
        public async Task<IActionResult> Index()
        {
            return View(await generalService.CreateGeneralTable());
        }
    }
}

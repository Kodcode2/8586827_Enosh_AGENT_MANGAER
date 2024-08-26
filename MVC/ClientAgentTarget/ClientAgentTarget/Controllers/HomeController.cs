using ClientAgentTarget.Models;
using ClientAgentTarget.Services;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace ClientAgentTarget.Controllers
{
    public class HomeController(IMatrixService matrixService) : Controller
    {
        public async Task<IActionResult> Index()
        {
            return View(await matrixService.InitMatrix());
        }
    }
}

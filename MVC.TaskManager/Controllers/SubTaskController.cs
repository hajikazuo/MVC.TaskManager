using Microsoft.AspNetCore.Mvc;

namespace MVC.TaskManager.Controllers
{
    public class SubTaskController : Controller
    {
        public async Task<IActionResult> Index()
        {
            return View();
        }

        public async Task<IActionResult> Edit(int id)
        {
            return View();
        }
    }
}

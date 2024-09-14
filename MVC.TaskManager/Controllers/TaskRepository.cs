using Microsoft.AspNetCore.Mvc;

namespace MVC.TaskManager.Controllers
{
    public class TaskRepository : Controller
    {
        public IActionResult Index()
        {
            return View();
        }


    }
}

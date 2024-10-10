using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using MVC.TaskManager.Models.Users;
using MVC.TaskManager.Repositories.Implementation;
using MVC.TaskManager.Repositories.Interface;

namespace MVC.TaskManager.Controllers
{
    public class SubTaskController : Controller
    {
        private readonly ISubTaskRepository _subTaskRepository;
        private readonly UserManager<User> _userManager;

        public SubTaskController(ISubTaskRepository subTaskRepository)
        {
            _subTaskRepository = subTaskRepository;
        }

        public async Task<IActionResult> Index()
        {
            if (!User.Identity.IsAuthenticated)
            {
                return View("Login");
            }

            var user = await _userManager.GetUserAsync(User);

            var subtasks = await _subTaskRepository.GetAllAsync(user);
            return View(subtasks);
        }

        public async Task<IActionResult> Edit(int id)
        {
            return View();
        }
    }
}

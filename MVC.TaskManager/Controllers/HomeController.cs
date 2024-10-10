using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using MVC.TaskManager.Models;
using MVC.TaskManager.Models.Users;
using MVC.TaskManager.Repositories.Interface;
using MVC.TaskManager.ViewModels;
using System.Diagnostics;

namespace MVC.TaskManager.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private readonly ITaskItemRepository _taskItemRepository;
        private readonly ISubTaskRepository _subTaskRepository;
        private readonly UserManager<User> _userManager;

        public HomeController(ITaskItemRepository taskItemRepository, ISubTaskRepository subTaskRepository, UserManager<User> userManager)
        {
            _taskItemRepository = taskItemRepository;
            _subTaskRepository = subTaskRepository;
            _userManager = userManager;
        }

        public async Task<IActionResult> Index()
        {
            if (!User.Identity.IsAuthenticated)
            {
                return View("Login");
            }

            var user = await _userManager.GetUserAsync(User);

            var taskItems = await _taskItemRepository.GetAllAsync(user);

            var subtasks = await _subTaskRepository.GetAllAsync(user);

            var data = new HomeViewModel
            {
                TaskItems = taskItems,
                SubTasks = subtasks
            };

            return View(data);
        }

    }
}

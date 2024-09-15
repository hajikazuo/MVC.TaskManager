using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using MVC.TaskManager.Extensions;
using MVC.TaskManager.Models;
using MVC.TaskManager.Models.Enums;
using MVC.TaskManager.Repositories.Interface;

namespace MVC.TaskManager.Controllers
{
    public class TaskItemController : Controller
    {
        private readonly IUserRepository _userRepository;
        private readonly ITaskItemRepository _taskItemRepository;

        public TaskItemController(IUserRepository userRepository, ITaskItemRepository taskItemRepository)
        {
            _userRepository = userRepository;
            _taskItemRepository = taskItemRepository;
        }

        public async Task <IActionResult> Index()
        {
            var taskItems = await _taskItemRepository.GetAllAsync();
            return View(taskItems);
        }

        public async Task<IActionResult> Create()
        {
            var users = await _userRepository.GetAllUsersAsync();
            ViewData["UserId"] = new SelectList(users, "Id", "Id");
            ViewData["Status"] = this.AssembleSelectListToEnum(new Status());
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(TaskItem task)
        {
            var taskItem = await _taskItemRepository.CreateAsync(task);

            var users = await _userRepository.GetAllUsersAsync();
            ViewData["UserId"] = new SelectList(users, "Id", "Id");
            ViewData["Status"] = this.AssembleSelectListToEnum(new Status());
            return RedirectToAction(nameof(Index));
        }
    }
}

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using MVC.TaskManager.Extensions;
using MVC.TaskManager.Models;
using MVC.TaskManager.Models.Enums;
using MVC.TaskManager.Repositories.Interface;
using MVC.TaskManager.ViewModels;

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
        public async Task<IActionResult> Create(TaskRegistrationViewModel task)
        {
            if (ModelState.IsValid)
            {
                var taskItem = new TaskItem
                {
                    Name = task.Name,
                    Description = task.Description,
                    DueDate = task.DueDate,
                    IsComplete = task.IsComplete,
                    Status = task.Status,
                    UserId = task.UserId,
                    SubTasks = task.SubTasks,
                };

                taskItem = await _taskItemRepository.CreateAsync(taskItem);
            }

            var users = await _userRepository.GetAllUsersAsync();
            ViewData["UserId"] = new SelectList(users, "Id", "Id");
            ViewData["Status"] = this.AssembleSelectListToEnum(new Status());
            return RedirectToAction(nameof(Index));
        }

        [ResponseCache(NoStore = true, Duration = 0)]
        public async Task<IActionResult> NewSubTask()
        {
            var users = await _userRepository.GetAllUsersAsync();
            ViewData["UserId"] = new SelectList(users, "Id", "Id");
            ViewData["Status"] = this.AssembleSelectListToEnum(new Status());
            return PartialView("SubTasks", new SubTask());
        }
    }
}

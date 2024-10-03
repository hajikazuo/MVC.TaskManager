using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
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

        public async Task<IActionResult> Details(Guid id)
        {
            var taskItem = await _taskItemRepository.GetByIdAsync(id);
            return View(taskItem);
        }

        public async Task<IActionResult> Create()
        {
            var users = await _userRepository.GetAllUsersAsync();
            ViewData["UserId"] = new SelectList(users, "Id", "CompleteName");
            ViewData["Status"] = this.AssembleSelectListToEnum(new Status());
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(TaskItemViewModel task)
        {
            if (ModelState.IsValid)
            {
                var taskItem = new TaskItem
                {
                    Name = task.Name,
                    Description = task.Description,
                    DueDate = task.DueDate,
                    Status = task.Status,
                    UserId = task.UserId,
                    SubTasks = task.SubTasks,
                };

                taskItem = await _taskItemRepository.CreateAsync(taskItem);
            }

            var users = await _userRepository.GetAllUsersAsync();
            ViewData["UserId"] = new SelectList(users, "Id", "CompleteName");
            ViewData["Status"] = this.AssembleSelectListToEnum(new Status());
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Edit(Guid id)
        {
            var taskItem = await _taskItemRepository.GetByIdAsync(id);

            if (taskItem == null)
            {
                return NotFound();
            }

            var task = new TaskItemViewModel
            {
                TaskItemId = taskItem.TaskItemId,
                Name = taskItem.Name,
                Description = taskItem.Description,
                DueDate = taskItem.DueDate,
                Status = taskItem.Status,
                UserId = taskItem.UserId,
                SubTasks = taskItem.SubTasks
            };

            var users = await _userRepository.GetAllUsersAsync();
            ViewData["UserId"] = new SelectList(users, "Id", "CompleteName");
            ViewData["Status"] = this.AssembleSelectListToEnum(new Status());
            return View(task);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(Guid id, TaskItemViewModel taskItem)
        {
            if (id != taskItem.TaskItemId)
            {
                return NotFound();
            }

            var task = new TaskItem
            {
                TaskItemId = taskItem.TaskItemId,
                Name = taskItem.Name,
                Description = taskItem.Description,
                DueDate = taskItem.DueDate,
                Status = taskItem.Status,
                UserId = taskItem.UserId,
                SubTasks = taskItem.SubTasks,
            };

            if (ModelState.IsValid)
            {
                await _taskItemRepository.UpdateAsync(task);
            }

            var users = await _userRepository.GetAllUsersAsync();
            ViewData["UserId"] = new SelectList(users, "Id", "UserName");
            ViewData["Status"] = this.AssembleSelectListToEnum(new Status());
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Delete(Guid id)
        {
            var taskItem = await _taskItemRepository.GetByIdAsync(id);
            if (taskItem == null)
            {
                return NotFound();
            }

            return View(taskItem);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var taskItem = await _taskItemRepository.DeleteAsync(id);

            if(taskItem == null)
            {
                return NotFound();
            }

            return RedirectToAction(nameof(Index));
        }


        [ResponseCache(NoStore = true, Duration = 0)]
        public async Task<IActionResult> NewSubTask()
        {
            var users = await _userRepository.GetAllUsersAsync();
            ViewData["UserId"] = new SelectList(users, "Id", "UserName");
            ViewData["Status"] = this.AssembleSelectListToEnum(new Status());
            return PartialView("SubTasks", new SubTask());
        }
    }
}

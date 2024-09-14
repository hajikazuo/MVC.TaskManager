using MVC.TaskManager.Models;
using MVC.TaskManager.Models.Users;

namespace MVC.TaskManager.Repositories.Interface
{
    public interface ITaskItemRepository
    {
        Task<IEnumerable<TaskItem>> GetAllAsync();
        Task<TaskItem> CreateAsync(TaskItem taskItem);
    }
}

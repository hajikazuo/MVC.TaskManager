using MVC.TaskManager.Models;

namespace MVC.TaskManager.Repositories.Interface
{
    public interface ITaskItemRepository
    {
        Task<TaskItem> CreateAsync(TaskItem taskItem);
    }
}

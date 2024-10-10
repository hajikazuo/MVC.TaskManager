using MVC.TaskManager.Models;
using MVC.TaskManager.Models.Users;

namespace MVC.TaskManager.Repositories.Interface
{
    public interface ITaskItemRepository
    {
        Task<IEnumerable<TaskItem>> GetAllAsync(User user);
        Task<TaskItem> GetByIdAsync(Guid id);
        Task<TaskItem> CreateAsync(TaskItem taskItem);
        Task<TaskItem> UpdateAsync(TaskItem taskItem);
        Task<TaskItem> DeleteAsync(Guid id);
    }
}

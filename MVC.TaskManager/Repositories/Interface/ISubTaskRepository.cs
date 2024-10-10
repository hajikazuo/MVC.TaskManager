using MVC.TaskManager.Models.Users;
using MVC.TaskManager.Models;

namespace MVC.TaskManager.Repositories.Interface
{
    public interface ISubTaskRepository
    {
        Task<IEnumerable<SubTask>> GetAllAsync(User user);
        Task<SubTask> GetByIdAsync(int id);
        Task<SubTask> UpdateAsync(SubTask subTaskItem);
    }
}

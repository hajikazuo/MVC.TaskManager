using MVC.TaskManager.Models.Users;

namespace MVC.TaskManager.Repositories.Interface
{
    public interface IUserRepository
    {
        Task <IEnumerable<User>> GetAllUsersAsync();
    }
}

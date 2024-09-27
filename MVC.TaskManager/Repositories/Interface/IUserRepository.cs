using Microsoft.AspNetCore.Identity;
using MVC.TaskManager.Models.Users;

namespace MVC.TaskManager.Repositories.Interface
{
    public interface IUserRepository
    {
        Task <List<User>> GetAllUsersAsync();
        Task<User> GetUserByIdAsync(Guid id);
        Task<IdentityResult> CreateAsync(User user, string password);
        Task<IdentityResult> UpdateAsync(User user);
        Task<IdentityResult> DeleteAsync(User user);
        Task<string> GetUserRoleAsync(User user);
        Task<List<IdentityRole<Guid>>> GetAllRolesAsync();
    }
}

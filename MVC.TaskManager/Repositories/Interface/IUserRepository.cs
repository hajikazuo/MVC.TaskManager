using Microsoft.AspNetCore.Identity;
using MVC.TaskManager.Models.Users;
using MVC.TaskManager.ViewModels.AccountViewModel;

namespace MVC.TaskManager.Repositories.Interface
{
    public interface IUserRepository
    {
        Task <List<User>> GetAllUsersAsync();
        Task<User> GetUserByIdAsync(Guid id);
        Task<IdentityResult> CreateAsync(User user, string password);
        Task<IdentityResult> UpdateAsync(User user);
        Task<IdentityResult> DeleteAsync(User user);
        Task<IdentityResult> RemovePasswordAsync(User user);
        Task<IdentityResult> AddPasswordAsync(User user, string newPassword);

        Task<string> GetUserRoleAsync(User user);
        Task<List<SelectGenericListViewModel>> GetAllRolesAsync();
        Task RemoveUserRolesAsync(User user);
        Task AddUserRoleAsync(User user, string role);

        Task<SignInResult> LoginAsync(LoginViewModel model);
        Task LogoutAsync();
    }
}

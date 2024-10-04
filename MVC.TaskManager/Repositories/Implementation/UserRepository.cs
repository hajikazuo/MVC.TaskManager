using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using MVC.TaskManager.Data;
using MVC.TaskManager.Models.Users;
using MVC.TaskManager.Repositories.Interface;
using MVC.TaskManager.ViewModels.AccountViewModel;

namespace MVC.TaskManager.Repositories.Implementation
{
    public class UserRepository : IUserRepository
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly AppDbContext _context;

        public UserRepository(AppDbContext context, UserManager<User> userManager, SignInManager<User> signInManager)
        {
            _context = context;
            _userManager = userManager;
            _signInManager = signInManager;
        }

        public async Task<List<User>> GetAllUsersAsync()
        {
            return  await _context.Users.ToListAsync();
        }

        public async Task<User> GetUserByIdAsync(Guid id)
        {
            return await _context.Users.FirstOrDefaultAsync(u => u.Id == id);
        }

        public async Task<IdentityResult> CreateAsync(User user, string password)
        {
            return await _userManager.CreateAsync(user, password);  
        }
        public async Task<IdentityResult> UpdateAsync(User user)
        {
            return await _userManager.UpdateAsync(user);
        }

        public async Task<IdentityResult> DeleteAsync(User user)
        {
            var taskItems = await _context.TaskItems.Where(t => t.UserId == user.Id).ToListAsync();
            foreach (var task in taskItems)
            {
                task.UserId = null;
            }

            var subTasks = await _context.SubTasks.Where(st => st.UserId == user.Id).ToListAsync();
            foreach (var subTask in subTasks)
            {
                subTask.UserId = null; 
            }

            await _context.SaveChangesAsync();

            return await _userManager.DeleteAsync(user);
        }

        public async Task<IdentityResult> RemovePasswordAsync(User user)
        {
            return await _userManager.RemovePasswordAsync(user);
        }

        public async Task<IdentityResult> AddPasswordAsync(User user, string newPassword)
        {
            return await _userManager.AddPasswordAsync(user, newPassword);
        }

        public async Task<string> GetUserRoleAsync(User user)
        {
            var roles = await _userManager.GetRolesAsync(user);
            return roles?.FirstOrDefault() ?? string.Empty;
        }

        public async Task<List<SelectGenericListViewModel>> GetAllRolesAsync()
        {
            return await _context.Roles.Select(role => new SelectGenericListViewModel
            {
                Id = role.Name,
                Name = role.Name
            }).ToListAsync();
        }

        public async Task RemoveUserRolesAsync(User user)
        {
            var userRoles = await _userManager.GetRolesAsync(user);
            await _userManager.RemoveFromRolesAsync(user, userRoles);
        }

        public async Task AddUserRoleAsync(User user, string role)
        {
            await _userManager.AddToRoleAsync(user, role);
        }

        public async Task<SignInResult> LoginAsync(LoginViewModel model)
        {
            return await _signInManager.PasswordSignInAsync(model.Email, model.Password, model.RememberMe, lockoutOnFailure: false);
        }
        public async Task LogoutAsync()
        {
            await _signInManager.SignOutAsync();
        }
    }
}

using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using MVC.TaskManager.Data;
using MVC.TaskManager.Models.Users;
using MVC.TaskManager.Repositories.Interface;

namespace MVC.TaskManager.Repositories.Implementation
{
    public class UserRepository : IUserRepository
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly AppDbContext _context;

        public UserRepository(AppDbContext context, UserManager<User> userManager)
        {
            _context = context;
            _userManager = userManager;
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
        public Task<IdentityResult> UpdateAsync(User user)
        {
            return _userManager.UpdateAsync(user);
        }

        public Task<IdentityResult> DeleteAsync(User user)
        {
            return _userManager.DeleteAsync(user);
        }

        public async Task<string> GetUserRoleAsync(User user)
        {
            var roles = await _userManager.GetRolesAsync(user);
            return roles?.FirstOrDefault() ?? string.Empty;
        }

        public async Task<List<IdentityRole<Guid>>> GetAllRolesAsync()
        {
            var roles = await _context.Roles.ToListAsync();
            return roles.Select(role => new IdentityRole<Guid>
            {
                Id = role.Id,       
                Name = role.Name    
            }).ToList();
        }

    }
}

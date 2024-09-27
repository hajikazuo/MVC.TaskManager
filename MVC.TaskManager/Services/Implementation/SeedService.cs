using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using MVC.TaskManager.Data;
using MVC.TaskManager.Models;
using MVC.TaskManager.Models.Users;
using MVC.TaskManager.Services.Interface;
using System;
using System.Data;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace MVC.TaskManager.Services.Implementation
{
    public class SeedService : ISeedService
    {
        private readonly AppDbContext _context;
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<Role> _roleManager;

        private const string AdminRole = "Admin";
        private const string TeamManagerRole = "TeamManager";
        private const string UserRole = "User";

        private Guid UserGuid = new Guid("3ec5061e-c0fc-4229-8d39-d407f7991bf8");

        public SeedService(AppDbContext context, UserManager<User> userManager, RoleManager<Role> roleManager)
        {
            _context = context;
            _userManager = userManager;
            _roleManager = roleManager;
        }

        public void Seed() 
        {
            CreateRole(AdminRole).GetAwaiter().GetResult();
            CreateRole(TeamManagerRole).GetAwaiter().GetResult();
            CreateRole(UserRole).GetAwaiter().GetResult();
            CreateUser("admin@admin.com", "Admin", "Teste@2024", UserGuid, role: AdminRole).GetAwaiter().GetResult();
            CreateTaskItem().GetAwaiter().GetResult();
        }

        private async Task<IdentityResult> CreateRole(string roleName)
        {
            if (!await _roleManager.RoleExistsAsync(roleName))
            {
                var role = new Role
                {
                    Name = roleName
                };
                return await _roleManager.CreateAsync(role);
            }
            return default;
        }

        private async Task<IdentityResult> CreateUser(string email, string username, string password, Guid userId, string role)
        {
            var userReturn = await _userManager.FindByEmailAsync(email);
            if (userReturn == null)
            {
                User user = new User();
                user.Id = userId;
                user.UserName = username;
                user.Email = email;
                user.EmailConfirmed = true;

                IdentityResult result = await _userManager.CreateAsync(user, password);

                if (result.Succeeded)
                {
                    _userManager.AddToRoleAsync(user, role).Wait();
                }

                return result;
            }
            else
            {
                return default;
            }
        }

        private async Task CreateTaskItem()
        {
            var exists = await _context.TaskItems.AnyAsync();
            if (exists != true)
            {
                var entity = new TaskItem
                {
                    Name = "Task example",
                    Description = "A some task",
                    DueDate = DateTime.Now,
                    Status = Models.Enums.Status.Pending,
                    UserId = UserGuid
                };
                _context.TaskItems?.Add(entity);
                await _context.SaveChangesAsync();
            }
        }
    }
}

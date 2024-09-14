using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using MVC.TaskManager.Data;
using MVC.TaskManager.Models;
using MVC.TaskManager.Models.Users;
using MVC.TaskManager.Services.Interface;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace MVC.TaskManager.Services.Implementation
{
    public class SeedService : ISeedService
    {
        private readonly AppDbContext _context;
        private readonly UserManager<User> _userManager;

        private Guid UserGuid = new Guid("3ec5061e-c0fc-4229-8d39-d407f7991bf8");

        public SeedService(AppDbContext context, UserManager<User> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public void Seed() 
        {
            CreateUser("admin@admin.com", "Admin", "Teste@2024", UserGuid).GetAwaiter().GetResult();
            CreateTaskItem().GetAwaiter().GetResult();
        }

        private async Task<IdentityResult> CreateUser(string email, string username, string password, Guid userId)
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
                    IsComplete = false,
                    Status = Models.Enums.Status.Pending,
                    UserId = UserGuid
                };
                _context.TaskItems?.Add(entity);
                await _context.SaveChangesAsync();
            }
        }
    }
}

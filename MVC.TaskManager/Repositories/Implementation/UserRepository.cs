using Microsoft.EntityFrameworkCore;
using MVC.TaskManager.Data;
using MVC.TaskManager.Models.Users;
using MVC.TaskManager.Repositories.Interface;

namespace MVC.TaskManager.Repositories.Implementation
{
    public class UserRepository : IUserRepository
    {
        private readonly AppDbContext _context;

        public UserRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<User>> GetAllUsersAsync()
        {
            return  await _context.Users.ToListAsync();
        }
    }
}

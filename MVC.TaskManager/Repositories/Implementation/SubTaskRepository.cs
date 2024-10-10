using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using MVC.TaskManager.Data;
using MVC.TaskManager.Models;
using MVC.TaskManager.Models.Users;
using MVC.TaskManager.Repositories.Interface;

namespace MVC.TaskManager.Repositories.Implementation
{
    public class SubTaskRepository : ISubTaskRepository
    {
        private readonly AppDbContext _context;
        UserManager<User> _userManager;

        public SubTaskRepository(AppDbContext context, UserManager<User> userManager)
        {
            _context = context;
            _userManager = userManager;
        }
        public async Task<IEnumerable<SubTask>> GetAllAsync(User user)
        {
            if (await _userManager.IsInRoleAsync(user, "Admin"))
            {
                return await _context.SubTasks
                .Include(u => u.User)
                .ToListAsync();
            }
            else
            {
                return await _context.SubTasks
                .Include(t => t.User)
                .Where(t => t.UserId == user.Id)
                .ToListAsync();
            }
        }

        public async Task<SubTask> GetByIdAsync(int id)
        {
            var existingSubTask = await _context.SubTasks
                .Include(u => u.User)
                .FirstOrDefaultAsync(x => x.SubTaskId == id);

            if (existingSubTask == null)
            {
                return null;
            }

            return existingSubTask;
        }

        public async Task<SubTask> UpdateAsync(SubTask subTaskItem)
        {
            var existingSubTask = await _context.SubTasks
                .Include(u => u.User)
                .FirstOrDefaultAsync(x => x.SubTaskId == subTaskItem.SubTaskId);

            if (existingSubTask == null)
            {
                return null;
            }

            _context.Entry(existingSubTask).CurrentValues.SetValues(subTaskItem);

            await _context.SaveChangesAsync();
            return existingSubTask;
        }
    }
}

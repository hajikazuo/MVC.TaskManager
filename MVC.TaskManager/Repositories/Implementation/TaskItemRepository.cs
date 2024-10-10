using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using MVC.TaskManager.Data;
using MVC.TaskManager.Models;
using MVC.TaskManager.Models.Users;
using MVC.TaskManager.Repositories.Interface;

namespace MVC.TaskManager.Repositories.Implementation
{
    public class TaskItemRepository : ITaskItemRepository
    {
        private readonly AppDbContext _context;
        UserManager<User> _userManager;

        public TaskItemRepository(AppDbContext context, UserManager<User> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public async Task<IEnumerable<TaskItem>> GetAllAsync(User user)
        {
            if (await _userManager.IsInRoleAsync(user, "Admin"))
            {
                return await _context.TaskItems
                .Include(u => u.User)
                .ToListAsync();
            }else
            {
                return await _context.TaskItems
                .Include(t => t.User)
                .Where(t => t.UserId == user.Id)
                .ToListAsync();
            }
        }

        public async Task<TaskItem> GetByIdAsync(Guid id)
        {
            var existingTask = await _context.TaskItems
                .Include(u => u.User)
                .Include(s => s.SubTasks)
                .ThenInclude(subTask => subTask.User)
                .FirstOrDefaultAsync(x => x.TaskItemId == id);

            if (existingTask == null)
            {
                return null;
            }

            return existingTask;
        }

        public async Task<TaskItem> CreateAsync(TaskItem taskItem)
        {
            await _context.TaskItems.AddAsync(taskItem);
            await _context.SaveChangesAsync();
            return taskItem;
        }

        public async Task<TaskItem> UpdateAsync(TaskItem taskItem)
        {
            var existingTask = await _context.TaskItems
                .Include(u => u.User)
                .Include(s => s.SubTasks)
                .FirstOrDefaultAsync(x => x.TaskItemId == taskItem.TaskItemId);

            if (existingTask == null)
            {
                return null;
            }

            _context.Entry(existingTask).CurrentValues.SetValues(taskItem);
            existingTask.SubTasks = taskItem.SubTasks;

            await _context.SaveChangesAsync();
            return existingTask;    
        }

        public async Task<TaskItem> DeleteAsync(Guid id)
        {
            var existingTask = _context.TaskItems.Include(t => t.SubTasks).FirstOrDefault(x => x.TaskItemId == id);

            if (existingTask != null)
            {
                _context.SubTasks.RemoveRange(existingTask.SubTasks);

                _context.TaskItems.Remove(existingTask);
                await _context.SaveChangesAsync();
                return existingTask;
            }

            return null;
        }
    }
}

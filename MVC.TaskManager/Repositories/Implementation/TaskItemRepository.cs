using Microsoft.EntityFrameworkCore;
using MVC.TaskManager.Data;
using MVC.TaskManager.Models;
using MVC.TaskManager.Repositories.Interface;

namespace MVC.TaskManager.Repositories.Implementation
{
    public class TaskItemRepository : ITaskItemRepository
    {
        private readonly AppDbContext _context;

        public TaskItemRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<TaskItem>> GetAllAsync()
        {
            return await _context.TaskItems
                .Include(u => u.User)
                .ToListAsync();
        }

        public async Task<TaskItem> GetByIdAsync(Guid id)
        {
            var existingTask = await _context.TaskItems
                .Include(u => u.User)
                .Include(s => s.SubTasks)
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
    }
}

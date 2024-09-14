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

        public async Task<TaskItem> CreateAsync(TaskItem taskItem)
        {
            await _context.TaskItems.AddAsync(taskItem);
            await _context.SaveChangesAsync();
            return taskItem;
        }
    }
}

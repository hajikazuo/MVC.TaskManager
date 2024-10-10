using MVC.TaskManager.Models;

namespace MVC.TaskManager.ViewModels
{
    public class HomeViewModel
    {
        public IEnumerable<TaskItem>? TaskItems { get; set; }
        public IEnumerable<SubTask>? SubTasks { get; set; }
    }
}

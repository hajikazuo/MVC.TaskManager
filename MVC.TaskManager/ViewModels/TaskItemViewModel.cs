using MVC.TaskManager.Models;
using MVC.TaskManager.Models.Enums;
using System.ComponentModel.DataAnnotations;

namespace MVC.TaskManager.ViewModels
{
    public class TaskItemViewModel
    {
        public Guid TaskItemId { get; set; }

        public string Name { get; set; }
        public string? Description { get; set; }

        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd HH:mm}", ApplyFormatInEditMode = true)]
        public DateTime DueDate { get; set; }
        public bool IsComplete { get; set; }
        public Status Status { get; set; }

        public Guid? UserId { get; set; }
        public List<SubTask> SubTasks { get; set; } = new List<SubTask>();
    }
}

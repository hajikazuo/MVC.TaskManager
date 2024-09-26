using MVC.TaskManager.Models.Enums;
using MVC.TaskManager.Models.Users;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace MVC.TaskManager.Models
{
    public class TaskItem
    {
        public Guid TaskItemId { get; set; }

        public string Name { get; set; }
        public string? Description { get; set; }

        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd HH:mm}", ApplyFormatInEditMode = true)]
        public DateTime DueDate { get; set; }
        public Status Status { get; set; }

        public Guid? UserId { get; set; }
        public virtual User? User { get; set; }

        public List<SubTask> SubTasks { get; set; } = new List<SubTask>();

        public void AddSubTask(SubTask subTask)
        {
            SubTasks.Add(subTask); 
        }
    }
}

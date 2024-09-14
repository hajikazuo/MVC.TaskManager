using MVC.TaskManager.Models.Enums;
using MVC.TaskManager.Models.Users;
using System.ComponentModel.DataAnnotations;

namespace MVC.TaskManager.Models
{
    public class SubTask
    {
        public Guid SubTaskId { get; set; }

        public required string Name { get; set; }
        public string? Description { get; set; }

        public DateTime DueDate { get; set; }
        public bool IsComplete { get; set; }
        public Status Status { get; set; }

        public Guid? UserId { get; set; }
        public Guid? TaskItemId { get; set; }

        public virtual User? User { get; set; } 
        public virtual TaskItem? TaskItem { get; set; }
    }
}

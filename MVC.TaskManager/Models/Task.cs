using MVC.TaskManager.Models.Enums;
using MVC.TaskManager.Models.Users;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace MVC.TaskManager.Models
{
    public class Task
    {
        public Guid TaskId { get; set; }

        public required string Name { get; set; }
        public string? Description { get; set; }

        public DateTime DueDate { get; set; }
        public bool IsComplete { get; set; }
        public Status Status { get; set; }

        public Guid UserId { get; set; }
        public virtual User? User { get; set; }

        [JsonIgnore]
        public virtual ICollection<SubTask>? SubTasks { get; set; } = new List<SubTask>();

    }
}

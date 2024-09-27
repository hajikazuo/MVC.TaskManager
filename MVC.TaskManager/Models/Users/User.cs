using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace MVC.TaskManager.Models.Users
{
    public class User : IdentityUser<Guid>
    {
        [MaxLength(100)]
        [Required]
        public string CompleteName { get; set; }

        [MaxLength(100)]
        public string? Image { get; set; }

        [JsonIgnore]
        public virtual ICollection<TaskItem>? TaskItems { get; set; }

        [JsonIgnore]
        public virtual ICollection<SubTask>? SubTasks { get; set; }
    }
}

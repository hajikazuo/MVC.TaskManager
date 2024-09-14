using Microsoft.AspNetCore.Identity;
using System.Text.Json.Serialization;

namespace MVC.TaskManager.Models.Users
{
    public class User : IdentityUser<Guid>
    {

        [JsonIgnore]
        public virtual ICollection<TaskItem>? TaskItems { get; set; }

        [JsonIgnore]
        public virtual ICollection<SubTask>? SubTasks { get; set; }
    }
}

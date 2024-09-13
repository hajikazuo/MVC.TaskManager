using Microsoft.AspNetCore.Identity;
using System.Text.Json.Serialization;

namespace MVC.TaskManager.Models.Users
{
    public class User : IdentityUser<Guid>
    {

        [JsonIgnore]
        public virtual ICollection<Task>? Tasks { get; set; }

        [JsonIgnore]
        public virtual ICollection<SubTask>? SubTasks { get; set; }
    }
}

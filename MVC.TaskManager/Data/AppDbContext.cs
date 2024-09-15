using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using MVC.TaskManager.Models;
using MVC.TaskManager.Models.Users;

namespace MVC.TaskManager.Data
{
    public class AppDbContext : IdentityDbContext<User, Role, Guid>
    {
        public AppDbContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<TaskItem> TaskItems { get; set; }
        public DbSet<SubTask> SubTasks { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<SubTask>(entity =>
            {
                entity.HasOne(x => x.User)
                      .WithMany(x => x.SubTasks)
                      .HasForeignKey(f => f.UserId)
                      .OnDelete(DeleteBehavior.SetNull);
            });
        }
    }
}

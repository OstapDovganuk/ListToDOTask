using Microsoft.EntityFrameworkCore;

namespace TODOHTTPApi.Models
{
    public class TaskContext : DbContext
    {
        public DbSet<ToDoTask> Tasks { get; set; }
        public TaskContext(DbContextOptions<TaskContext> options)
            : base(options)
        {
            Database.EnsureCreated();
        }
    }
}

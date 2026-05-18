using Microsoft.EntityFrameworkCore;
using TaskManagerAPI.Models;

namespace TaskManagerAPI.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<TaskItem> Tasks => Set<TaskItem>();
        public DbSet<User> Users => Set<User>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>().HasData(
                new User { Id = 1, Username = "admin", Password = "admin123" }
            );

            var created1 = new DateTime(2024, 01, 01, 0, 0, 0, DateTimeKind.Utc);
            var created2 = new DateTime(2024, 01, 02, 0, 0, 0, DateTimeKind.Utc);

            modelBuilder.Entity<TaskItem>().HasData(
                new TaskItem
                {
                    Id = 1,
                    Title = "Complete project setup",
                    Description = "Set up .NET and Angular projects",
                    Priority = "High",
                    IsCompleted = false,
                    CreatedAt = created1
                },
                new TaskItem
                {
                    Id = 2,
                    Title = "Design database schema",
                    Description = "Create tables for tasks and users",
                    Priority = "High",
                    IsCompleted = true,
                    CreatedAt = created2
                }
            );
        }
    }
}
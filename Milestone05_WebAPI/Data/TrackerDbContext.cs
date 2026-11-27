using Microsoft.EntityFrameworkCore;
using StudyTaskTracker.WebApi.Models;

namespace StudyTaskTracker.WebApi.Data;

internal sealed class TrackerDbContext(DbContextOptions<TrackerDbContext> options) : DbContext(options)
{
    public DbSet<StudyTask> StudyTasks => Set<StudyTask>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        var users = modelBuilder.Entity<User>();
        users.ToTable("Users");
        users.HasKey(user => user.Id);
        users.Property(user => user.Username).IsRequired();
        users.HasData(new User { Id = User.DemoId, Username = "demo" });

        var tasks = modelBuilder.Entity<StudyTask>();
        tasks.ToTable("StudyTasks");
        tasks.HasKey(task => task.Id);
        tasks.Property(task => task.Title).IsRequired();
        tasks.Property(task => task.Course).IsRequired();
        tasks.Property(task => task.DueDate);
        tasks.Property(task => task.OwnerId);
        tasks.HasOne<User>()
            .WithMany()
            .HasForeignKey(task => task.OwnerId)
            .IsRequired()
            .OnDelete(DeleteBehavior.Restrict);
        tasks.HasIndex(task => new { task.OwnerId, task.DueDate });
    }
}

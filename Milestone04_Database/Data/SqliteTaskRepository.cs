using Microsoft.EntityFrameworkCore;
using StudyTaskTracker.Database.Models;

namespace StudyTaskTracker.Database.Data;

internal sealed class SqliteTaskRepository(TrackerDbContext database) : ITaskRepository
{
    public IReadOnlyList<StudyTask> ListForOwner(int ownerId)
    {
        return database.StudyTasks.AsNoTracking()
            .Where(task => task.OwnerId == ownerId)
            .OrderBy(task => task.Id)
            .ToList();
    }

    public StudyTask? FindForOwner(int ownerId, int id)
    {
        return database.StudyTasks.SingleOrDefault(task => task.OwnerId == ownerId && task.Id == id);
    }

    public void Add(StudyTask task)
    {
        database.StudyTasks.Add(task);
        database.SaveChanges();
    }

    public void Delete(StudyTask task)
    {
        database.StudyTasks.Remove(task);
        database.SaveChanges();
    }

    public void SaveChanges()
    {
        database.SaveChanges();
    }
}

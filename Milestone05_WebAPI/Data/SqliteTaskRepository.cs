using Microsoft.EntityFrameworkCore;
using StudyTaskTracker.WebApi.Models;

namespace StudyTaskTracker.WebApi.Data;

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

    public bool Delete(StudyTask task)
    {
        database.StudyTasks.Remove(task);
        return SaveChanges();
    }

    public bool SaveChanges()
    {
        try
        {
            database.SaveChanges();
            return true;
        }
        catch (DbUpdateConcurrencyException)
        {
            // The task was deleted by another request before the write.
            return false;
        }
    }
}

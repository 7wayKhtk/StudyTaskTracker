using StudyTaskTracker.Layered.Models;

namespace StudyTaskTracker.Layered.Data;

internal sealed class JsonTaskRepository : ITaskRepository
{
    private readonly JsonFile<StudyTask> file;
    private readonly List<StudyTask> tasks;

    public JsonTaskRepository(string filePath)
    {
        file = new JsonFile<StudyTask>(filePath);
        tasks = file.Load();
    }

    public IReadOnlyList<StudyTask> ListForOwner(int ownerId)
    {
        return tasks.Where(task => task.OwnerId == ownerId).ToList();
    }

    public StudyTask? FindForOwner(int ownerId, int id)
    {
        return tasks.Find(task => task.OwnerId == ownerId && task.Id == id);
    }

    public void Add(StudyTask task)
    {
        task.Id = tasks.Count == 0 ? 1 : tasks.Max(existingTask => existingTask.Id) + 1;
        tasks.Add(task);
        SaveChanges();
    }

    public void Delete(StudyTask task)
    {
        tasks.Remove(task);
        SaveChanges();
    }

    public void SaveChanges()
    {
        file.Save(tasks);
    }

    public int ClaimUnownedTasks(int ownerId)
    {
        List<StudyTask> unowned = tasks.Where(task => task.OwnerId is null).ToList();
        if (unowned.Count == 0)
        {
            return 0;
        }

        foreach (StudyTask task in unowned)
        {
            task.Claim(ownerId);
        }

        SaveChanges();
        return unowned.Count;
    }
}

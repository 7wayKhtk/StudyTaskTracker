using StudyTaskTracker.Layered.Data;
using StudyTaskTracker.Layered.Models;

namespace StudyTaskTracker.Layered.Services;

internal sealed class TaskService
{
    private readonly JsonTaskRepository repository;
    private readonly List<StudyTask> tasks;

    public TaskService(JsonTaskRepository repository)
    {
        this.repository = repository;
        tasks = repository.Load();
    }

    public void Add(string title, string course, DateOnly dueDate)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(title);
        ArgumentException.ThrowIfNullOrWhiteSpace(course);

        int id = tasks.Count == 0 ? 1 : tasks.Max(task => task.Id) + 1;
        tasks.Add(new StudyTask(id, title.Trim(), course.Trim(), dueDate));
        repository.Save(tasks);
    }

    public IReadOnlyList<StudyTask> List()
    {
        return tasks.AsReadOnly();
    }

    public StudyTask? Find(int id)
    {
        return tasks.Find(task => task.Id == id);
    }

    public bool Delete(int id)
    {
        StudyTask? task = Find(id);
        if (task is null)
        {
            return false;
        }

        tasks.Remove(task);
        repository.Save(tasks);
        return true;
    }

    public bool MarkComplete(int id)
    {
        StudyTask? task = Find(id);
        if (task is null)
        {
            return false;
        }

        task.MarkComplete();
        repository.Save(tasks);
        return true;
    }
}

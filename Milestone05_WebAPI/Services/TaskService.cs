using StudyTaskTracker.WebApi.Data;
using StudyTaskTracker.WebApi.Models;

namespace StudyTaskTracker.WebApi.Services;

public sealed class TaskService(ITaskRepository repository)
{
    public IReadOnlyList<StudyTask> List()
    {
        return repository.ListForOwner(User.DemoId);
    }

    public StudyTask? Find(int id)
    {
        return repository.FindForOwner(User.DemoId, id);
    }

    public StudyTask Add(string title, string course, DateOnly dueDate)
    {
        var task = new StudyTask(0, title.Trim(), course.Trim(), dueDate, User.DemoId);
        repository.Add(task);
        return task;
    }

    public bool Update(int id, string title, string course, DateOnly dueDate, bool isCompleted)
    {
        StudyTask? task = Find(id);
        if (task is null)
        {
            return false;
        }

        task.UpdateDetails(title.Trim(), course.Trim(), dueDate, isCompleted);
        return repository.SaveChanges();
    }

    public bool Delete(int id)
    {
        StudyTask? task = Find(id);
        return task is not null && repository.Delete(task);
    }
}

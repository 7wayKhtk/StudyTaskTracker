using StudyTaskTracker.Layered.Data;
using StudyTaskTracker.Layered.Models;

namespace StudyTaskTracker.Layered.Services;

internal sealed class TaskService(ITaskRepository repository, AccountService accounts)
{
    public void Add(string title, string course, DateOnly dueDate)
    {
        int ownerId = RequireOwnerId();
        ArgumentException.ThrowIfNullOrWhiteSpace(title);
        ArgumentException.ThrowIfNullOrWhiteSpace(course);

        repository.Add(new StudyTask(0, title.Trim(), course.Trim(), dueDate, ownerId: ownerId));
    }

    public IReadOnlyList<StudyTask> List()
    {
        return repository.ListForOwner(RequireOwnerId());
    }

    public StudyTask? Find(int id)
    {
        return repository.FindForOwner(RequireOwnerId(), id);
    }

    public bool Delete(int id)
    {
        StudyTask? task = Find(id);
        if (task is null)
        {
            return false;
        }

        repository.Delete(task);
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
        repository.SaveChanges();
        return true;
    }

    public bool CanImportLegacyTasks => accounts.CurrentUser?.Id == 1;

    public int ImportLegacyTasks()
    {
        if (!CanImportLegacyTasks)
        {
            throw new InvalidOperationException("The first registered account can import earlier tasks.");
        }

        return repository.ClaimUnownedTasks(RequireOwnerId());
    }

    private int RequireOwnerId()
    {
        return accounts.CurrentUser?.Id ?? throw new InvalidOperationException("Sign in to manage tasks.");
    }
}

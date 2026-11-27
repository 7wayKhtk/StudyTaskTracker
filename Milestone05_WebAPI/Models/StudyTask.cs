namespace StudyTaskTracker.WebApi.Models;

public sealed class StudyTask
{
    public int Id { get; internal set; }
    public int OwnerId { get; }
    public string Title { get; private set; }
    public string Course { get; private set; }
    public DateOnly DueDate { get; private set; }
    public bool IsCompleted { get; private set; }

    public StudyTask(int id, string title, string course, DateOnly dueDate,
        int ownerId, bool isCompleted = false)
    {
        Id = id;
        Title = title;
        Course = course;
        DueDate = dueDate;
        IsCompleted = isCompleted;
        OwnerId = ownerId;
    }

    public void UpdateDetails(string title, string course, DateOnly dueDate, bool isCompleted)
    {
        Title = title;
        Course = course;
        DueDate = dueDate;
        IsCompleted = isCompleted;
    }
}

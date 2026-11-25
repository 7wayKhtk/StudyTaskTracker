namespace StudyTaskTracker.Database.Models;

internal sealed class StudyTask
{
    public int Id { get; internal set; }
    public int OwnerId { get; }
    public string Title { get; }
    public string Course { get; }
    public DateOnly DueDate { get; }
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

    public void MarkComplete()
    {
        IsCompleted = true;
    }
}

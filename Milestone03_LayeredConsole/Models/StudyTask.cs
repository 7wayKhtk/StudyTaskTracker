namespace StudyTaskTracker.Layered.Models;

internal sealed class StudyTask
{
    public int Id { get; internal set; }
    public int? OwnerId { get; private set; }
    public string Title { get; }
    public string Course { get; }
    public DateOnly DueDate { get; }
    public bool IsCompleted { get; private set; }

    public StudyTask(int id, string title, string course, DateOnly dueDate,
        bool isCompleted = false, int? ownerId = null)
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

    internal void Claim(int ownerId)
    {
        OwnerId ??= ownerId;
    }
}

namespace StudyTaskTracker.Layered.Models;

internal sealed class StudyTask
{
    public int Id { get; }
    public string Title { get; }
    public string Course { get; }
    public DateOnly DueDate { get; }
    public bool IsCompleted { get; private set; }

    public StudyTask(int id, string title, string course, DateOnly dueDate, bool isCompleted = false)
    {
        Id = id;
        Title = title;
        Course = course;
        DueDate = dueDate;
        IsCompleted = isCompleted;
    }

    public void MarkComplete()
    {
        IsCompleted = true;
    }
}

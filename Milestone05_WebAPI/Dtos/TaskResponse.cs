using StudyTaskTracker.WebApi.Models;

namespace StudyTaskTracker.WebApi.Dtos;

public sealed record TaskResponse(int Id, string Title, string Course, DateOnly DueDate, bool IsCompleted)
{
    internal static TaskResponse FromTask(StudyTask task)
    {
        return new TaskResponse(task.Id, task.Title, task.Course, task.DueDate, task.IsCompleted);
    }
}

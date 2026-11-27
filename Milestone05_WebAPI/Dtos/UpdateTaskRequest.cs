using System.Text.Json.Serialization;

namespace StudyTaskTracker.WebApi.Dtos;

public sealed class UpdateTaskRequest : CreateTaskRequest
{
    [JsonRequired]
    public bool IsCompleted { get; init; }
}

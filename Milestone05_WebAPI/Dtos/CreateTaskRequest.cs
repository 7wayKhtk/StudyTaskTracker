using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace StudyTaskTracker.WebApi.Dtos;

public class CreateTaskRequest
{
    [Required]
    [StringLength(150)]
    public string Title { get; init; } = string.Empty;

    [Required]
    [StringLength(80)]
    public string Course { get; init; } = string.Empty;

    [JsonRequired]
    public DateOnly DueDate { get; init; }
}

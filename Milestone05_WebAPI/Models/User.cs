namespace StudyTaskTracker.WebApi.Models;

internal sealed class User
{
    public const int DemoId = 1;

    public int Id { get; init; }
    public string Username { get; init; } = string.Empty;
}

using System.Text.Json;
using StudyTaskTracker.Layered.Data;
using StudyTaskTracker.Layered.Presentation;
using StudyTaskTracker.Layered.Services;

string filePath = Path.GetFullPath("tasks-layered.json");

try
{
    var repository = new JsonTaskRepository(filePath);
    var taskService = new TaskService(repository);
    var menu = new ConsoleMenu(taskService);
    menu.Run(filePath);
    return 0;
}
catch (Exception exception) when (exception is IOException or UnauthorizedAccessException or JsonException)
{
    Console.Error.WriteLine($"Could not read or save tasks in {filePath}.");
    Console.Error.WriteLine(exception.Message);
    return 1;
}

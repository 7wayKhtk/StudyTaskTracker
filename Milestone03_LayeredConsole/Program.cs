using System.Text.Json;
using StudyTaskTracker.Layered.Data;
using StudyTaskTracker.Layered.Presentation;
using StudyTaskTracker.Layered.Services;

string taskFile = Path.GetFullPath("tasks-layered.json");
string userFile = Path.GetFullPath("users-layered.json");

try
{
    // Hold the lock while the app uses its in-memory copy of the JSON files.
    using FileStream dataLock = File.Open(taskFile + ".lock", FileMode.OpenOrCreate,
        FileAccess.ReadWrite, FileShare.None);

    var accounts = new AccountService(new JsonUserRepository(userFile));
    var tasks = new TaskService(new JsonTaskRepository(taskFile), accounts);
    var menu = new ConsoleMenu(tasks, accounts);

    Console.WriteLine("Study Task Tracker");
    Console.WriteLine($"Tasks file: {taskFile}");
    Console.WriteLine($"Users file: {userFile}");
    menu.Run();
    return 0;
}
catch (Exception exception) when (exception is IOException or UnauthorizedAccessException or JsonException)
{
    Console.Error.WriteLine($"Could not open, read, or save app data. {exception.Message}");
    return 1;
}

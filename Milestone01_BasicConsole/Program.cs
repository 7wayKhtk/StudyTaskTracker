using System.Globalization;
using System.Text.Json;

const string dateFormat = "yyyy-MM-dd";
string filePath = Path.GetFullPath("tasks.json");
var jsonOptions = new JsonSerializerOptions { WriteIndented = true };

try
{
    List<StudyTask> tasks = LoadTasks(filePath);
    Console.WriteLine("Study Task Tracker");
    Console.WriteLine($"Data file: {filePath}");

    while (true)
    {
        Console.WriteLine();
        Console.WriteLine("1. Add a task");
        Console.WriteLine("2. List tasks");
        Console.WriteLine("3. Find a task");
        Console.WriteLine("4. Delete a task");
        Console.WriteLine("0. Exit");
        Console.Write("Choose an option: ");

        string? choice = Console.ReadLine()?.Trim();
        switch (choice)
        {
            case "1":
                if (AddTask(tasks))
                {
                    SaveTasks(tasks, filePath, jsonOptions);
                    Console.WriteLine("Task added.");
                }
                break;

            case "2":
                ListTasks(tasks);
                break;

            case "3":
                FindOrDeleteTask(tasks, delete: false);
                break;

            case "4":
                if (FindOrDeleteTask(tasks, delete: true))
                {
                    SaveTasks(tasks, filePath, jsonOptions);
                    Console.WriteLine("Task deleted.");
                }
                break;

            case "0":
            case null:
                return 0;

            default:
                Console.WriteLine("Choose a number from 0 to 4.");
                break;
        }
    }
}
catch (Exception exception) when (exception is IOException or UnauthorizedAccessException or JsonException)
{
    Console.Error.WriteLine($"Could not read or save tasks in {filePath}.");
    Console.Error.WriteLine(exception.Message);
    return 1;
}

static bool AddTask(List<StudyTask> tasks)
{
    Console.Write("Title: ");
    string? title = Console.ReadLine()?.Trim();
    Console.Write("Course: ");
    string? course = Console.ReadLine()?.Trim();
    if (string.IsNullOrWhiteSpace(title) || string.IsNullOrWhiteSpace(course))
    {
        Console.WriteLine("Enter both a title and a course.");
        return false;
    }

    Console.Write("Due date (YYYY-MM-DD): ");
    string? dateInput = Console.ReadLine()?.Trim();
    if (!DateOnly.TryParseExact(dateInput, dateFormat, CultureInfo.InvariantCulture,
        DateTimeStyles.None, out DateOnly dueDate))
    {
        Console.WriteLine("Enter a valid date in YYYY-MM-DD format.");
        return false;
    }

    int id = tasks.Count == 0 ? 1 : tasks.Max(task => task.Id) + 1;
    tasks.Add(new StudyTask(id, title, course, dueDate, IsCompleted: false));
    return true;
}

static void ListTasks(List<StudyTask> tasks)
{
    if (tasks.Count == 0)
    {
        Console.WriteLine("No tasks yet.");
        return;
    }

    foreach (StudyTask task in tasks)
    {
        PrintTask(task);
    }
}

static bool FindOrDeleteTask(List<StudyTask> tasks, bool delete)
{
    Console.Write("Task ID: ");
    if (!int.TryParse(Console.ReadLine(), out int id) || id < 1)
    {
        Console.WriteLine("Enter a positive whole number.");
        return false;
    }

    int index = tasks.FindIndex(task => task.Id == id);
    if (index == -1)
    {
        Console.WriteLine($"No task found with ID {id}.");
        return false;
    }

    if (delete)
    {
        tasks.RemoveAt(index);
        return true;
    }

    PrintTask(tasks[index]);
    return false;
}

static void PrintTask(StudyTask task)
{
    string status = task.IsCompleted ? "Completed" : "Pending";
    string dueDate = task.DueDate.ToString(dateFormat, CultureInfo.InvariantCulture);
    Console.WriteLine($"#{task.Id} | {task.Title} | {task.Course} | Due {dueDate} | {status}");
}

static List<StudyTask> LoadTasks(string filePath)
{
    try
    {
        string json = File.ReadAllText(filePath);
        return JsonSerializer.Deserialize<List<StudyTask>>(json)
            ?? throw new JsonException("Expected a JSON array of tasks.");
    }
    catch (FileNotFoundException)
    {
        return new List<StudyTask>();
    }
}

static void SaveTasks(List<StudyTask> tasks, string filePath, JsonSerializerOptions options)
{
    string temporaryPath = filePath + ".tmp";
    File.WriteAllText(temporaryPath, JsonSerializer.Serialize(tasks, options));
    File.Move(temporaryPath, filePath, overwrite: true);
}

readonly record struct StudyTask(int Id, string Title, string Course, DateOnly DueDate, bool IsCompleted);

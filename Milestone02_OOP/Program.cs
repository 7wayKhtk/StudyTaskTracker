using System.Globalization;
using System.Text.Json;

const string dateFormat = "yyyy-MM-dd";
string filePath = Path.GetFullPath("tasks-oop.json");
var jsonOptions = new JsonSerializerOptions
{
    WriteIndented = true,
    RespectNullableAnnotations = true,
    RespectRequiredConstructorParameters = true
};

try
{
    List<StudyTask> tasks = LoadTasks(filePath, jsonOptions);
    Console.WriteLine("Study Task Tracker");
    Console.WriteLine($"Data file: {filePath}");

    while (true)
    {
        Console.WriteLine();
        Console.WriteLine("1. Add a task");
        Console.WriteLine("2. List tasks");
        Console.WriteLine("3. Find a task");
        Console.WriteLine("4. Delete a task");
        Console.WriteLine("5. Mark a task complete");
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
                StudyTask? foundTask = ReadTaskById(tasks);
                if (foundTask is not null)
                {
                    PrintTask(foundTask);
                }
                break;

            case "4":
                StudyTask? taskToDelete = ReadTaskById(tasks);
                if (taskToDelete is not null)
                {
                    tasks.Remove(taskToDelete);
                    SaveTasks(tasks, filePath, jsonOptions);
                    Console.WriteLine("Task deleted.");
                }
                break;

            case "5":
                StudyTask? taskToComplete = ReadTaskById(tasks);
                if (taskToComplete is not null)
                {
                    taskToComplete.MarkComplete();
                    SaveTasks(tasks, filePath, jsonOptions);
                    Console.WriteLine("Task marked complete.");
                }
                break;

            case "0":
            case null:
                return 0;

            default:
                Console.WriteLine("Choose a number from 0 to 5.");
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
    tasks.Add(new StudyTask(id, title, course, dueDate));
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

static StudyTask? ReadTaskById(List<StudyTask> tasks)
{
    Console.Write("Task ID: ");
    if (!int.TryParse(Console.ReadLine(), out int id) || id < 1)
    {
        Console.WriteLine("Enter a positive whole number.");
        return null;
    }

    StudyTask? task = tasks.Find(candidate => candidate.Id == id);
    if (task is null)
    {
        Console.WriteLine($"No task found with ID {id}.");
    }

    return task;
}

static void PrintTask(StudyTask task)
{
    string status = task.IsCompleted ? "Completed" : "Pending";
    string dueDate = task.DueDate.ToString(dateFormat, CultureInfo.InvariantCulture);
    Console.WriteLine($"#{task.Id} | {task.Title} | {task.Course} | Due {dueDate} | {status}");
}

static List<StudyTask> LoadTasks(string filePath, JsonSerializerOptions options)
{
    try
    {
        string json = File.ReadAllText(filePath);
        List<StudyTask> tasks = JsonSerializer.Deserialize<List<StudyTask>>(json, options)
            ?? throw new JsonException("Expected a JSON array of tasks.");
        if (tasks.Any(task => task is null))
        {
            throw new JsonException("Every item in the JSON array must be a task.");
        }

        return tasks;
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

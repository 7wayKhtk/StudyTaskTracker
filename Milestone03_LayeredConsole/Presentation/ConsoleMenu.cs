using System.Globalization;
using StudyTaskTracker.Layered.Models;
using StudyTaskTracker.Layered.Services;

namespace StudyTaskTracker.Layered.Presentation;

internal sealed class ConsoleMenu
{
    private const string DateFormat = "yyyy-MM-dd";
    private readonly TaskService taskService;

    public ConsoleMenu(TaskService taskService)
    {
        this.taskService = taskService;
    }

    public void Run(string filePath)
    {
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

            switch (Console.ReadLine()?.Trim())
            {
                case "1":
                    AddTask();
                    break;

                case "2":
                    ListTasks();
                    break;

                case "3":
                    FindTask();
                    break;

                case "4":
                    DeleteTask();
                    break;

                case "5":
                    CompleteTask();
                    break;

                case "0":
                case null:
                    return;

                default:
                    Console.WriteLine("Choose a number from 0 to 5.");
                    break;
            }
        }
    }

    private void AddTask()
    {
        Console.Write("Title: ");
        string? title = Console.ReadLine();
        Console.Write("Course: ");
        string? course = Console.ReadLine();
        if (string.IsNullOrWhiteSpace(title) || string.IsNullOrWhiteSpace(course))
        {
            Console.WriteLine("Enter both a title and a course.");
            return;
        }

        Console.Write("Due date (YYYY-MM-DD): ");
        string? dateInput = Console.ReadLine()?.Trim();
        if (!DateOnly.TryParseExact(dateInput, DateFormat, CultureInfo.InvariantCulture,
            DateTimeStyles.None, out DateOnly dueDate))
        {
            Console.WriteLine("Enter a valid date in YYYY-MM-DD format.");
            return;
        }

        taskService.Add(title, course, dueDate);
        Console.WriteLine("Task added.");
    }

    private void ListTasks()
    {
        IReadOnlyList<StudyTask> tasks = taskService.List();
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

    private void FindTask()
    {
        if (ReadTaskId() is not int id)
        {
            return;
        }

        StudyTask? task = taskService.Find(id);
        if (task is null)
        {
            Console.WriteLine($"No task found with ID {id}.");
            return;
        }

        PrintTask(task);
    }

    private void DeleteTask()
    {
        if (ReadTaskId() is int id)
        {
            Console.WriteLine(taskService.Delete(id) ? "Task deleted." : $"No task found with ID {id}.");
        }
    }

    private void CompleteTask()
    {
        if (ReadTaskId() is int id)
        {
            Console.WriteLine(taskService.MarkComplete(id)
                ? "Task marked complete."
                : $"No task found with ID {id}.");
        }
    }

    private static int? ReadTaskId()
    {
        Console.Write("Task ID: ");
        if (!int.TryParse(Console.ReadLine(), out int id) || id < 1)
        {
            Console.WriteLine("Enter a positive whole number.");
            return null;
        }

        return id;
    }

    private static void PrintTask(StudyTask task)
    {
        string status = task.IsCompleted ? "Completed" : "Pending";
        string dueDate = task.DueDate.ToString(DateFormat, CultureInfo.InvariantCulture);
        Console.WriteLine($"#{task.Id} | {task.Title} | {task.Course} | Due {dueDate} | {status}");
    }
}

using System.Globalization;
using System.Text;
using StudyTaskTracker.Database.Models;
using StudyTaskTracker.Database.Services;

namespace StudyTaskTracker.Database.Presentation;

internal sealed class ConsoleMenu
{
    private const string DateFormat = "yyyy-MM-dd";
    private readonly TaskService taskService;
    private readonly AccountService accounts;

    public ConsoleMenu(TaskService taskService, AccountService accounts)
    {
        this.taskService = taskService;
        this.accounts = accounts;
    }

    public void Run()
    {
        while (true)
        {
            Console.WriteLine();
            if (accounts.CurrentUser is null)
            {
                if (!ShowAccountMenu())
                {
                    return;
                }

                continue;
            }

            Console.WriteLine($"Signed in as {accounts.CurrentUser.Username}");
            Console.WriteLine("1. Add a task");
            Console.WriteLine("2. List tasks");
            Console.WriteLine("3. Find a task");
            Console.WriteLine("4. Delete a task");
            Console.WriteLine("5. Mark a task complete");
            Console.WriteLine("6. Log out");
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

                case "6":
                    accounts.Logout();
                    Console.WriteLine("Logged out.");
                    break;

                case "0":
                case null:
                    return;

                default:
                    Console.WriteLine("Choose one of the menu options.");
                    break;
            }
        }
    }

    private bool ShowAccountMenu()
    {
        Console.WriteLine("1. Register");
        Console.WriteLine("2. Log in");
        Console.WriteLine("0. Exit");
        Console.Write("Choose an option: ");

        switch (Console.ReadLine()?.Trim())
        {
            case "1":
                Console.Write("Username: ");
                string? newUsername = Console.ReadLine();
                string? error = accounts.Register(newUsername, ReadPassword());
                Console.WriteLine(error ?? "Account created. You can now log in.");
                return true;

            case "2":
                Console.Write("Username: ");
                string? username = Console.ReadLine();
                Console.WriteLine(accounts.Login(username, ReadPassword())
                    ? "Logged in."
                    : "Invalid username or password.");
                return true;

            case "0":
            case null:
                return false;

            default:
                Console.WriteLine("Choose one of the menu options.");
                return true;
        }
    }

    private static string? ReadPassword()
    {
        Console.Write("Password: ");
        if (Console.IsInputRedirected)
        {
            return Console.ReadLine();
        }

        var password = new StringBuilder();
        while (true)
        {
            ConsoleKeyInfo key = Console.ReadKey(intercept: true);
            if (key.Key == ConsoleKey.Enter)
            {
                Console.WriteLine();
                return password.ToString();
            }

            if (key.Key == ConsoleKey.Backspace && password.Length > 0)
            {
                password.Length--;
            }
            else if (!char.IsControl(key.KeyChar))
            {
                password.Append(key.KeyChar);
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

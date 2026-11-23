# Study Task Tracker

A C# application for keeping track of coursework and due dates.

## Milestone 01 · Basic console

The menu repeats until you exit. You can add a task, list all tasks, find one by its
numeric ID, and delete it. A task stores a title, course, due date, and completion
status. New tasks start as pending.

This version demonstrates variables and basic types, a `switch` and `if` conditions,
`while` and `foreach` loops, methods, a `List<StudyTask>`, and JSON persistence.
It loads the list on startup and saves after every successful addition or deletion.

### Run it

Install the .NET 10 SDK and run Milestone 01 from the `StudyTaskTracker` folder.

```sh
cd Milestone01_BasicConsole
dotnet run
```

In Visual Studio with .NET 10 support, you can open `StudyTaskTracker.sln` and run
`Milestone01_BasicConsole`. The app prints the full path to `tasks.json` on
startup. It uses the current working directory, so launch from the same directory
each time when testing persistence.

### Try it

1. Add a task named “Finish lab report” for “Databases” with a date in `YYYY-MM-DD` format.
2. List tasks and note its ID. Exit, rerun the app, and find the task by ID.
3. Delete the task by ID. Exit, rerun, and verify the task is gone.

## Milestone 02 · OOP and persistence

Milestone 01 represented each task with a small record struct. Milestone 02 uses a
`StudyTask` class in its own file. Calling `new StudyTask(...)` uses its constructor
to create an object and set its initial properties. The console can read those
properties. Only the class can assign `IsCompleted`, and option 5 calls its
`MarkComplete()` method. This is encapsulation. The class controls how a task
changes, while the console handles input and output.

The same JSON fields are used to save and restore task objects, including their
completion status. Milestone 02 uses `tasks-oop.json`, so you can run both milestones
from the same directory with independent data. To bring over Day 1 tasks, copy
`tasks.json` to `tasks-oop.json` in the directory where you run Milestone 02, before
creating any new data there. This milestone continues to use JSON file storage.

### Run it

With the .NET 10 SDK installed, run Milestone 02 from the `StudyTaskTracker` folder.

```sh
cd Milestone02_OOP
dotnet run
```

In Visual Studio, open `StudyTaskTracker.sln`, right-click `Milestone02_OOP` in
Solution Explorer, and select **Set as Startup Project**. Press **Ctrl+F5** to run it.

### Try it

1. Add a task, list the tasks, and check that the new task shows **Pending**.
2. Choose option 5 and enter the task ID to mark it **Completed**.
3. Exit, rerun, and find the task by ID to verify the completion status was saved.

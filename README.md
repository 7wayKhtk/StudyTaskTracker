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

Install the .NET 10 SDK. From the `StudyTaskTracker` folder, run:

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

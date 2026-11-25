using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using StudyTaskTracker.Database.Data;
using StudyTaskTracker.Database.Presentation;
using StudyTaskTracker.Database.Services;

string databasePath = Path.GetFullPath("study-task-tracker.db");
string connectionString = new SqliteConnectionStringBuilder
{
    DataSource = databasePath,
    ForeignKeys = true
}.ToString();

var options = new DbContextOptionsBuilder<TrackerDbContext>()
    .UseSqlite(connectionString)
    .Options;

try
{
    using var database = new TrackerDbContext(options);
    database.Database.EnsureCreated();

    var accounts = new AccountService(new SqliteUserRepository(database));
    var tasks = new TaskService(new SqliteTaskRepository(database), accounts);
    var menu = new ConsoleMenu(tasks, accounts);

    Console.WriteLine("Study Task Tracker");
    Console.WriteLine($"Database file: {databasePath}");
    menu.Run();
    return 0;
}
catch (Exception exception) when (exception is SqliteException or DbUpdateException)
{
    Console.Error.WriteLine($"Could not open, read, or save the database. {exception.GetBaseException().Message}");
    return 1;
}

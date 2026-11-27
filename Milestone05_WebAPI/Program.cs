using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using StudyTaskTracker.WebApi.Data;
using StudyTaskTracker.WebApi.Services;

var builder = WebApplication.CreateBuilder(args);

string connectionString = new SqliteConnectionStringBuilder
{
    DataSource = Path.Combine(builder.Environment.ContentRootPath, "study-task-api.db"),
    ForeignKeys = true
}.ToString();

builder.Services.AddDbContext<TrackerDbContext>(options => options.UseSqlite(connectionString));
builder.Services.AddScoped<ITaskRepository, SqliteTaskRepository>();
builder.Services.AddScoped<TaskService>();
builder.Services.AddControllers();
builder.Services.AddProblemDetails();
builder.Services.AddSwaggerGen();

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var database = scope.ServiceProvider.GetRequiredService<TrackerDbContext>();
    database.Database.EnsureCreated();
}

app.UseExceptionHandler();
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapControllers();
app.Run();

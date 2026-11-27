using Microsoft.AspNetCore.Mvc;
using StudyTaskTracker.WebApi.Dtos;
using StudyTaskTracker.WebApi.Models;
using StudyTaskTracker.WebApi.Services;

namespace StudyTaskTracker.WebApi.Controllers;

[ApiController]
[Route("api/tasks")]
public sealed class TasksController(TaskService tasks) : ControllerBase
{
    [HttpGet]
    [ProducesResponseType<IReadOnlyList<TaskResponse>>(StatusCodes.Status200OK)]
    public ActionResult<IReadOnlyList<TaskResponse>> GetAll()
    {
        return Ok(tasks.List().Select(TaskResponse.FromTask).ToList());
    }

    [HttpGet("{id:int}")]
    [ProducesResponseType<TaskResponse>(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public ActionResult<TaskResponse> GetById(int id)
    {
        StudyTask? task = tasks.Find(id);
        if (task is null)
        {
            return NotFound();
        }

        return Ok(TaskResponse.FromTask(task));
    }

    [HttpPost]
    [ProducesResponseType<TaskResponse>(StatusCodes.Status201Created)]
    [ProducesResponseType<ValidationProblemDetails>(StatusCodes.Status400BadRequest)]
    public ActionResult<TaskResponse> Create(CreateTaskRequest request)
    {
        StudyTask task = tasks.Add(request.Title, request.Course, request.DueDate);
        return CreatedAtAction(nameof(GetById), new { id = task.Id }, TaskResponse.FromTask(task));
    }

    [HttpPut("{id:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType<ValidationProblemDetails>(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public IActionResult Update(int id, UpdateTaskRequest request)
    {
        return tasks.Update(id, request.Title, request.Course, request.DueDate, request.IsCompleted)
            ? NoContent()
            : NotFound();
    }

    [HttpDelete("{id:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public IActionResult Delete(int id)
    {
        return tasks.Delete(id) ? NoContent() : NotFound();
    }
}

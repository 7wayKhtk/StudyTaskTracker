using System.Text.Json;
using StudyTaskTracker.Layered.Models;

namespace StudyTaskTracker.Layered.Data;

internal sealed class JsonTaskRepository
{
    private readonly string filePath;
    private readonly JsonSerializerOptions jsonOptions = new()
    {
        WriteIndented = true,
        RespectNullableAnnotations = true,
        RespectRequiredConstructorParameters = true
    };

    public JsonTaskRepository(string filePath)
    {
        this.filePath = filePath;
    }

    public List<StudyTask> Load()
    {
        try
        {
            string json = File.ReadAllText(filePath);
            List<StudyTask> tasks = JsonSerializer.Deserialize<List<StudyTask>>(json, jsonOptions)
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

    public void Save(List<StudyTask> tasks)
    {
        string temporaryPath = filePath + ".tmp";
        File.WriteAllText(temporaryPath, JsonSerializer.Serialize(tasks, jsonOptions));
        File.Move(temporaryPath, filePath, overwrite: true);
    }
}

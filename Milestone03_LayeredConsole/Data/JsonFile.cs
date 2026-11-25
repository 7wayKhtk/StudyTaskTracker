using System.Text.Json;

namespace StudyTaskTracker.Layered.Data;

internal sealed class JsonFile<T>(string path) where T : class
{
    private readonly JsonSerializerOptions options = new()
    {
        WriteIndented = true,
        RespectNullableAnnotations = true,
        RespectRequiredConstructorParameters = true
    };

    public List<T> Load()
    {
        try
        {
            string json = File.ReadAllText(path);
            List<T> items = JsonSerializer.Deserialize<List<T>>(json, options)
                ?? throw new JsonException("Expected a JSON array.");
            if (items.Any(item => item is null))
            {
                throw new JsonException("Every item in the JSON array must be an object.");
            }

            return items;
        }
        catch (FileNotFoundException)
        {
            return new List<T>();
        }
    }

    public void Save(List<T> items)
    {
        string temporaryPath = path + ".tmp";
        File.WriteAllText(temporaryPath, JsonSerializer.Serialize(items, options));
        File.Move(temporaryPath, path, overwrite: true);
    }
}

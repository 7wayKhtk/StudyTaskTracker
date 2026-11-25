using StudyTaskTracker.Layered.Models;

namespace StudyTaskTracker.Layered.Data;

internal sealed class JsonUserRepository : IUserRepository
{
    private readonly JsonFile<User> file;
    private readonly List<User> users;

    public JsonUserRepository(string filePath)
    {
        file = new JsonFile<User>(filePath);
        users = file.Load();
    }

    public User? FindByUsername(string normalizedUsername)
    {
        return users.Find(user => user.NormalizedUsername == normalizedUsername);
    }

    public bool TryAdd(User user)
    {
        if (FindByUsername(user.NormalizedUsername) is not null)
        {
            return false;
        }

        user.Id = users.Count == 0 ? 1 : users.Max(existingUser => existingUser.Id) + 1;
        users.Add(user);
        file.Save(users);
        return true;
    }
}

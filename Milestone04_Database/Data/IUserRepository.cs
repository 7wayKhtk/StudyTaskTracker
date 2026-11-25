using StudyTaskTracker.Database.Models;

namespace StudyTaskTracker.Database.Data;

internal interface IUserRepository
{
    User? FindByUsername(string normalizedUsername);
    bool TryAdd(User user);
}

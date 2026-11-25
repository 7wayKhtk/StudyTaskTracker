using StudyTaskTracker.Layered.Models;

namespace StudyTaskTracker.Layered.Data;

internal interface IUserRepository
{
    User? FindByUsername(string normalizedUsername);
    bool TryAdd(User user);
}

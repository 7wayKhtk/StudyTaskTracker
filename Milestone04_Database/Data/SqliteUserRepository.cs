using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using StudyTaskTracker.Database.Models;

namespace StudyTaskTracker.Database.Data;

internal sealed class SqliteUserRepository(TrackerDbContext database) : IUserRepository
{
    private const int SqliteUniqueConstraint = 2067;

    public User? FindByUsername(string normalizedUsername)
    {
        return database.Users.AsNoTracking()
            .SingleOrDefault(user => user.NormalizedUsername == normalizedUsername);
    }

    public bool TryAdd(User user)
    {
        database.Users.Add(user);
        try
        {
            database.SaveChanges();
            return true;
        }
        catch (DbUpdateException exception) when (exception.InnerException is SqliteException
            { SqliteExtendedErrorCode: SqliteUniqueConstraint })
        {
            // A second process may have registered the name after the service checked it.
            database.Entry(user).State = EntityState.Detached;
            return false;
        }
    }
}

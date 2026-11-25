namespace StudyTaskTracker.Layered.Models;

internal sealed class User
{
    public int Id { get; internal set; }
    public string Username { get; }
    public string NormalizedUsername { get; }
    public byte[] PasswordSalt { get; }
    public byte[] PasswordHash { get; }

    public User(int id, string username, string normalizedUsername, byte[] passwordSalt, byte[] passwordHash)
    {
        Id = id;
        Username = username;
        NormalizedUsername = normalizedUsername;
        PasswordSalt = passwordSalt;
        PasswordHash = passwordHash;
    }
}

using System.Security.Cryptography;
using StudyTaskTracker.Layered.Data;
using StudyTaskTracker.Layered.Models;

namespace StudyTaskTracker.Layered.Services;

internal sealed class AccountService(IUserRepository users)
{
    private const int SaltSize = 16;
    private const int HashSize = 32;
    private const int HashIterations = 600_000;

    public User? CurrentUser { get; private set; }

    public string? Register(string? username, string? password)
    {
        username = username?.Trim();
        if (username is null || username.Length is < 3 or > 30 ||
            !username.All(character => char.IsAsciiLetterOrDigit(character) || character is '_' or '-' or '.'))
        {
            return "Use 3 to 30 letters, numbers, dots, hyphens, or underscores for the username.";
        }

        if (password is null || password.Length is < 8 or > 128 || string.IsNullOrWhiteSpace(password))
        {
            return "Use a password of 8 to 128 characters that includes a non-space character.";
        }

        string normalizedUsername = username.ToUpperInvariant();
        if (users.FindByUsername(normalizedUsername) is not null)
        {
            return "That username is already registered.";
        }

        byte[] salt = RandomNumberGenerator.GetBytes(SaltSize);
        var user = new User(0, username, normalizedUsername, salt, HashPassword(password, salt));
        return users.TryAdd(user) ? null : "That username is already registered.";
    }

    public bool Login(string? username, string? password)
    {
        CurrentUser = null;
        if (string.IsNullOrWhiteSpace(username) || password is null || password.Length is < 8 or > 128)
        {
            return false;
        }

        User? user = users.FindByUsername(username.Trim().ToUpperInvariant());
        if (user is null || !CryptographicOperations.FixedTimeEquals(
                user.PasswordHash, HashPassword(password, user.PasswordSalt)))
        {
            return false;
        }

        CurrentUser = user;
        return true;
    }

    public void Logout()
    {
        CurrentUser = null;
    }

    private static byte[] HashPassword(string password, byte[] salt)
    {
        return Rfc2898DeriveBytes.Pbkdf2(password, salt, HashIterations, HashAlgorithmName.SHA256, HashSize);
    }
}

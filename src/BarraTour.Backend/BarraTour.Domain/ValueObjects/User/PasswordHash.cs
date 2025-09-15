namespace BarraTour.Domain.ValueObjects.User;

public record PasswordHash
{
    public string Value { get; private set; }
    public string Salt { get; private set; }
    public DateTime CreatedAt { get; private set; } = DateTime.UtcNow;

    // Construtor sem parâmetros para o EF Core
    private PasswordHash()
    {
        Value = string.Empty;
        Salt = string.Empty;
    }

    private PasswordHash(string hash, string salt)
    {
        Value = hash;
        Salt = salt;
    }

    public static PasswordHash Create(string password)
    {
        if (string.IsNullOrWhiteSpace(password))
            throw new ArgumentException("Password cannot be empty.", nameof(password));
                
        if (password.Length < 6)
            throw new ArgumentException("Password must be at least 6 characters long.", nameof(password));

        var salt = GenerateSalt();
        var hash = HashPassword(password, salt);
        return new PasswordHash(hash, salt);
    }

    public bool Verify(string password) => HashPassword(password, Salt) == Value;

    private static string GenerateSalt()
    {
        return Convert.ToBase64String(Guid.NewGuid().ToByteArray()).Substring(0, 16);
    }

    private static string HashPassword(string password, string salt)
    {
        using var sha256 = System.Security.Cryptography.SHA256.Create();
        var saltedPassword = password + salt;
        var bytes = sha256.ComputeHash(System.Text.Encoding.UTF8.GetBytes(saltedPassword));
        return Convert.ToBase64String(bytes);
    }

    public static implicit operator string(PasswordHash passwordHash) => passwordHash.Value;
        
    public override string ToString() => "*****"; // Nunca expor o hash real
}
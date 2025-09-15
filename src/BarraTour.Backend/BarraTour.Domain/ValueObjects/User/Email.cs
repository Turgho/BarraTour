using System.Text.RegularExpressions;

namespace BarraTour.Domain.ValueObjects.User;

public record Email
{
    private static readonly Regex EmailRegex = new(
        @"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$",
        RegexOptions.Compiled | RegexOptions.IgnoreCase);

    public string Value { get; private set; }

    // Construtor sem parâmetros para EF Core
    private Email()
    {
        Value = string.Empty;
    }

    public Email(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new ArgumentException("Email não pode estar vazio.", nameof(value));

        var normalizedEmail = value.Trim().ToLowerInvariant();

        if (!EmailRegex.IsMatch(normalizedEmail))
            throw new ArgumentException("Formato de email inválido.", nameof(value));

        if (normalizedEmail.Length > 254)
            throw new ArgumentException("Email não pode exceder 254 caracteres.", nameof(value));

        Value = normalizedEmail;
    }

    public static implicit operator string(Email email) => email.Value;
    public static implicit operator Email(string value) => new(value);

    public override string ToString() => Value;
}
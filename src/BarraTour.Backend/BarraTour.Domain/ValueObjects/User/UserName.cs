namespace BarraTour.Domain.ValueObjects.User;

public record UserName
{
    public string FirstName { get; init; }
    public string LastName { get; init; }
    
    // Construtor sem parâmetros para EF Core
    private UserName()
    {
        FirstName = string.Empty;
        LastName = string.Empty;
    }
    
    public UserName(string firstName, string lastName)
    {
        if (string.IsNullOrWhiteSpace(firstName) || firstName.Length < 2)
            throw new ArgumentException("First name must be at least 2 characters");
            
        if (string.IsNullOrWhiteSpace(lastName) || lastName.Length < 2)
            throw new ArgumentException("Last name must be at least 2 characters");
            
        FirstName = firstName.Trim();
        LastName = lastName.Trim();
    }

    private string FullName => $"{FirstName} {LastName}";
    public string Initials => $"{FirstName[0]}{LastName[0]}".ToUpper();
    
    public static implicit operator string(UserName userName) => userName.FullName;
    
    public override string ToString() => FullName;
}
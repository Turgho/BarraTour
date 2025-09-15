namespace BarraTour.Domain.ValueObjects.User;

public record PhoneNumber
{
    public string CountryCode { get; init; }
    public string Number { get; init; }
    
    // Construtor sem parâmetros para EF Core
    private PhoneNumber()
    {
        CountryCode = string.Empty;
        Number = string.Empty;
    }

    public PhoneNumber(string countryCode, string number)
    {
        if (string.IsNullOrWhiteSpace(countryCode))
            throw new ArgumentException("Country code is required", nameof(countryCode));

        if (string.IsNullOrWhiteSpace(number) || number.Length < 8)
            throw new ArgumentException("Number must be at least 8 characters", nameof(number));

        CountryCode = CleanCountryCode(countryCode);
        Number = CleanPhoneNumber(number);
    }

    public static PhoneNumber Parse(string fullNumber)
    {
        if (string.IsNullOrWhiteSpace(fullNumber))
            throw new ArgumentException("Phone number cannot be empty", nameof(fullNumber));

        // Remove espaços e caracteres especiais, mantém apenas dígitos e +
        var cleanNumber = System.Text.RegularExpressions.Regex.Replace(fullNumber, @"[^\d+]", "");

        // Implementar lógica de parsing conforme necessário
        // Exemplo simples: assumindo que o fullNumber vem no formato "+5511999999999"
        if (cleanNumber.StartsWith("+"))
        {
            var countryCode = cleanNumber.Substring(0, 3); // +55
            var number = cleanNumber.Substring(3);
            return new PhoneNumber(countryCode, number);
        }

        // Se não tem código do país, assume Brasil (+55)
        if (cleanNumber.Length >= 10)
        {
            return new PhoneNumber("+55", cleanNumber);
        }

        throw new ArgumentException("Invalid phone number format", nameof(fullNumber));
    }

    private static string CleanCountryCode(string countryCode)
    {
        if (!countryCode.StartsWith("+"))
            return "+" + countryCode.Replace("+", "");
        return countryCode;
    }

    private static string CleanPhoneNumber(string number)
    {
        return System.Text.RegularExpressions.Regex.Replace(number, @"[^\d]", "");
    }

    public string ToFormattedString()
    {
        // Formatação brasileira
        if (CountryCode == "+55" && Number.Length == 11)
            return $"({Number.Substring(0, 2)}) {Number.Substring(2, 5)}-{Number.Substring(7)}";
        
        if (CountryCode == "+55" && Number.Length == 10)
            return $"({Number.Substring(0, 2)}) {Number.Substring(2, 4)}-{Number.Substring(6)}";

        return $"{CountryCode} {Number}";
    }

    public override string ToString() => ToFormattedString();

    public string ToInternationalFormat() => $"{CountryCode}{Number}";

    public static implicit operator string(PhoneNumber phoneNumber) => phoneNumber.ToInternationalFormat();
}
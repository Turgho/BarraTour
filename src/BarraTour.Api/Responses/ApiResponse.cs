namespace BarraTour.Api.Responses;

public class ApiResponse<T>
{
    public T Data { get; set; } = default!;
    public string Message { get; set; } = string.Empty;
    public bool Success { get; set; }
    public IEnumerable<string> Errors { get; set; } = [];

    // Resposta de sucesso com dados
    public static ApiResponse<T> Ok(T data, string message = "Operação realizada com sucesso")
        => new() { Data = data, Message = message, Success = true, Errors = [] };

    // Resposta de falha
    public static ApiResponse<T> Fail(string message, IEnumerable<string>? errors = null)
        => new() { Data = default!, Message = message, Success = false, Errors = errors ?? [] };

    // Resposta de sucesso sem dados
    public static ApiResponse<T> EmptyOk(string message = "Operação realizada com sucesso")
        => new() { Data = default!, Message = message, Success = true, Errors = [] };
}

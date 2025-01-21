namespace BugBusters.Server.Core.Dtos;

public class ApiResponseDto<T>
{
    public T? Data { get; set; }
    public string? Message { get; set; }
    public bool IsSuccess { get; set; }
}
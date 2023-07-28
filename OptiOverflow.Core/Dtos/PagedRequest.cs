namespace OptiOverflow.Core.Dtos;

public class PagedRequest
{
    public string? Query { get; set; }
    public int Page { get; set; } = 1;
    public int PageSize { get; set; } = 10;
}
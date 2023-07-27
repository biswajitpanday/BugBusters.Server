namespace OptiOverflow.Core.Dtos;

public class PagedResponse<T>
{
    public T? Items { get; set; }
    public int TotalPages { get; set; }
    public long ItemCount { get; set; }
}
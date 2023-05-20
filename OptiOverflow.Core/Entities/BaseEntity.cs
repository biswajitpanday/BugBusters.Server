namespace OptiOverflow.Core.Entities;

public class BaseEntity
{
    public Guid Id { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime LastUpdate { get; set; }
    public bool IsDeleted { get; set; }
}
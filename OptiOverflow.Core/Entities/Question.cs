namespace OptiOverflow.Core.Entities;

public class Question: BaseEntity
{
    public string Title { get; set; } = null!;
    public string Body { get; set; } = null!;
    public Guid CreatedById { get; set; }
}
namespace OptiOverflow.Core.Entities;

public class Question: BaseEntity
{
    public string Title { get; set; } = null!;
    public string Body { get; set; } = null!;
    public Guid CreatedById { get; set; }
    public Guid LastUpdatedById { get; set; }

    public ICollection<Answer>? Answers { get; set; }
    public ICollection<Vote>? Votes { get; set; }

}
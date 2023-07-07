namespace OptiOverflow.Core.Entities;

public class Answer : BaseEntity
{
    public string Body { get; set; } = null!;
    public bool IsAccepted { get; set; }

    public Guid? CreatedById { get; set; }
    public Guid QuestionId { get; set; }

    public ApplicationUser CreatedBy { get; set; } = null!;
    public Question Question { get; set; } = null!;
    public ICollection<Vote>? Votes { get; set; }
}
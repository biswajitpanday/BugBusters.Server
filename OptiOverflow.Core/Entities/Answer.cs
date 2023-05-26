namespace OptiOverflow.Core.Entities;

public class Answer: BaseEntity
{
    public string Body { get; set; } = null!;
    public bool IsAccepted { get; set; }

    public Guid CreatedBy { get; set; }
    public Guid QuestionId { get; set; }
}
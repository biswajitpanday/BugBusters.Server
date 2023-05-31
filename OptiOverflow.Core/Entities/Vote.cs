namespace OptiOverflow.Core.Entities;

public class Vote: BaseEntity
{
    public bool IsUpVote { get; set; }

    public Guid UserId { get; set; }
    public Guid? QuestionId { get; set; }
    public Guid? AnswerId { get; set; }
}
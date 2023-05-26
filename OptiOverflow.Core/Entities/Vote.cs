using OptiOverflow.Core.Enums;

namespace OptiOverflow.Core.Entities;

public class Vote: BaseEntity
{
    public VoteTypeEnum VoteType { get; set; }

    public Guid? QuestionId { get; set; }
    public Guid? AnswerId { get; set; }
}
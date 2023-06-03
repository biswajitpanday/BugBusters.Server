using OptiOverflow.Core.Entities;
using OptiOverflow.Core.Interfaces.Common;

namespace OptiOverflow.Core.Dtos;

public class AnswerDto : IMapFrom<Answer>
{
    public Guid Id { get; set; }
    public string Body { get; set; } = null!;
    public bool IsAccepted { get; set; }
    public Guid UserId { get; set; }
    public Guid QuestionId { get; set; }
    public DateTime CreatedAt { get; set; }
    public int VoteCount { get; set; }
}
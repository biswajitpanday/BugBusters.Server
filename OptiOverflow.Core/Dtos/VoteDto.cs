using OptiOverflow.Core.Entities;
using OptiOverflow.Core.Interfaces.Common;

namespace OptiOverflow.Core.Dtos;

public class VoteDto : IMapFrom<Vote>
{
    public Guid Id { get; set; }
    public bool IsUpVote { get; set; }
    public Guid UserId { get; set; }
    public Guid? QuestionId { get; set; }
    public Guid? AnswerId { get; set; }
}
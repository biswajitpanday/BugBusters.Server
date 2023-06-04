
using OptiOverflow.Core.Entities;
using OptiOverflow.Core.Interfaces.Common;

namespace OptiOverflow.Core.Dtos;

public class VoteCreateDto : IMapFrom<Vote>
{
    public bool IsUpVote { get; set; }
    public Guid? QuestionId { get; set; }
    public Guid? AnswerId { get; set; }
}
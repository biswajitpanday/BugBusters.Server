using BugBusters.Server.Core.Entities;
using BugBusters.Server.Core.Interfaces.Common;

namespace BugBusters.Server.Core.Dtos;

public class VoteResponseDto : IMapFrom<Vote>
{
    public Guid Id { get; set; }
    public bool IsUpVote { get; set; }
    public Guid UserId { get; set; }
    public Guid? QuestionId { get; set; }
    public Guid? AnswerId { get; set; }
}
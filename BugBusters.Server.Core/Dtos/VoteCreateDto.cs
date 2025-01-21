using BugBusters.Server.Core.Entities;
using BugBusters.Server.Core.Interfaces.Common;

namespace BugBusters.Server.Core.Dtos;

public class VoteCreateDto : IMapFrom<Vote>
{
    public bool IsUpVote { get; set; }
    public Guid? QuestionId { get; set; }
    public Guid? AnswerId { get; set; }
}
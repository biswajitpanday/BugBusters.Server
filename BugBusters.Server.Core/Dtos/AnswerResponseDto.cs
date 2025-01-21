using BugBusters.Server.Core.Entities;
using BugBusters.Server.Core.Interfaces.Common;

namespace BugBusters.Server.Core.Dtos;

public class AnswerResponseDto : IMapFrom<Answer>
{
    public Guid Id { get; set; }
    public string Body { get; set; } = null!;
    public bool IsAccepted { get; set; }
    public Guid? CreatedById { get; set; }
    public Guid QuestionId { get; set; }
    public DateTime CreatedAt { get; set; }
    public int UpVoteCount { get; set; }
    public int DownVoteCount { get; set; }
    public ProfileResponseDto CreatedBy { get; set; } = null!;
}
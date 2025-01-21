using BugBusters.Server.Core.Entities;
using BugBusters.Server.Core.Interfaces.Common;

namespace BugBusters.Server.Core.Dtos;

public class AnswerCreateDto : IMapFrom<Answer>
{
    public string Body { get; set; } = null!;
    public string QuestionId { get; set; } = null!;
}
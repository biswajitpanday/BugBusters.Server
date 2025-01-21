using BugBusters.Server.Core.Entities;
using BugBusters.Server.Core.Interfaces.Common;

namespace BugBusters.Server.Core.Dtos;

public class QuestionCreateDto : IMapFrom<Question>
{
    public string Title { get; set; } = null!;
    public string Body { get; set; } = null!;
}
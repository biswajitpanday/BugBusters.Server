using OptiOverflow.Core.Entities;
using OptiOverflow.Core.Interfaces.Common;

namespace OptiOverflow.Core.Dtos;

public class QuestionDto : IMapFrom<Question>
{
    public Guid Id { get; set; }
    public string Title { get; set; } = null!;
    public string Body { get; set; } = null!;
    public Guid CreatedById { get; set; }
    public Guid LastUpdatedById { get; set; }
    public DateTime CreatedAt { get; set; }
    public int UpVoteCount { get; set; }
    public int DownVoteCount { get; set; }
    public List<AnswerDto?>? Answers { get; set; }
}
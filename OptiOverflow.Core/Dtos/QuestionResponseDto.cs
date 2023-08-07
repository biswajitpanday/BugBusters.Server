using OptiOverflow.Core.Entities;
using OptiOverflow.Core.Interfaces.Common;
using System.Text.Json.Serialization;

namespace OptiOverflow.Core.Dtos;

public class QuestionResponseDto : IMapFrom<Question>
{
    public Guid Id { get; set; }
    public string Title { get; set; } = null!;
    public string Body { get; set; } = null!;
    public Guid CreatedById { get; set; }
    public Guid LastUpdatedById { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime LastUpdated { get; set; }
    public int UpVoteCount { get; set; }
    public int DownVoteCount { get; set; }
    public int AnswerCount { get; set; }
    public bool HasAcceptedAnswer { get; set; }
    public required ProfileResponseDto CreatedBy { get; set; }
    public ProfileResponseDto? LastUpdatedBy { get; set; }
    [JsonIgnore]
    public List<AnswerResponseDto>? Answers { get; set; }
    public PagedResponse<List<AnswerResponseDto>?>? PagedAnswers { get; set; }
}
using OptiOverflow.Core.Entities;
using OptiOverflow.Core.Interfaces.Common;

namespace OptiOverflow.Core.Dtos;

public class AnswerCreateDto : IMapFrom<Answer>
{
    public string Body { get; set; } = null!;
    public string QuestionId { get; set; } = null!;
}
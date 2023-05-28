using OptiOverflow.Core.Entities;
using OptiOverflow.Core.Interfaces.Common;

namespace OptiOverflow.Core.Dtos;

public class QuestionCreateDto : IMapFrom<Question>
{
    public string Title { get; set; } = null!;
    public string Body { get; set; } = null!;
}
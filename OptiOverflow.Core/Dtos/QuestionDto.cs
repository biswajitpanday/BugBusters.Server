namespace OptiOverflow.Core.Dtos;

public class QuestionDto
{
    public string Title { get; set; } = null!;
    public string Body { get; set; } = null!;
    public Guid CreatedById { get; set; }
}
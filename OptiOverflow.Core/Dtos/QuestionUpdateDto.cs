namespace OptiOverflow.Core.Dtos;

public class QuestionUpdateDto
{
    public Guid? Id { get; set; }
    public string Title { get; set; } = null!;
    public string Body { get; set; } = null!;
}
using OptiOverflow.Core.Dtos;

namespace OptiOverflow.Core.Interfaces.Services;

public interface IQuestionService
{
    Task<List<QuestionDto>> GetAll();
    Task<QuestionDto?> GetById(Guid id);
    Task<QuestionDto> Create(QuestionCreateDto question, Guid userId);
    Task<QuestionDto?> Update(QuestionUpdateDto questionUpdateDto, Guid id, Guid userId);
    Task Delete(Guid id);
}
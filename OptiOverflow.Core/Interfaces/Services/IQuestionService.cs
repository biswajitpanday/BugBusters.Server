using OptiOverflow.Core.Dtos;

namespace OptiOverflow.Core.Interfaces.Services;

public interface IQuestionService
{
    Task<List<QuestionResponseDto>?> GetAll();
    Task<PagedResponse<List<QuestionResponseDto>>?> GetAll(PagedRequest pagedRequest);
    Task<QuestionResponseDto?> GetById(Guid id);
    Task<QuestionResponseDto> Create(QuestionCreateDto question, Guid userId);
    Task<QuestionResponseDto?> Update(QuestionUpdateDto questionUpdateDto, Guid id, Guid userId);
    Task Delete(Guid id);
}
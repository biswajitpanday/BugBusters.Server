using OptiOverflow.Core.Dtos;

namespace OptiOverflow.Core.Interfaces.Services;

public interface IAnswerService
{
    Task<AnswerDto> Create(AnswerCreateDto answer, Guid userId);
    Task<AnswerDto?> Update(AnswerUpdateDto answer, Guid id, Guid userId);
    Task Delete(Guid id);
}
using OptiOverflow.Core.Dtos;

namespace OptiOverflow.Core.Interfaces.Services;

public interface IAnswerService
{
    Task<AnswerResponseDto> Create(AnswerCreateDto answer, Guid userId);
    Task<AnswerResponseDto?> Update(AnswerUpdateDto answer, Guid id, Guid userId);
    Task Delete(Guid id);
}
using BugBusters.Server.Core.Dtos;

namespace BugBusters.Server.Core.Interfaces.Services;

public interface IAnswerService
{
    Task<AnswerResponseDto?> Accept(Guid id, Guid userId);
    Task<AnswerResponseDto> Create(AnswerCreateDto answer, Guid userId);
    Task<AnswerResponseDto?> Update(AnswerUpdateDto answer, Guid id, Guid userId);
    Task Delete(Guid id);
}
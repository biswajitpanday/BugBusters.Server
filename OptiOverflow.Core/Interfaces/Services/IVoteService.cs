using OptiOverflow.Core.Dtos;

namespace OptiOverflow.Core.Interfaces.Services;

public interface IVoteService
{
    Task<VoteResponseDto?> Create(VoteCreateDto vote, Guid userId);
}
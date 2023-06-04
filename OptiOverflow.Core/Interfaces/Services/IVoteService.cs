using OptiOverflow.Core.Dtos;

namespace OptiOverflow.Core.Interfaces.Services;

public interface IVoteService
{
    Task<VoteDto?> Create(VoteCreateDto vote, Guid userId);
}
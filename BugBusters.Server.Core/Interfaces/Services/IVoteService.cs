using BugBusters.Server.Core.Dtos;

namespace BugBusters.Server.Core.Interfaces.Services;

public interface IVoteService
{
    Task<VoteResponseDto?> Create(VoteCreateDto vote, Guid userId);
}
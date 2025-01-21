using AutoMapper;
using BugBusters.Server.Core.Dtos;
using BugBusters.Server.Core.Entities;
using BugBusters.Server.Core.Interfaces.Common;
using BugBusters.Server.Core.Interfaces.Repositories;
using BugBusters.Server.Core.Interfaces.Services;
using Microsoft.AspNetCore.Identity;

namespace BugBusters.Server.Service;

public class VoteService : IVoteService
{
    private readonly IMapper _mapper;
    private readonly IVoteRepository _voteRepository;
    private readonly ICurrentUserService _currentUserService;
    private readonly UserManager<ApplicationUser> _userManager;

    public VoteService(IMapper mapper,
        IVoteRepository voteRepository,
        ICurrentUserService currentUserService,
        UserManager<ApplicationUser> userManager)
    {
        _mapper = mapper;
        _voteRepository = voteRepository;
        _currentUserService = currentUserService;
        _userManager = userManager;
    }

    public async Task<VoteResponseDto?> Create(VoteCreateDto vote, Guid userId)
    {
        var voteAlreadyExists = false;
        if (vote.QuestionId != null)
            voteAlreadyExists =
                await _voteRepository.AnyAsync(x => x.UserId == userId && x.QuestionId == vote.QuestionId);
        else if (vote.AnswerId != null)
            voteAlreadyExists =
                await _voteRepository.AnyAsync(x => x.UserId == userId && x.AnswerId == vote.AnswerId);

        if (voteAlreadyExists)
            return null;
        var voteEntity = _mapper.Map<Vote>(vote);
        voteEntity.UserId = userId;
        voteEntity.IsUpVote = vote.IsUpVote;
        await _voteRepository.AddAsync(voteEntity);
        await _voteRepository.SaveChangesAsync();
        return _mapper.Map<VoteResponseDto>(voteEntity);
    }
}
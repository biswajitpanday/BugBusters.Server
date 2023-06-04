using AutoMapper;
using Microsoft.AspNetCore.Identity;
using OptiOverflow.Core.Dtos;
using OptiOverflow.Core.Entities;
using OptiOverflow.Core.Interfaces.Common;
using OptiOverflow.Core.Interfaces.Repositories;
using OptiOverflow.Core.Interfaces.Services;

namespace OptiOverflow.Service;

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

    public async Task<VoteDto?> Create(VoteCreateDto vote, Guid userId)
    {
        var isAlreadyVoted = await _voteRepository.AnyAsync(x => x.UserId == userId && (x.QuestionId == vote.QuestionId || x.AnswerId == vote.AnswerId));
        if (isAlreadyVoted)
            return null;
        var voteEntity = _mapper.Map<Vote>(vote);
        voteEntity.UserId = userId;
        voteEntity.IsUpVote = vote.IsUpVote;
        await _voteRepository.AddAsync(voteEntity);
        await _voteRepository.SaveChangesAsync();
        return _mapper.Map<VoteDto>(voteEntity);
    }
}
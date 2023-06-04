using Microsoft.AspNetCore.Mvc;
using OptiOverflow.Core.Dtos;
using OptiOverflow.Core.Entities;
using OptiOverflow.Core.Interfaces.Common;
using OptiOverflow.Core.Interfaces.Services;

namespace OptiOverflow.Api.Controllers;

public class VoteController : BaseController
{
    private readonly ILogger<VoteController> _logger;
    private readonly IVoteService _voteService;
    private readonly ICurrentUserService _currentUserService;

    public VoteController(ILogger<VoteController> logger,
    IVoteService voteService, ICurrentUserService currentUserService)
    {
        _logger = logger;
        _voteService = voteService;
        _currentUserService = currentUserService;
    }
    [HttpPost]
    public async Task<ActionResult> Create([FromBody] VoteCreateDto vote)
    {
        var result = await _voteService.Create(vote, _currentUserService.UserId);
        if (result == null)
            return Conflict();
        return CreatedAtAction(nameof(Create), new { id = result.Id }, vote);
    }
}
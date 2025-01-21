using BugBusters.Server.Core.Dtos;
using BugBusters.Server.Core.Interfaces.Common;
using BugBusters.Server.Core.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;

namespace BugBusters.Server.Api.Controllers;

public class AnswerController : BaseController
{
    private readonly ILogger<AnswerController> _logger;
    private readonly IAnswerService _answerService;
    private readonly ICurrentUserService _currentUserService;

    public AnswerController(ILogger<AnswerController> logger,
        IAnswerService answerService,
        ICurrentUserService currentUserService)
    {
        _logger = logger;
        _answerService = answerService;
        _currentUserService = currentUserService;
    }

    [HttpPut("Accept/{id:guid}")]
    public async Task<IActionResult> Accept(Guid id)
    {
        var userId = _currentUserService.UserId;
        var response = await _answerService.Accept(id, userId);
        if (response == null)
            return NotFound();
        return Ok(response);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] AnswerCreateDto answer)
    {
        var userId = _currentUserService.UserId;
        var result = await _answerService.Create(answer, userId);
        return CreatedAtAction(nameof(Create), new { id = result.Id }, result);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(Guid id, AnswerUpdateDto answer)
    {
        var userId = _currentUserService.UserId;
        var response = await _answerService.Update(answer, id, userId);
        if (response == null)
            return NotFound();
        return Ok(response);
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        await _answerService.Delete(id);
        return NoContent();
    }
}
﻿using Microsoft.AspNetCore.Mvc;
using OptiOverflow.Core.Dtos;
using OptiOverflow.Core.Interfaces.Common;
using OptiOverflow.Core.Interfaces.Services;

namespace OptiOverflow.Api.Controllers;

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


    [HttpPost]
    public async Task<IActionResult> Create([FromBody] AnswerCreateDto answer)
    {
        var userId = _currentUserService.UserId;
        var result = await _answerService.Create(answer, userId);
        return CreatedAtAction(nameof(Create), new { id = result.Id }, answer);
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
using BugBusters.Server.Core.Dtos;
using BugBusters.Server.Core.Interfaces.Common;
using BugBusters.Server.Core.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BugBusters.Server.Api.Controllers;

[Authorize]
public class QuestionController : BaseController
{
    private readonly IQuestionService _questionService;
    private readonly ICurrentUserService _currentUserService;

    public QuestionController(IQuestionService questionService, ICurrentUserService currentUserService)
    {
        _questionService = questionService;
        _currentUserService = currentUserService;
    }

    [HttpGet]
    public async Task<ActionResult> Get([FromQuery] PagedRequest pagedRequest)
    {
        var questions = await _questionService.GetAll(pagedRequest);
        if (questions == null)
            return NotFound();
        return Ok(questions);
    }

    [HttpGet("{id:guid}")]
    public async Task<ActionResult> Get(Guid id, [FromQuery] PagedRequest pagedRequest)
    {
        var question = await _questionService.GetById(id, pagedRequest);
        if (question == null)
            return NotFound();
        return Ok(question);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] QuestionCreateDto question)
    {
        var userId = _currentUserService.UserId;
        var result = await _questionService.Create(question, userId);
        return CreatedAtAction(nameof(Create), new { id = result.Id }, result);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(Guid id, QuestionUpdateDto question)
    {
        var userId = _currentUserService.UserId;
        var response = await _questionService.Update(question, id, userId);
        if (response == null)
            return NotFound();
        return Ok(response);
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        await _questionService.Delete(id);
        return NoContent();
    }

}
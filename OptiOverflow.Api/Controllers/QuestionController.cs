using Microsoft.AspNetCore.Mvc;
using OptiOverflow.Core.Dtos;
using OptiOverflow.Core.Interfaces.Common;
using OptiOverflow.Core.Interfaces.Services;

namespace OptiOverflow.Api.Controllers;

public class QuestionController: BaseController
{
    private readonly ILogger<QuestionController> _logger;
    private readonly IQuestionService _questionService;
    private readonly ICurrentUserService _currentUserService;

    public QuestionController(ILogger<QuestionController> logger, 
        IQuestionService questionService, ICurrentUserService currentUserService)
    {
        _logger = logger;
        _questionService = questionService;
        _currentUserService = currentUserService;
    }

    [HttpGet]
    public async Task<ActionResult> Get()
    {
        var questions = await _questionService.GetAll();
        return Ok(questions);
    }

    [HttpGet("{id:guid}")]
    public IActionResult Get(Guid id)
    {
        var question = _questionService.GetById(id);
        if (question == null)
            return NotFound();
        return Ok(question);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] QuestionCreateDto question)
    {
        var userId = _currentUserService.UserId;
        var result = await _questionService.Create(question, userId);
        return CreatedAtAction(nameof(Create), new { id = result.Id }, question);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(Guid id, QuestionUpdateDto question)
    {
        var userId = _currentUserService.UserId;
        var response = await _questionService.Update(question, id, userId);
        if (response == null)
            return NotFound();
        return Ok(question);
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        await _questionService.Delete(id);
        return NoContent();
    }

}
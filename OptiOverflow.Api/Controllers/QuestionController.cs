using System.Security.Claims;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OptiOverflow.Core.Dtos;
using OptiOverflow.Core.Entities;
using OptiOverflow.Core.Interfaces.Services;

namespace OptiOverflow.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class QuestionController: ControllerBase
{
    private readonly ILogger<QuestionController> _logger;
    private readonly IQuestionService _questionService;
    private readonly UserManager<ApplicationUser> _userManager;

    public QuestionController(ILogger<QuestionController> logger, 
        IQuestionService questionService, UserManager<ApplicationUser> userManager)
    {
        _logger = logger;
        _questionService = questionService;
        _userManager = userManager;
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
    public async Task<IActionResult> Create(QuestionCreateDto question)
    {
        var userId = User.FindFirstValue(ClaimTypes.Name);
        var result = await _questionService.Create(question, userId);
        return CreatedAtAction(nameof(Create), new { id = result.Id }, question);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(Guid id, QuestionUpdateDto question)
    {
        var userId = User.FindFirstValue(ClaimTypes.Email);
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
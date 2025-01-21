using BugBusters.Server.Core.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BugBusters.Server.Api.Controllers;

//[Authorize]
public class JiraController : BaseController
{
    private readonly IJiraService _jiraService;

    public JiraController(IJiraService jiraService)
    {
        _jiraService = jiraService;
    }

    [HttpGet]
    public async Task<ActionResult> Get()
    {
        var jiraTicket = await _jiraService.GetTicketAsync();
        if (jiraTicket == null)
            return NotFound();
        return Ok(jiraTicket);
    }
}
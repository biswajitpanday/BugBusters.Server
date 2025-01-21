using Microsoft.AspNetCore.Mvc;

namespace BugBusters.Server.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class BaseController : ControllerBase
{

    public BaseController()
    {
    }
}
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using OptiOverflow.Core.Constants;
using OptiOverflow.Core.Entities;
using OptiOverflow.Core.Interfaces.Services;

namespace OptiOverflow.Api.Controllers
{
    [Authorize(Policy = PolicyConstants.ApplicationAdmin)]
    public class UserController : BaseController
    {
        private readonly ILogger<UserController> _logger;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IUserService _userService;

        public UserController(ILogger<UserController> logger, UserManager<ApplicationUser> userManager,
            IUserService userService)
        {
            _logger = logger;
            _userManager = userManager;
            _userService = userService;
        }


        [HttpGet]
        public async Task<ActionResult> Get()
        {
            var users = await _userService.Get(); // @todo
            if (!users.Any())
                return NotFound();
            return Ok(users);
        }

        [HttpGet("{id:guid}")]
        public async Task<ActionResult> Get(Guid id)
        {
            var user = await _userManager.FindByIdAsync(id.ToString());
            if (user == null)
                return NotFound();
            return Ok(user);
        }

        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var user = await _userManager.FindByIdAsync(id.ToString());
            if (user == null)
                return NotFound();
            await _userManager.DeleteAsync(user);
            return NoContent();
        }
    }
}

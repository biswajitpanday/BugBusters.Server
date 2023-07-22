using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using OptiOverflow.Core.Constants;
using OptiOverflow.Core.Entities;
using OptiOverflow.Core.Interfaces.Common;
using OptiOverflow.Core.Interfaces.Services;

namespace OptiOverflow.Api.Controllers
{
    [Authorize(Policy = PolicyConstants.ApplicationAdmin)]
    public class UserController : BaseController
    {
        private readonly ILogger<UserController> _logger;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IUserService _userService;
        private readonly ICurrentUserService _currentUserService;

        public UserController(ILogger<UserController> logger, 
            UserManager<ApplicationUser> userManager,
            IUserService userService,
            ICurrentUserService currentUserService)
        {
            _logger = logger;
            _userManager = userManager;
            _userService = userService;
            _currentUserService = currentUserService;
        }

        [HttpGet("profile")]
        public async Task<ActionResult> Profile()
        {
            var profile = await _userService.Profile(_currentUserService.UserId);
            return Ok(profile);
        }


        [HttpGet]
        public async Task<ActionResult> Get()
        {
            var users = await _userService.Get();
            if (users != null && !users.Any())
                return NotFound();
            return Ok(users);
        }

        [HttpGet("{id:guid}")]
        public async Task<ActionResult> Get(Guid id)
        {
            var user = await _userService.GetById(id);
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
            await _userManager.DeleteAsync(user);   // Todo: Soft Delete Account. Need to SoftDelete the Profile
            return NoContent();
        }
    }
}

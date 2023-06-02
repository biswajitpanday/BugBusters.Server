using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using OptiOverflow.Core.Dtos;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Authorization;
using OptiOverflow.Core.Constants;
using OptiOverflow.Core.Entities;
using OptiOverflow.Core.Interfaces.Services;

namespace OptiOverflow.Api.Controllers;


[AllowAnonymous]
public class AuthController: BaseController
{
    private readonly ILogger<AuthController> _logger;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly RoleManager<IdentityRole<Guid>> _roleManager;
    private readonly IUserProfileService _userProfileService;
    private readonly IConfiguration _configuration;

    public AuthController(ILogger<AuthController> logger, 
        UserManager<ApplicationUser> userManager,
        RoleManager<IdentityRole<Guid>> roleManager,
        IUserProfileService userProfileService,
        IConfiguration configuration)
    {
        _logger = logger;
        _userManager = userManager;
        _roleManager = roleManager;
        _userProfileService = userProfileService;
        _configuration = configuration;
    }

    [HttpPost]
    [Route("login")]
    public async Task<IActionResult> Login([FromBody] LoginDto model)
    {
        var user = await _userManager.FindByNameAsync(model.UserName);
        if (user != null && await _userManager.CheckPasswordAsync(user, model.Password))
        {
            if (user.UserName == null) return Unauthorized();
            
            var userRoles = await _userManager.GetRolesAsync(user);
            var token = GenerateToken(user, userRoles);

            return Ok(new
            {
                token = new JwtSecurityTokenHandler().WriteToken(token),
                expiration = token.ValidTo,
                isActivated = true,
                profile = await _userProfileService.Get(user),
                role = userRoles.FirstOrDefault()
            });
        }
        return Unauthorized();
    }


    [HttpPost]
    [Route("register")]
    public async Task<IActionResult> Register([FromBody] RegistrationDto model)
    {
        var existingUser = await _userManager.FindByNameAsync(model.UserName);
        if (existingUser != null)
            return StatusCode(StatusCodes.Status500InternalServerError,
                new ApiResponseDto<object> { IsSuccess = false, Message = "User already exists" });
        ApplicationUser user = new()
        {
            Email = model.Email,
            SecurityStamp = Guid.NewGuid().ToString(),
            UserName = model.UserName,
            CreatedAt = DateTime.UtcNow,
            LastUpdate = DateTime.UtcNow,
            IsDeleted = false,
        };
        var result = await _userManager.CreateAsync(user, model.Password);
        if (!result.Succeeded)
            return StatusCode(StatusCodes.Status500InternalServerError,
                new ApiResponseDto<object>
                { IsSuccess = false, Message = "User creation failed! Please try again later" });
        await AssignRole(user, UserRoles.User);
        await _userProfileService.Create(model, user);
        return Ok(new ApiResponseDto<object> { IsSuccess = true, Message = "User created successfully" });
    }

    [HttpPost]
    [Route("register-admin")]
    public async Task<IActionResult> RegisterAdmin([FromBody] RegistrationDto model)
    {
        // Todo: Add Some sorts of verification process so that not everyone can register as Admin.
        var existingUser = await _userManager.FindByNameAsync(model.UserName);
        if (existingUser != null)
            return StatusCode(StatusCodes.Status500InternalServerError,
                new ApiResponseDto<object> { IsSuccess = false, Message = "Admin already exists" });
        ApplicationUser user = new()
        {
            Email = model.Email,
            SecurityStamp = Guid.NewGuid().ToString(),
            UserName = model.UserName,
            CreatedAt = DateTime.UtcNow,
            LastUpdate = DateTime.UtcNow,
            IsDeleted = false,
        };
        var result = await _userManager.CreateAsync(user, model.Password);
        if (!result.Succeeded)
            return StatusCode(StatusCodes.Status500InternalServerError,
                new ApiResponseDto<object>
                { IsSuccess = false, Message = "Admin creation failed! Please try again later" });

        await AssignRole(user, UserRoles.Admin);
        await _userProfileService.Create(model, user);
        return Ok(new ApiResponseDto<object> { IsSuccess = true, Message = "Admin created successfully" });
    }



    #region Private Methods

    private async Task AssignRole(ApplicationUser user, string role)
    {
        if (await _roleManager.RoleExistsAsync(role))
            await _userManager.AddToRoleAsync(user, role);
    }

    private JwtSecurityToken GetToken(IEnumerable<Claim> authClaims)
    {
        var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:Secret"] ?? string.Empty));
        var token = new JwtSecurityToken(
            issuer: _configuration["JWT:ValidIssuer"],
            audience: _configuration["JWT:ValidAudience"],
            expires: DateTime.Now.AddHours(24),
            claims: authClaims,
            signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256)
        );
        return token;
    }

    private JwtSecurityToken GenerateToken(ApplicationUser user, IList<string> userRoles)
    {
        var authClaims = new List<Claim>
        {
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new Claim(ClaimTypes.Email, user.Email),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
        };
        authClaims.AddRange(userRoles.Select(userRole => new Claim(ClaimTypes.Role, userRole)));

        var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:Secret"]));
        var cred = new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            _configuration["JWT:ValidIssuer"],
            _configuration["JWT:ValidAudience"],
            expires: DateTime.Now.AddMinutes(Convert.ToDouble((_configuration["JWT:ExpireInMinutes"]))),
            claims: authClaims,
            signingCredentials: cred
        );
        return token;
    }
    #endregion
}
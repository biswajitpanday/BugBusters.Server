using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using OptiOverflow.Core.Dtos;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using OptiOverflow.Core.Constants;
using OptiOverflow.Core.Entities;
using OptiOverflow.Core.Interfaces.Services;

namespace OptiOverflow.Api.Controllers;

[AllowAnonymous]
public class AuthController : BaseController
{
    private readonly ILogger<AuthController> _logger;
    private readonly IMapper _mapper;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly RoleManager<IdentityRole<Guid>> _roleManager;
    private readonly IConfiguration _configuration;

    public AuthController(ILogger<AuthController> logger,
        IMapper mapper,
        UserManager<ApplicationUser> userManager,
        RoleManager<IdentityRole<Guid>> roleManager,
        IConfiguration configuration)
    {
        _logger = logger;
        _mapper = mapper;
        _userManager = userManager;
        _roleManager = roleManager;
        _configuration = configuration;
    }

    [HttpPost]
    [Route("login")]
    public async Task<IActionResult> Login(LoginDto model)
    {
        var user = await _userManager.FindByEmailAsync(model.Email);
        if (user != null && await _userManager.CheckPasswordAsync(user, model.Password))
        {
            if (user.Email == null) return Unauthorized();
            return await CreateAuthResponse(user);
        }
        return Unauthorized();
    }

    [HttpPost]
    [Route("register")]
    public async Task<IActionResult> Register([FromBody] RegistrationDto model)
    {
        var existingUserByEmail = await _userManager.FindByEmailAsync(model.Email);
        if (existingUserByEmail != null)
            return StatusCode(StatusCodes.Status500InternalServerError,
                new ApiResponseDto<object> { IsSuccess = false, Message = "Email already exists" });
        var existingUserByUserName = await _userManager.FindByNameAsync(model.UserName);
        if (existingUserByUserName != null)
            return StatusCode(StatusCodes.Status500InternalServerError,
                new ApiResponseDto<object> { IsSuccess = false, Message = "Username already exists" });

        var user = _mapper.Map<ApplicationUser>(model);
        var result = await _userManager.CreateAsync(user, model.Password);
        if (!result.Succeeded)
            return StatusCode(StatusCodes.Status500InternalServerError,
                new ApiResponseDto<object>
                { IsSuccess = false, Message = "User creation failed! Please try again later" });
        await AssignRole(user, UserRoles.User);
        return await CreateAuthResponse(user);
    }

    [HttpPost]
    [Route("register-admin")]
    public async Task<IActionResult> RegisterAdmin([FromBody] RegistrationDto model)
    {
        // Todo: Add Some sorts of verification process so that not everyone can register as Admin.
        var existingUserByEmail = await _userManager.FindByEmailAsync(model.Email);
        if (existingUserByEmail != null)
            return StatusCode(StatusCodes.Status500InternalServerError,
                new ApiResponseDto<object> { IsSuccess = false, Message = "Email already exists" });
        var existingUserByUserName = await _userManager.FindByNameAsync(model.UserName);
        if (existingUserByUserName != null)
            return StatusCode(StatusCodes.Status500InternalServerError,
                new ApiResponseDto<object> { IsSuccess = false, Message = "Username already exists" });

        var user = _mapper.Map<ApplicationUser>(model);
        var result = await _userManager.CreateAsync(user, model.Password);
        if (!result.Succeeded)
            return StatusCode(StatusCodes.Status500InternalServerError,
                new ApiResponseDto<object>
                { IsSuccess = false, Message = "Admin creation failed! Please try again later" });

        await AssignRole(user, UserRoles.Admin);
        return await CreateAuthResponse(user);
    }


    // Todo: Forget Password
    // Todo: Reset Password


    #region Private Methods

    private async Task<IActionResult> CreateAuthResponse(ApplicationUser user)
    {
        var userRoles = await _userManager.GetRolesAsync(user);
        var token = GenerateToken(user, userRoles);
        var profileData = new
        {
            firstName = user.FirstName,
            middleName = user.MiddleName,
            lastName = user.LastName,
            email = user.Email,
            phoneNumber = user.PhoneNumber,
            dateOfBirth = user.DateOfBirth
        };
        return Ok(new
        {
            token = new JwtSecurityTokenHandler().WriteToken(token),
            role = userRoles.Select(x => x).FirstOrDefault(),
            isActivated = true,
            profileData
        });
    }

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
            new Claim(ClaimTypes.Name, user.UserName),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
        };
        authClaims.AddRange(userRoles.Select(userRole => new Claim(ClaimTypes.Role, userRole)));

        var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:Secret"]!));
        var cred = new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            _configuration["JWT:ValidIssuer"],
            _configuration["JWT:ValidAudience"],
            expires: DateTime.UtcNow.AddMinutes(Convert.ToDouble((_configuration["JWT:ExpireInMinutes"]))),
            claims: authClaims,
            signingCredentials: cred
        );
        return token;
    }
    #endregion
}
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using AutoMapper;
using BugBusters.Server.Core.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using Moq;
using OptiOverflow.Api.Controllers;
using OptiOverflow.Core.Dtos;

namespace BugBusters.Server.UnitTest;


[TestFixture]
public class AuthUnitTest
{
    private AuthController _authController;
    private Mock<ILogger<AuthController>> _mockLogger;
    private Mock<IMapper> _mockMapper;
    private Mock<UserManager<ApplicationUser>> _mockUserManager;
    private Mock<RoleManager<IdentityRole<Guid>>> _mockRoleManager;
    private Mock<IConfiguration> _mockConfiguration;

    [SetUp]
    public void Setup()
    {
        var applicationUser = new ApplicationUser { Id = Guid.Parse("67497637-3A50-4FD8-9C9D-08DB6EC33FB2"), Email = "admin001@example.com", UserName = "admin001" };
        var role = new IdentityRole<Guid> { Id = Guid.NewGuid(), Name = "Admin" };

        _mockLogger = new Mock<ILogger<AuthController>>();
        _mockMapper = new Mock<IMapper>();
        //_mockUserManager = new Mock<UserManager<ApplicationUser>>(Mock.Of<IUserStore<ApplicationUser>>(), null!, null!, null!, null!, null!, null!, null!, null!);
        _mockUserManager = GetUserManagerMock(applicationUser);
        //_mockRoleManager = new Mock<RoleManager<IdentityRole<Guid>>>(Mock.Of<IRoleStore<IdentityRole<Guid>>>(), null!, null!, null!, null!);
        _mockRoleManager = GetRoleManagerMock(role);
        _mockConfiguration = new Mock<IConfiguration>();

        _authController = new AuthController(_mockLogger.Object, _mockMapper.Object, _mockUserManager.Object, _mockRoleManager.Object, _mockConfiguration.Object);
    }

    private Mock<UserManager<ApplicationUser>> GetUserManagerMock(ApplicationUser user)
    {
        var store = new Mock<IUserStore<ApplicationUser>>();
        var userManager = new Mock<UserManager<ApplicationUser>>(store.Object, null, null, null, null, null, null, null, null);
        userManager.Setup(um => um.FindByEmailAsync(It.IsAny<string>())).ReturnsAsync(user);
        userManager.Setup(um => um.CheckPasswordAsync(user, It.IsAny<string>())).ReturnsAsync(true);
        return userManager;
    }

    private Mock<RoleManager<IdentityRole<Guid>>> GetRoleManagerMock(IdentityRole<Guid> role)
    {
        var store = new Mock<IRoleStore<IdentityRole<Guid>>>();
        var roleManager = new Mock<RoleManager<IdentityRole<Guid>>>(store.Object, null, null, null, null);
        roleManager.Setup(rm => rm.FindByNameAsync(It.IsAny<string>())).ReturnsAsync(role);
        return roleManager;
    }

    private JwtSecurityToken GenerateMockToken()
    {
        // Create a mock token for testing purposes
        var authClaims = new List<Claim>
        {
            new Claim(ClaimTypes.NameIdentifier, Guid.NewGuid().ToString()),
            new Claim(ClaimTypes.Email, "admin@example.com"),
            // Add more claims...
        };

        var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("your-mock-secret-key"));
        var cred = new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            "mock-issuer",
            "mock-audience",
            expires: DateTime.UtcNow.AddMinutes(30),
            claims: authClaims,
            signingCredentials: cred
        );
        return token;
    }

    [Test]
    public async Task Login_ShouldReturnSuccess()
    {
        // Arrange
        var loginDto = new LoginDto
        {
            Email = "admin001@example.com",
            Password = "123456"
        };

        // Act
        var result = await _authController.Login(loginDto) as OkObjectResult;

        // Assert
        Assert.NotNull(result);
        if (result != null)
            Assert.That(result.StatusCode, Is.EqualTo(200));
    }
}
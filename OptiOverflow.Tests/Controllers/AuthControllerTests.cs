using System.Net;
using System.Net.Http.Json;
using System.Text;
using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Moq;
using OptiOverflow.Api.Controllers;
using OptiOverflow.Core.Dtos;
using OptiOverflow.Core.Entities;

namespace OptiOverflow.Tests.Controllers;

[TestFixture]
public class AuthControllerTests
{
    private const string AuthBaseUrl = $"{TestUtility.BaseUrl}Auth/";

    private AuthController _authController;
    private Mock<ILogger<AuthController>> _mockLogger;
    private Mock<IMapper> _mockMapper;
    private Mock<UserManager<ApplicationUser>> _mockUserManager;
    private Mock<RoleManager<IdentityRole<Guid>>> _mockRoleManager;
    private Mock<IConfiguration> _mockConfiguration;

    
    private HttpClient _httpClient;

    [SetUp]
    public void Setup()
    {
        _mockLogger = new Mock<ILogger<AuthController>>();
        _mockMapper = new Mock<IMapper>();
        _mockUserManager = new Mock<UserManager<ApplicationUser>>(Mock.Of<IUserStore<ApplicationUser>>(), null, null, null, null, null, null, null, null);
        _mockRoleManager = new Mock<RoleManager<IdentityRole<Guid>>>(Mock.Of<IRoleStore<IdentityRole<Guid>>>(), null, null, null, null);
        _mockConfiguration = new Mock<IConfiguration>();

        _httpClient = new HttpClient();

        _authController = new AuthController(
            _mockLogger.Object, 
            _mockMapper.Object, 
            _mockUserManager.Object,
            _mockRoleManager.Object, 
            _mockConfiguration.Object);
    }

    //[Test]
    //public async Task CanLoginWithValidCredentials()
    //{
    //    // Arrange
    //    var loginDto = new LoginDto
    //    {
    //        Email = "admin001@example.com",
    //        Password = "123456"
    //    };

    //    // Act
    //    var request = new HttpRequestMessage(HttpMethod.Post, $"{AuthBaseUrl}Login");
    //    request.Content = JsonContent.Create(loginDto);
    //    var response = await _httpClient.SendAsync(request);

    //    // Assert
    //    Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
    //}

    //[Test]
    //public async Task CannotLoginWithInvalidCredentials()
    //{
    //    // Arrange
    //    var loginDto = new LoginDto
    //    {
    //        Email = "invalid@example.com",
    //        Password = "invalid"
    //    };

    //    // Act
    //    var request = new HttpRequestMessage(HttpMethod.Post, $"{AuthBaseUrl}Login");
    //    request.Content = JsonContent.Create(loginDto);
    //    var response = await _httpClient.SendAsync(request);

    //    // Assert
    //    Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.Unauthorized));
    //}


    [Test]
    public async Task Login_ShouldReturnSuccess()
    {
        // Arrange
        var userModel = new ApplicationUser { Email = "admin001@example.com" };
        _mockUserManager.Setup(u => u.FindByEmailAsync("admin001@example.com")).ReturnsAsync(userModel);
        _mockUserManager.Setup(u => u.CheckPasswordAsync(userModel, "123456")).ReturnsAsync(true);
        
        var loginDto = new LoginDto
        {
            Email = "admin001@example.com",
            Password = "123456"
        };

        // Act
        var result = await _authController.Login(loginDto) as ObjectResult;

        // Assert
        Assert.NotNull(result);
        Assert.AreEqual(200, result.StatusCode);
    }

    [Test]
    public async Task Login_ShouldReturnUnauthorized()
    {
        // Arrange
        _mockUserManager.Setup(u => u.FindByEmailAsync("invalid@example.com"))
            .ReturnsAsync((ApplicationUser)null);

        var loginDto = new LoginDto { Email = "invalid@example.com", Password = "invalidPassword" };

        // Act
        var result = await _authController.Login(loginDto) as UnauthorizedResult;

        // Assert
        Assert.NotNull(result);
    }
}
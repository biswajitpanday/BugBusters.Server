using System.Net;
using System.Net.Http.Json;
using BugBusters.Server.Core.Dtos;

namespace BugBusters.Server.IntegrationTest
{
    [TestFixture]
    public class AuthIntegrationTest
    {
        private const string AuthBaseUrl = $"{TestUtility.BaseUrl}Auth/";
        private HttpClient? _httpClient;

        [SetUp]
        public void Setup()
        {
            _httpClient = new HttpClient();
        }

        [Test]
        public async Task CanLoginWithValidCredentials()
        {
            // Arrange
            var loginDto = new LoginDto
            {
                Email = "admin001@example.com",
                Password = "123456"
            };

            // Act
            var request = new HttpRequestMessage(HttpMethod.Post, $"{AuthBaseUrl}Login");
            request.Content = JsonContent.Create(loginDto);
            var response = await _httpClient?.SendAsync(request);

            // Assert
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
        }

        [Test]
        public async Task CannotLoginWithInvalidCredentials()
        {
            // Arrange
            var loginDto = new LoginDto
            {
                Email = "invalid@example.com",
                Password = "invalid"
            };

            // Act
            var request = new HttpRequestMessage(HttpMethod.Post, $"{AuthBaseUrl}Login");
            request.Content = JsonContent.Create(loginDto);
            var response = await _httpClient.SendAsync(request);

            // Assert
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.Unauthorized));
        }
    }
}
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using WebApi.Common;
using WebApi.Models.Requests.Users;
using WebApi.Models.Responses.Users;
using WebApi.Tests.IntegrationTests.TestFixtures;
using Xunit;

namespace WebApi.Tests.IntegrationTests.ControllerTests
{
    public class AuthControllerTests :IClassFixture<TestFixture<Startup>>
    {
        private readonly HttpClient _client;

        public AuthControllerTests(TestFixture<Startup> fixture)
        {
            _client = fixture.Client;
        }

        [Fact]
        public async Task TestGetAuthToken()
        {
            var user = new AuthRequest
            {
                Username = Config.AdminName,
                Password = Config.AdminPwd
            };
            var response = await _client.PostAsJsonAsync("/api/v1/user/auth", user);
            response.EnsureSuccessStatusCode();
            var responseContent = await response.Content.ReadAsJsonAsync<AuthResponse>();
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.NotNull(responseContent.Username);
        }
    }
}

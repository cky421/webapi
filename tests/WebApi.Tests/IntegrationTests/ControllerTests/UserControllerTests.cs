using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
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
            Assert.NotNull(responseContent.Username);
        }

        [Fact]
        public async Task TestCreteaUser()
        {
            var user = new CreateUserRequest
            {
                Username = "TestUser1",
                Password = "Password"
            };
            var response = await _client.PostAsJsonAsync("/api/v1/user", user);
            response.EnsureSuccessStatusCode();
            var responseContent = await response.Content.ReadAsJsonAsync<UserResponse>();
            Assert.NotNull(responseContent.UserId);
        }

        [Fact]
        public async Task TestUpdateUser()
        {
            var createRequest = new CreateUserRequest
            {
                Username = "TestUser2",
                Password = "Password"
            };
            var response1 = await _client.PostAsJsonAsync("/api/v1/user", createRequest);
            response1.EnsureSuccessStatusCode();
            var responseContent1 = await response1.Content.ReadAsJsonAsync<UserResponse>();
            Assert.NotNull(responseContent1.UserId);

            var authRequest = new AuthRequest
            {
                Username = createRequest.Username,
                Password = createRequest.Password
            };

            var response2 = await _client.PostAsJsonAsync("/api/v1/user/auth", authRequest);
            response2.EnsureSuccessStatusCode();
            var responseContent2 = await response2.Content.ReadAsJsonAsync<AuthResponse>();
            Assert.NotNull(responseContent2.Token);

            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", responseContent2.Token);

            var updateRequest = new UpdateUserRequest
            {
                Password = "newPassword"
            };

            var response3 = await _client.PutAsJsonAsync("/api/v1/user", updateRequest);
            response3.EnsureSuccessStatusCode();

            var authRequest2 = new AuthRequest
            {
                Username = createRequest.Username,
                Password = updateRequest.Password
            };
            var response4 = await _client.PostAsJsonAsync("/api/v1/user/auth", authRequest2);
            response4.EnsureSuccessStatusCode();
            var responseContent4 = await response4.Content.ReadAsJsonAsync<AuthResponse>();
            Assert.NotNull(responseContent4.Token);
        }

        [Fact]
        public async Task TestBadRequest()
        {
            var authRequest = new AuthRequest
            {
                Username = "",
                Password = ""
            };
            var response = await _client.PostAsJsonAsync("/api/v1/user/auth", authRequest);
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }

        [Fact]
        public async Task TestErrorUsernameOrPassword()
        {
            var authRequest = new AuthRequest
            {
                Username = "TestUser3",
                Password = "Password"
            };
            var response = await _client.PostAsJsonAsync("/api/v1/user/auth", authRequest);
            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }
    }
}

using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using WebApi.Common;
using WebApi.Models.Mongodb;
using WebApi.Models.Requests.Groups;
using WebApi.Models.Requests.Passwords;
using WebApi.Models.Responses.Groups;
using WebApi.Models.Responses.Passwords;
using WebApi.Tests.IntegrationTests.TestFixtures;
using Xunit;
using static WebApi.Common.Helper;

namespace WebApi.Tests.IntegrationTests.ControllerTests
{
    public class PasswordControllerTests : IClassFixture<TestPasswordFixture>
    {
        private readonly HttpClient _client;

        public PasswordControllerTests(TestPasswordFixture fixture)
        {
            _client = fixture.Client;
            var admin = new User
            {
                Username = Config.AdminName,
                Password = Config.AdminPwd,
                UserId = fixture.AdminId
            };
            var token = GenerateToken(admin.UserId, admin.Username, DateTime.Now + TimeSpan.FromHours(1));
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        }

        [Fact]
        public async Task TestInsertPassword()
        {
            var createGroup = new CreateGroupRequest
            {
                GroupName = "NewGroup"
            };
            var createGroupResponse = await _client.PostAsJsonAsync("/api/v1/password/group", createGroup);
            createGroupResponse.EnsureSuccessStatusCode();
            var createGroupContent = await createGroupResponse.Content.ReadAsJsonAsync<GroupResponse>();

            Assert.NotNull(createGroupContent.GroupId);
            Assert.NotEmpty(createGroupContent.GroupId);

            var createPassword = new CreatePasswordRequest
            {
                GroupId = createGroupContent.GroupId,
                Title = "Test Password"
            };

            var createPasswordResponse = await _client.PostAsJsonAsync("/api/v1/password", createPassword);
            createPasswordResponse.EnsureSuccessStatusCode();
            var createPasswordContent = await createPasswordResponse.Content.ReadAsJsonAsync<PasswordResponse>();
            Assert.Equal(createPassword.Title, createPasswordContent.Title);

            var getPasswordResponse = await _client.GetAsync($"/api/v1/password/{createPasswordContent.PasswordId}");
            getPasswordResponse.EnsureSuccessStatusCode();
            var getPasswordContent = await getPasswordResponse.Content.ReadAsJsonAsync<PasswordResponse>();
            Assert.Equal(createPasswordContent.PasswordId, getPasswordContent.PasswordId);
        }

        [Fact]
        public async Task TestUpdatePassword()
        {
            var createGroup = new CreateGroupRequest
            {
                GroupName = "NewGroup2"
            };
            var createGroupResponse = await _client.PostAsJsonAsync("/api/v1/password/group", createGroup);
            createGroupResponse.EnsureSuccessStatusCode();
            var createGroupContent = await createGroupResponse.Content.ReadAsJsonAsync<GroupResponse>();

            Assert.NotNull(createGroupContent.GroupId);
            Assert.NotEmpty(createGroupContent.GroupId);

            var createPassword = new CreatePasswordRequest
            {
                GroupId = createGroupContent.GroupId,
                Title = "Test Password2"
            };

            var createPasswordResponse = await _client.PostAsJsonAsync("/api/v1/password", createPassword);
            createPasswordResponse.EnsureSuccessStatusCode();
            var createPasswordContent = await createPasswordResponse.Content.ReadAsJsonAsync<PasswordResponse>();
            Assert.Equal(createPassword.Title, createPasswordContent.Title);

            var updatePassword = new UpdatePasswordRequest
            {
                PasswordId = createPasswordContent.PasswordId,
                Title = "New Test Password",
                GroupId = createGroupContent.GroupId
            };
            var updatePasswordResponse = await _client.PutAsJsonAsync("/api/v1/password/", updatePassword);
            updatePasswordResponse.EnsureSuccessStatusCode();
            var updatePasswordContent = await updatePasswordResponse.Content.ReadAsJsonAsync<PasswordResponse>();
            Assert.Equal(updatePassword.Title, updatePasswordContent.Title);
        }

        [Fact]
        public async Task TestDeletePassword()
        {
            var createGroup = new CreateGroupRequest
            {
                GroupName = "NewGroup3"
            };
            var createGroupResponse = await _client.PostAsJsonAsync("/api/v1/password/group", createGroup);
            createGroupResponse.EnsureSuccessStatusCode();
            var createGroupContent = await createGroupResponse.Content.ReadAsJsonAsync<GroupResponse>();

            Assert.NotNull(createGroupContent.GroupId);
            Assert.NotEmpty(createGroupContent.GroupId);

            var createPassword = new CreatePasswordRequest
            {
                GroupId = createGroupContent.GroupId,
                Title = "Test Password3"
            };

            var createPasswordResponse = await _client.PostAsJsonAsync("/api/v1/password", createPassword);
            createPasswordResponse.EnsureSuccessStatusCode();
            var createPasswordContent = await createPasswordResponse.Content.ReadAsJsonAsync<PasswordResponse>();
            Assert.Equal(createPassword.Title, createPasswordContent.Title);
            Assert.NotEmpty(createPasswordContent.PasswordId);
            Assert.NotNull(createPasswordContent.PasswordId);

            var deletePasswordResponse = await _client.DeleteAsync($"/api/v1/password/{createPasswordContent.PasswordId}");
            deletePasswordResponse.EnsureSuccessStatusCode();
        }

        [Fact]
        public async Task TestFetchPassword()
        {
            var createGroup = new CreateGroupRequest
            {
                GroupName = "NewGroup4"
            };
            var createGroupResponse = await _client.PostAsJsonAsync("/api/v1/password/group", createGroup);
            createGroupResponse.EnsureSuccessStatusCode();
            var createGroupContent = await createGroupResponse.Content.ReadAsJsonAsync<GroupResponse>();

            Assert.NotNull(createGroupContent.GroupId);
            Assert.NotEmpty(createGroupContent.GroupId);

            var createPassword = new CreatePasswordRequest
            {
                GroupId = createGroupContent.GroupId,
                Title = "Test Password4"
            };

            var createPasswordResponse = await _client.PostAsJsonAsync("/api/v1/password", createPassword);
            createPasswordResponse.EnsureSuccessStatusCode();
            var createPasswordContent = await createPasswordResponse.Content.ReadAsJsonAsync<PasswordResponse>();
            Assert.Equal(createPassword.Title, createPasswordContent.Title);
            Assert.NotEmpty(createPasswordContent.PasswordId);
            Assert.NotNull(createPasswordContent.PasswordId);

            var fetchResponse = await _client.GetAsync($"/api/v1/password/{createGroupContent.GroupId}/all");
            fetchResponse.EnsureSuccessStatusCode();
            var fetchContent = await fetchResponse.Content.ReadAsJsonAsync<FetchPasswordsResponse>();
            Assert.Equal(1, fetchContent.Passwords.Count);
        }
    }
}
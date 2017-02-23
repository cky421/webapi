using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using WebApi.Common;
using WebApi.Models.Mongodb;
using WebApi.Models.Requests;
using WebApi.Models.Responses;
using WebApi.Tests.IntegrationTests.TestFixtures;
using Xunit;
using static WebApi.Common.Auth.ClaimsIdentityHelper;

namespace WebApi.Tests.IntegrationTests.ControllerTests
{
    public class GroupControllerTests : IClassFixture<TextGroupFixture>
    {
        private readonly HttpClient _client;

        public GroupControllerTests(TextGroupFixture fixture)
        {
            _client = fixture.Client;
            var admin = new User
            {
                Username = Config.AdminName,
                Password = Config.AdminPwd,
                UserId = fixture.AdminId
            };
            var token = GenerateToken(admin, DateTime.Now + TimeSpan.FromHours(1));
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

        }

        [Fact]
        public async Task TestInsertGroup()
        {
            var createGroupRequest = new CreateGroupRequest
            {
                GroupName = "TestGroup0"
            };
            var createResponse = await _client.PostAsJsonAsync("/api/v1/password/group", createGroupRequest);
            createResponse.EnsureSuccessStatusCode();
            var createResponseContent = await createResponse.Content.ReadAsJsonAsync<GroupResponse>();
            Assert.NotNull(createResponseContent.GroupId);
            Assert.NotEmpty(createResponseContent.GroupId);
            var getResponse = await _client.GetAsync($"/api/v1/password/group/{createResponseContent.GroupId}");
            getResponse.EnsureSuccessStatusCode();
            var getResponseContent = await getResponse.Content.ReadAsJsonAsync<GroupResponse>();
            Assert.Equal(createGroupRequest.GroupName, getResponseContent.GroupName);
        }

        [Fact]
        public async Task TestUpdateGroup()
        {
            var groupsResponse = await _client.GetAsync("/api/v1/password/group");
            groupsResponse.EnsureSuccessStatusCode();
            var groupsResponseContent = await groupsResponse.Content.ReadAsJsonAsync<FetchGroupResponse>();
            Assert.NotNull(groupsResponseContent?.Groups);
            Assert.NotEmpty(groupsResponseContent.Groups);
            var group = groupsResponseContent.Groups[0];
            var updateGroupRequest = new UpdateGroupRequest
            {
                GroupId = group.GroupId,
                NewGroupName = "UpdateGroupName"
            };
            var updateGroupResponse = await _client.PutAsJsonAsync("/api/v1/password/group/", updateGroupRequest);
            updateGroupResponse.EnsureSuccessStatusCode();
            var getResponse = await _client.GetAsync($"/api/v1/password/group/{group.GroupId}");
            getResponse.EnsureSuccessStatusCode();
            var getResponseContent = await getResponse.Content.ReadAsJsonAsync<GroupResponse>();
            Assert.Equal("UpdateGroupName", getResponseContent.GroupName);
        }

        [Fact]
        public async Task TestDeleteGroup()
        {
            var groupsResponse = await _client.GetAsync("/api/v1/password/group");
            groupsResponse.EnsureSuccessStatusCode();
            var groupsResponseContent = await groupsResponse.Content.ReadAsJsonAsync<FetchGroupResponse>();
            Assert.NotNull(groupsResponseContent?.Groups);
            Assert.NotEmpty(groupsResponseContent.Groups);
            var group = groupsResponseContent.Groups[0];
            var deleteResponse = await _client.DeleteAsync($"/api/v1/password/group/{group.GroupId}");
            deleteResponse.EnsureSuccessStatusCode();
            var getResponse = await _client.GetAsync($"/api/v1/password/group/{group.GroupId}");
            Assert.Equal(HttpStatusCode.NotFound, getResponse.StatusCode);
        }
    }
}
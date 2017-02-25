using System;
using Microsoft.Extensions.Logging;
using Moq;
using Newtonsoft.Json;
using WebApi.Common;
using WebApi.Controllers.V1;
using WebApi.Models.Requests;
using WebApi.Models.Responses;
using WebApi.Models.Responses.Users;
using WebApi.Repositories.Interfaces;
using Xunit;

namespace WebApi.Tests.UnitTests
{
    public class AuthControllerTests
    {
        [Fact]
        public void TestGetAuthToken()
        {
            var mockRepo = new Mock<IUserRepository>();
            var user = new UserResponse
            {
                UserId = Guid.NewGuid().ToString(),
                UserName = Config.AdminName,
                Result = Results.Succeed
            };

            mockRepo.Setup(repo => repo.Find(Config.AdminName, Config.AdminPwd)).Returns(user);
            var mockLog = new Mock<ILogger<AuthController>>();

            var auth = new AuthController(mockRepo.Object, mockLog.Object);
            var result = auth.Post(new AuthRequest
            {
                Username = Config.AdminName,
                Password = Config.AdminPwd
            });

            var response = JsonConvert.DeserializeObject<AuthResponse>(result);
            Assert.Null(response.Message);
            Assert.NotNull(response.Token);
        }
    }
}
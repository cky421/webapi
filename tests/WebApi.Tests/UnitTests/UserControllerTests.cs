using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using Newtonsoft.Json;
using WebApi.Common;
using WebApi.Controllers.V1;
using WebApi.Models.Requests.Users;
using WebApi.Models.Responses;
using WebApi.Models.Responses.Users;
using WebApi.Repositories.Interfaces;
using Xunit;
using static WebApi.Common.Helper;

namespace WebApi.Tests.UnitTests
{
    public class AuthControllerTests
    {
        [Fact]
        public void TestGetAuthToken()
        {
            var mockRepo = new Mock<IUserRepository>();
            var expiresIn = DateTime.Now + Config.ExpiresSpan;
            var userId = Guid.NewGuid().ToString();
            var accessToken = GenerateToken(userId, Config.AdminName, expiresIn);
            var builder = new AuthResponse.Builder();
            builder.SetExpire(Config.ExpiresSpan.TotalSeconds)
                .SetToken(accessToken)
                .SetUserId(Guid.NewGuid().ToString())
                .SetType(Config.TokenType)
                .SetUsername(Config.AdminName)
                .SetResult(Results.Succeed);

            mockRepo.Setup(repo => repo.Auth(Config.AdminName, Config.AdminPwd)).Returns(builder.Build());
            var mockLog = new Mock<ILogger<UserController>>();

            var userController = new UserController(mockRepo.Object, mockLog.Object);
            var result = userController.Auth(new AuthRequest
            {
                Username = Config.AdminName,
                Password = Config.AdminPwd
            });

            var okObjectResult = result as OkObjectResult;

            Assert.NotNull(okObjectResult);
            Assert.NotNull(okObjectResult.Value);
            var response = JsonConvert.DeserializeObject<AuthResponse>(okObjectResult.Value.ToString());
            Assert.NotNull(response.Token);
        }
    }
}
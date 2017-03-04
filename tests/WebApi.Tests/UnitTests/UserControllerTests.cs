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
using Microsoft.Extensions.Logging;
using Moq;
using Newtonsoft.Json;
using WebApi.Common;
using WebApi.Controllers;
using WebApi.Models;
using WebApi.Models.Mongodb;
using WebApi.Models.Responses;
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
            var user = new User
            {
                Username = Config.AdminName,
                Password = Config.AdminPwd
            };
            mockRepo.Setup(repo => repo.Find(Config.AdminName, Config.AdminPwd)).Returns(user);
            var mockLog = new Mock<ILogger<AuthController>>();

            var auth = new AuthController(mockRepo.Object, mockLog.Object);
            var result = auth.Post(user);

            var response = JsonConvert.DeserializeObject<Response<AuthResponse>>(result);
            Assert.Null(response.Message);
            Assert.NotNull(response.Data);
        }
    }
}
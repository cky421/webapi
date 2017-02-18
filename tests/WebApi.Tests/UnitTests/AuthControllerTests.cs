using Microsoft.Extensions.Logging;
using Moq;
using Newtonsoft.Json;
using WebApi.Common;
using WebApi.Controllers;
using WebApi.Models;
using WebApi.Repositories;
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

            var response = JsonConvert.DeserializeObject<Response>(result);
            Assert.Equal(ResponseState.Success, response.state);
        }
    }
}
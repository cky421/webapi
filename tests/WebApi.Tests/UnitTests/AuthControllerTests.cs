using Moq;
using Xunit;
using Microsoft.Extensions.Logging;
using WebApi.Repositories;
using WebApi.Controllers;
using WebApi.Common;
using WebApi.Models;
using Newtonsoft.Json;

namespace WebApi.Tests.UnitTest
{
    public class AuthControllerTests
    {
        [Fact]
        public void TestGetAuthToken()
        {
            var mockRepo = new Mock<IUserRepository>();
            User user = new User();
            user.Username = Config.AdminName;
            user.Password = Config.AdminPwd;
            mockRepo.Setup(repo => repo.Find(Config.AdminName, Config.AdminPwd)).Returns(user);
            var mockLog = new Mock<ILogger<AuthController>>();

            var auth = new AuthController(mockRepo.Object, mockLog.Object);
            var result = auth.POST(user);

            Response response = JsonConvert.DeserializeObject<Response>(result);
            Assert.Equal(ResponseState.Success, response.state);
        }
    }
}
using System.Net.Http;
using WebApi.Tests.IntegrationTests.TestFixtures;
using Xunit;

namespace WebApi.Tests.IntegrationTests.ControllerTests
{
    public class PasswordControllerTests : IClassFixture<TestPasswordFixture>
    {
        private readonly HttpClient _client;

        public PasswordControllerTests(TestFixture<Startup> fixture)
        {
            _client = fixture.Client;
        }
    }
}
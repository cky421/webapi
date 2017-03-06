using WebApi.Common;
using WebApi.Repositories;
using WebApi.Repositories.Interfaces;

namespace WebApi.Tests.IntegrationTests.TestFixtures
{
    public class TestPasswordFixture : TestFixture<Startup>
    {
        public string AdminId { get; set; }

        private readonly IGroupRepository _groupRepository;
        private readonly IPasswordRepository _passwordRepository;

        public TestPasswordFixture() : base()
        {
            IUserRepository userRepository = new UserRepository();
            var admin = userRepository.Find(Config.AdminName, Config.AdminPwd);
            AdminId = admin.UserId;

            _groupRepository = new GroupRepository();
            _groupRepository.InsertGroup(AdminId, "TestGroup1");
            _groupRepository.InsertGroup(AdminId, "TestGroup2");
            _groupRepository.InsertGroup(AdminId, "TestGroup3");

            _passwordRepository = new PasswordRepository();
        }

        public override void Dispose()
        {
            base.Dispose();
            _groupRepository.Clear(AdminId);
            _passwordRepository.Clear(AdminId);
        }
    }
}
using WebApi.Common;
using WebApi.Repositories;
using WebApi.Repositories.Interfaces;

namespace WebApi.Tests.IntegrationTests.TestFixtures
{
    public class TextGroupFixture : TestFixture<Startup>
    {
        private readonly IGroupRepository _groupRepository;

        public string AdminId { get; set; }

        public TextGroupFixture() : base()
        {
            IUserRepository userRepository = new UserRepository();
            var admin = userRepository.Find(Config.AdminName, Config.AdminPwd);
            AdminId = admin.UserId;

            _groupRepository = new GroupRepository();
            _groupRepository.InsertGroup("TestGroup1", AdminId);
            _groupRepository.InsertGroup("TestGroup2", AdminId);
            _groupRepository.InsertGroup("TestGroup3", AdminId);
        }

        public override void Dispose()
        {
            base.Dispose();
            _groupRepository.Clear(AdminId);
        }
    }
}
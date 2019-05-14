using System.Linq;
using Xunit;

namespace Bootstrap.DataAccess
{
    [Collection("SQLServerContext")]
    public class GroupsTest
    {
        [Fact]
        public void Retrieves_Ok()
        {
            Assert.NotEmpty(GroupHelper.Retrieves());
        }

        [Fact]
        public void SaveAndDelete_Ok()
        {
            Group g = new Group() { GroupName = "UnitTest", Description = "UnitTestSave" };
            Assert.True(GroupHelper.Save(g));

            var ids = GroupHelper.Retrieves().Where(t => t.GroupName == "UnitTest").Select(t => t.Id);
            Assert.True(GroupHelper.Delete(ids));
        }

        [Fact]
        public void RetrievesByRoleId_Ok()
        {
            var groups = GroupHelper.RetrievesByRoleId(RoleHelper.Retrieves().Where(r => r.RoleName == "Administrators").First().Id);
            Assert.NotEmpty(groups);
        }

        [Fact]
        public void RetrievesByUserId_Ok()
        {
            var userId = UserHelper.Retrieves().FirstOrDefault(r => r.UserName == "Admin").Id;
            var groups = GroupHelper.RetrievesByUserId(userId);
            Assert.NotNull(groups);
        }

        [Fact]
        public void SaveByUserId_Ok()
        {
            var userId = UserHelper.Retrieves().FirstOrDefault(r => r.UserName == "Admin").Id;
            Assert.True(GroupHelper.SaveByUserId(userId, GroupHelper.Retrieves().Select(g => g.Id)));
        }

        [Fact]
        public void SaveByRoleId_Ok()
        {
            var roleId = RoleHelper.Retrieves().FirstOrDefault(r => r.RoleName == "Administrators").Id;
            Assert.True(GroupHelper.SaveByRoleId(roleId, GroupHelper.Retrieves().Select(g => g.Id)));
        }

        [Fact]
        public void RetrievesByUserName_Ok()
        {
            Assert.NotNull(GroupHelper.RetrievesByUserName("Admin"));
        }
    }
}

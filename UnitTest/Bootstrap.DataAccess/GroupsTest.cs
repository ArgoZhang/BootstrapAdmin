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
            Group g = new Group();
            Assert.NotEmpty(g.Retrieves());
        }

        [Fact]
        public void SaveAndDelete_Ok()
        {
            Group g = new Group() { GroupName = "UnitTest", Description = "UnitTestSave" };
            Assert.True(g.Save(g));

            var ids = g.Retrieves().Where(t => t.GroupName == "UnitTest").Select(t => t.Id);
            Assert.True(g.Delete(ids));
        }

        [Fact]
        public void RetrievesByRoleId_Ok()
        {
            Group p = new Group();
            var groups = p.RetrievesByRoleId(new Role().Retrieves().Where(r => r.RoleName == "Administrators").First().Id);
            Assert.NotEmpty(groups);
        }

        [Fact]
        public void RetrievesByUserId_Ok()
        {
            Group p = new Group();
            var groups = p.RetrievesByUserId("1");
        }

        [Fact]
        public void SaveByUserId_Ok()
        {
            Group p = new Group();
            var groups = p.SaveByUserId("1", new string[] { "1", "2", "3" });
        }

        [Fact]
        public void SaveByRoleId_Ok()
        {
            Group p = new Group();
            var groups = p.SaveByRoleId("1", new string[] { "1", "2" });
        }

        [Fact]
        public void RetrievesByUserName_Ok()
        {
            Group p = new Group();
            Assert.NotNull(p.RetrievesByUserName("Admin"));
        }
    }
}

using Xunit;

namespace Bootstrap.DataAccess
{
    public class GroupsTest : IClassFixture<BootstrapAdminStartup>
    {
        [Fact]
        public void Retrieves_Ok()
        {
            Group g = new Group();
            Assert.NotEmpty(g.Retrieves());
        }

        [Fact]
        public void Save_Ok()
        {
            Group g = new Group() { GroupName = "UnitTest", Description = "UnitTestSave" };
            Assert.True(g.Save(g));
        }

        [Fact]
        public void Delete_Ok()
        {
            Group g = new Group();
            Assert.True(g.Delete(new string[] { "12", "13" }));
        }

        [Fact]
        public void RetrievesByRoleId_Ok()
        {
            Group p = new Group();
            var groups = p.RetrievesByRoleId("1");
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
            Assert.NotEmpty(p.RetrievesByUserName("Admin"));
        }
    }
}

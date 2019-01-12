using Xunit;

namespace Bootstrap.DataAccess
{
    public class MenusTest : IClassFixture<BootstrapAdminStartup>
    {
        [Fact]
        public void Save_Ok()
        {
            var m = new Menu()
            {
                Name = "UnitTest",
                Application = "0",
                Category = "0",
                Icon = "fa fa-fa",
                IsResource = 0,
                Target = "_blank",
                Order = 10,
                Url = "#",
                ParentId = "0"
            };
            Assert.True(m.Save(m));
        }

        [Fact]
        public void RetrieveMenusByRoleId_Ok()
        {
            var m = new Menu();
            Assert.NotEmpty(m.RetrieveMenusByRoleId("1"));
        }

        [Fact]
        public void Delete_Ok()
        {
            var m = new Menu()
            {
                Name = "UnitTest",
                Application = "0",
                Category = "0",
                Icon = "fa fa-fa",
                IsResource = 0,
                Target = "_blank",
                Order = 10,
                Url = "#",
                ParentId = "0"
            };
            m.Save(m);
            Assert.True(m.Delete(new string[] { m.Id }));
        }

        [Fact]
        public void RetrieveAllMenus_Ok()
        {
            var m = new Menu();
            Assert.NotEmpty(m.RetrieveAllMenus("Admin"));
        }

        [Fact]
        public void SaveMenusByRoleId_Ok()
        {
            var m = new Menu();
            Assert.True(m.SaveMenusByRoleId("1", new string[] { "450", "451" }));
        }
    }
}

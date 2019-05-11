using Bootstrap.Security;
using System.Linq;
using Xunit;

namespace Bootstrap.DataAccess
{
    [Collection("SQLServerContext")]
    public class MenusTest
    {
        [Fact]
        public void Save_Ok()
        {
            var m = new Menu();
            var poco = new BootstrapMenu()
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
            m.Delete(m.RetrieveAllMenus("Admin").Where(n => n.Name == m.Name).Select(n => n.Id));
            Assert.True(m.Save(poco));
            m.Delete(new string[] { poco.Id });
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
            var m = new Menu();
            var poco = new BootstrapMenu()
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
            m.Save(poco);
            Assert.True(m.Delete(new string[] { poco.Id }));
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

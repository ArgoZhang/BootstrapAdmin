using Bootstrap.Security;
using System.Linq;
using Xunit;

namespace Bootstrap.DataAccess.SqlServer
{
    [Collection("SQLServerContext")]
    public class MenusTest
    {
        [Fact]
        public void Save_Ok()
        {
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
            Assert.True(MenuHelper.Save(poco));
            MenuHelper.Delete(MenuHelper.RetrieveAllMenus("Admin").Where(n => n.Name == poco.Name).Select(n => n.Id));
        }

        [Fact]
        public void RetrieveMenusByRoleId_Ok()
        {
            var roleId = RoleHelper.Retrieves().FirstOrDefault(r => r.RoleName == "Administrators").Id;
            Assert.NotEmpty(MenuHelper.RetrieveMenusByRoleId(roleId));
        }

        [Fact]
        public void Delete_Ok()
        {
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
            MenuHelper.Save(poco);
            MenuHelper.Delete(MenuHelper.RetrieveAllMenus("Admin").Where(n => n.Name == poco.Name).Select(n => n.Id));
        }

        [Fact]
        public void RetrieveAllMenus_Ok()
        {
            Assert.NotEmpty(MenuHelper.RetrieveAllMenus("Admin"));
        }

        [Fact]
        public void SaveMenusByRoleId_Ok()
        {
            var roleId = RoleHelper.Retrieves().FirstOrDefault(r => r.RoleName == "Administrators").Id;
            Assert.True(MenuHelper.SaveMenusByRoleId(roleId, MenuHelper.RetrieveAllMenus("Admin").Select(m => m.Id)));
        }
    }
}

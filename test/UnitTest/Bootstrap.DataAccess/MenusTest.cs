using Bootstrap.Security;
using System.Linq;
using Xunit;

namespace Bootstrap.DataAccess
{
    [Collection("Login")]
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
                ParentId = "0",
                ParentName = "Test"
            };

            // insert
            Assert.True(MenuHelper.Save(poco));

            // update
            poco = MenuHelper.RetrieveAllMenus("Admin").Where(m => m.Id == poco.Id).FirstOrDefault();
            Assert.True(MenuHelper.Save(poco));

            // clean
            MenuHelper.Delete(new string[] { poco.Id });
        }

        [Fact]
        public void RetrieveMenusByRoleId_Ok()
        {
            var roleId = RoleHelper.Retrieves().FirstOrDefault(r => r.RoleName == "Administrators").Id;
            Assert.NotNull(MenuHelper.RetrieveMenusByRoleId(roleId));
        }

        [Fact]
        public void RetrieveAllMenus_Ok()
        {
            Assert.NotEmpty(MenuHelper.RetrieveAllMenus("Admin"));
            Assert.Empty(MenuHelper.RetrieveAllMenus("_UnitTest"));
        }

        [Fact]
        public void SaveMenusByRoleId_Ok()
        {
            var roleId = RoleHelper.Retrieves().FirstOrDefault(r => r.RoleName == "Administrators").Id;
            Assert.True(MenuHelper.SaveMenusByRoleId(roleId, MenuHelper.RetrieveAllMenus("Admin").Select(m => m.Id)));
        }

        [Fact]
        public void AuthorizateButtons_Ok()
        {
            Assert.True(MenuHelper.AuthorizateButtons("admin", "~/Admin/Profiles", "saveTheme"));
            Assert.False(MenuHelper.AuthorizateButtons("admin", "~/Admin/Profiles", "unitTest"));
        }
    }
}

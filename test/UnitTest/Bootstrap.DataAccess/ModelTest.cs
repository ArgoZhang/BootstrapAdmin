using Bootstrap.Security;
using Longbow.Web;
using System.Linq;
using Xunit;

namespace Bootstrap.DataAccess
{
    [Collection("SystemModel")]
    public class ModelTest
    {
        [Fact]
        public void DictDelete_Ok()
        {
            var ids = DictHelper.RetrieveDicts().Where(d => d.Define == 0);
            Assert.True(DictHelper.Delete(ids.Select(d => d.Id)));
            Assert.Equal(ids.Count(), DictHelper.RetrieveDicts().Count(d => d.Define == 0));
        }

        [Fact]
        public void MenuSave_Ok()
        {
            var menu = MenuHelper.RetrieveMenus("Admin").FirstOrDefault(m => m.Category == "0");
            var name = menu.Name;
            menu.Name = "UnitTest";
            Assert.True(MenuHelper.Save(menu));

            var menu2 = MenuHelper.RetrieveMenus("Admin").FirstOrDefault(m => m.Id == menu.Id);
            Assert.Equal(name, menu2.Name);
        }

        [Fact]
        public void MenuDelete_Ok()
        {
            var menu = MenuHelper.RetrieveMenus("Admin").FirstOrDefault(m => m.Category == "0");
            Assert.True(MenuHelper.Delete(new string[] { menu.Id }));
            var menu2 = MenuHelper.RetrieveMenus("Admin").FirstOrDefault(m => m.Id == menu.Id);
            Assert.NotNull(menu2);

            // 保护模式下，正常菜单可以删除
            var poco = new BootstrapMenu()
            {
                Name = "UnitTest",
                Application = "3",
                Category = "1",
                Icon = "fa fa-fa",
                IsResource = 0,
                Target = "_blank",
                Order = 10,
                Url = "#",
                ParentId = "0",
                ParentName = "Test",
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
        public void UserSave_Ok()
        {
            var user = UserHelper.Retrieves().FirstOrDefault(m => m.UserName == "User");
            user.DisplayName = "UnitTest";

            // 演示模式下不允许更改  Admin User 账户信息
            Assert.True(UserHelper.Save(user));
        }

        [Fact]
        public void UserUpdate_Ok()
        {
            var user = UserHelper.Retrieves().FirstOrDefault(m => m.UserName == "User");
            user.DisplayName = "UnitTest";
            Assert.True(UserHelper.Update(user.Id, "123789", "UnitTest"));
        }

        [Fact]
        public void UserChangePassword_Ok()
        {
            Assert.True(UserHelper.ChangePassword("User", "123789", "123789"));
        }

        [Fact]
        public void ConfigIPLocator_Ok()
        {
            var op = new IPLocatorOption()
            {
                IP = "182.148.123.196"
            };
            var dict = DictHelper.RetrieveDicts().FirstOrDefault(d => d.Category == "网站设置" && d.Name == "IP地理位置接口" && d.Define == 0);
            Assert.NotNull(dict);
            dict.Code = "JuheIPSvr";

            // 演示模式下不能保存
            Assert.True(DictHelper.Save(dict));
            Assert.Equal("None", DictHelper.RetrieveLocaleIPSvr());
        }

        [Fact]
        public void SaveByUserId_Ok()
        {
            var roleId = RoleHelper.Retrieves().FirstOrDefault(r => r.RoleName.Equals("Administrators", System.StringComparison.OrdinalIgnoreCase)).Id;
            Assert.True(UserHelper.SaveByRoleId(roleId, new string[0]));
            // 仍然属于 Administrators
            Assert.Contains(RoleHelper.RetrievesByUserName("Admin"), r => r.Equals("Administrators"));
        }

        [Fact]
        public void SaveByRoleID_Ok()
        {
            var uId = UserHelper.Retrieves().FirstOrDefault(u => u.UserName.Equals("Admin", System.StringComparison.OrdinalIgnoreCase))?.Id;
            Assert.True(RoleHelper.SaveByUserId(uId, new string[0]));
            Assert.Contains(RoleHelper.RetrievesByUserName("Admin"), r => r.Equals("Administrators"));
        }
    }
}

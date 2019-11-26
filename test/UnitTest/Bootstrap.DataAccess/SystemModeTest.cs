using Bootstrap.Security;
using Longbow.Cache;
using Longbow.Web;
using System.Linq;
using Xunit;

namespace Bootstrap.DataAccess
{
    [Collection("SQLServerContext")]
    [AutoRollback]
    public class SystemModeTest
    {
        private void SetSystemMode()
        {
            var dict = DictHelper.RetrieveDicts().FirstOrDefault(d => d.Category == "系统设置" && d.Name == "演示系统");
            dict.Code = "1";
            DictHelper.Save(dict);
        }

        [Fact]
        public void DictDelete_Ok()
        {
            SetSystemMode();
            var ids = DictHelper.RetrieveDicts().Where(d => d.Define == 0);
            Assert.True(DictHelper.Delete(ids.Select(d => d.Id)));
            Assert.Equal(ids.Count(), DictHelper.RetrieveDicts().Count(d => d.Define == 0));
        }

        [Fact]
        public void MenuSave_Ok()
        {
            SetSystemMode();
            var menu = MenuHelper.RetrieveMenus("Admin").FirstOrDefault(m => m.Category == "0");
            var name = menu.Name;
            menu.Name = "UnitTest";
            Assert.True(MenuHelper.Save(menu));

            CacheManager.Clear(MenuHelper.RetrieveMenusAll + "*");
            var menu2 = MenuHelper.RetrieveMenus("Admin").FirstOrDefault(m => m.Id == menu.Id);
            Assert.Equal(name, menu2.Name);
        }

        [Fact]
        public void MenuDelete_Ok()
        {
            SetSystemMode();
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
            SetSystemMode();
            var user = UserHelper.Retrieves().FirstOrDefault(m => m.UserName == "User");
            user.DisplayName = "UnitTest";
            Assert.True(UserHelper.Save(user));

            CacheManager.Clear(UserHelper.RetrieveUsersDataKey);
            var user2 = UserHelper.Retrieves().FirstOrDefault(m => m.Id == user.Id);
            Assert.NotEqual("UnitTest", user2.DisplayName);
        }

        [Fact]
        public void UserUpdate_Ok()
        {
            SetSystemMode();
            var user = UserHelper.Retrieves().FirstOrDefault(m => m.UserName == "User");
            user.DisplayName = "UnitTest";
            Assert.True(UserHelper.Update(user.Id, "123789", "UnitTest"));

            CacheManager.Clear(UserHelper.RetrieveUsersDataKey);
            var user2 = UserHelper.Retrieves().FirstOrDefault(m => m.Id == user.Id);
            Assert.NotEqual("UnitTest", user2.DisplayName);
        }

        [Fact]
        public void UserChangePassword_Ok()
        {
            SetSystemMode();
            Assert.True(UserHelper.ChangePassword("User", "123789", "123789"));
        }

        [Fact]
        public void ConfigIPLocator_Ok()
        {
            var op = new IPLocatorOption()
            {
                IP = "182.148.123.196"
            };
            var dict = DictHelper.RetrieveDicts().FirstOrDefault(d => d.Category == "系统设置" && d.Name == "IP地理位置接口" && d.Define == 0);
            Assert.NotNull(dict);
            dict.Code = "JuheIPSvr";
            DictHelper.Save(dict);
            DictHelper.ConfigIPLocator(op);
            Assert.NotNull(op.Url);
        }

        [Fact]
        public void SaveByUserId_Ok()
        {
            SetSystemMode();
            var roleId = RoleHelper.Retrieves().FirstOrDefault(r => r.RoleName.Equals("Administrators", System.StringComparison.OrdinalIgnoreCase)).Id;
            Assert.True(UserHelper.SaveByRoleId(roleId, new string[0]));
            // 仍然属于 Administrators
            Assert.Contains(RoleHelper.RetrievesByUserName("Admin"), r => r.Equals("Administrators"));
        }

        [Fact]
        public void SaveByRoleID_Ok()
        {
            SetSystemMode();
            var uId = UserHelper.Retrieves().FirstOrDefault(u => u.UserName.Equals("Admin", System.StringComparison.OrdinalIgnoreCase))?.Id;
            Assert.True(RoleHelper.SaveByUserId(uId, new string[0]));
            Assert.Contains(RoleHelper.RetrievesByUserName("Admin"), r => r.Equals("Administrators"));
        }

        [Fact]
        public void RetrieveHomeUrl_Ok()
        {
            Assert.Equal("~/Home/Index", DictHelper.RetrieveHomeUrl("BA"));
            var url = DictHelper.RetrieveHomeUrl("Demo");
            Assert.Equal("http://localhost:49185/", url);

            // INSERT INTO [Dicts] ([Category], [Name], [Code], [Define]) VALUES ('应用首页', 2, 'http://localhost:49185/', 0);
            var dict = DictHelper.RetrieveDicts().FirstOrDefault(d => d.Category == "应用首页" && d.Name == "Demo");
            url = dict.Code;
            dict.Code = "BA";
            Assert.True(DictHelper.Save(dict));
            Assert.Equal("BA", DictHelper.RetrieveHomeUrl("Demo"));

            dict.Code = url;
            Assert.True(DictHelper.Save(dict));
            Assert.Equal(url, DictHelper.RetrieveHomeUrl("Demo"));
        }
    }
}

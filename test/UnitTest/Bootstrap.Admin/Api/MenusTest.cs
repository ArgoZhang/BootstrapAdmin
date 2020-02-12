using Bootstrap.DataAccess;
using Bootstrap.Security;
using Longbow.Web.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using Xunit;

namespace Bootstrap.Admin.Api
{
    public class MenusTest : ControllerTest
    {
        public MenusTest(BALoginWebHost factory) : base(factory, "api/Menus") { }

        [Theory]
        [InlineData("Order", "asc")]
        [InlineData("Name", "asc")]
        [InlineData("ParentName", "asc")]
        [InlineData("CategoryName", "asc")]
        [InlineData("Target", "asc")]
        [InlineData("IsResource", "asc")]
        [InlineData("Application", "asc")]
        [InlineData("Order", "desc")]
        [InlineData("Name", "desc")]
        [InlineData("ParentName", "desc")]
        [InlineData("CategoryName", "desc")]
        [InlineData("Target", "desc")]
        [InlineData("IsResource", "desc")]
        [InlineData("Application", "desc")]
        public async void Get_Ok(string query, string order)
        {
            var qd = await Client.GetAsJsonAsync<QueryData<object>>($"?sort={query}&order={order}&offset=0&limit=100&parentName=%E6%B5%8B%E8%AF%95%E9%A1%B5%E9%9D%A2&name=%E5%85%B3%E4%BA%8E&category=1&isresource=0&appId=Demo&_=1558235377255");
            Assert.Single(qd.rows);
        }

        [Theory()]
        [InlineData("个人中心")]
        [InlineData("首页")]
        public async void Search_Ok(string search)
        {
            // 菜单 系统菜单 系统使用条件
            var qd = await Client.GetAsJsonAsync<QueryData<object>>($"?search={search}&sort=&order=&offset=0&limit=20&category=&name=&define=0&_=1547608210979");
            Assert.NotEmpty(qd.rows);
        }

        [Fact]
        public async void PostAndDelete_Ok()
        {
            var ret = await Client.PostAsJsonAsync<BootstrapMenu, bool>("", new BootstrapMenu() { Name = "UnitTest-Menu", Application = "0", Category = "0", ParentId = "0", Url = "#", Target = "_self", IsResource = 0 });
            Assert.True(ret);

            var ids = MenuHelper.RetrieveAllMenus("Admin").Where(d => d.Name == "UnitTest-Menu").Select(d => d.Id);
            Assert.True(await Client.DeleteAsJsonAsync<IEnumerable<string>, bool>("", ids));
        }


        [Fact]
        public async void PostById_Ok()
        {
            var uid = UserHelper.Retrieves().Where(u => u.UserName == "Admin").First().Id;
            var ret = await Client.PostAsJsonAsync<string, IEnumerable<object>>($"{uid}?type=user", string.Empty);
            Assert.NotEmpty(ret);

            var rid = RoleHelper.Retrieves().Where(r => r.RoleName == "Administrators").First().Id;
            ret = await Client.PostAsJsonAsync<string, IEnumerable<object>>($"{rid}?type=role", string.Empty);
            Assert.NotEmpty(ret);
        }

        [Fact]
        public async void PutById_Ok()
        {
            var ids = MenuHelper.RetrieveAllMenus("Admin").Select(g => g.Id);
            var rid = RoleHelper.Retrieves().Where(r => r.RoleName == "Administrators").First().Id;
            var ret = await Client.PutAsJsonAsync<IEnumerable<string>, bool>($"{rid}", ids);
            Assert.True(ret);
        }

        [Fact]
        public void RetrieveAppMenus_Ok()
        {
            Assert.Empty(MenuHelper.RetrieveAppMenus("", "", ""));
            Assert.NotEmpty(MenuHelper.RetrieveAppMenus("Demo", "Admin", ""));
        }
    }
}

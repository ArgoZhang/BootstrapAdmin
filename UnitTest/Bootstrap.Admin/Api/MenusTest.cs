using Bootstrap.DataAccess;
using Bootstrap.Security;
using Longbow.Web.Mvc;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace Bootstrap.Admin.Api.SqlServer
{
    public class MenusTest : ControllerTest
    {
        public MenusTest(BAWebHost factory) : base(factory, "api/Menus") { }

        [Fact]
        public async void Get_Ok()
        {
            // 菜单 系统菜单 系统使用条件
            var query = "?sort=Order&order=asc&offset=0&limit=20&parentName=&name=%E5%90%8E%E5%8F%B0%E7%AE%A1%E7%90%86&category=0&isresource=0&_=1547619684999";
            var qd = await Client.GetAsJsonAsync<QueryData<object>>(query);
            Assert.Single(qd.rows);
        }

        [Fact]
        public async void PostAndDelete_Ok()
        {
            var ret = await Client.PostAsJsonAsync<BootstrapMenu, bool>(new BootstrapMenu() { Name = "UnitTest-Menu", Application = "0", Category = "0", ParentId = "0", Url = "#", Target = "_self", IsResource = 0 });
            Assert.True(ret);

            var menu = new Menu();
            var ids = menu.RetrieveAllMenus("Admin").Where(d => d.Name == "UnitTest-Menu").Select(d => d.Id);
            Assert.True(await Client.DeleteAsJsonAsync<IEnumerable<string>, bool>("", ids));
        }


        [Fact]
        public async void PostById_Ok()
        {
            var uid = new User().Retrieves().Where(u => u.UserName == "Admin").First().Id;
            var ret = await Client.PostAsJsonAsync<string, IEnumerable<object>>($"{uid}?type=user", string.Empty);
            Assert.NotEmpty(ret);

            var rid = new Role().Retrieves().Where(r => r.RoleName == "Administrators").First().Id;
            ret = await Client.PostAsJsonAsync<string, IEnumerable<object>>($"{rid}?type=role", string.Empty);
            Assert.NotEmpty(ret);
        }

        [Fact]
        public async void PutById_Ok()
        {
            var ids = new Menu().RetrieveAllMenus("Admin").Select(g => g.Id);
            var rid = new Role().Retrieves().Where(r => r.RoleName == "Administrators").First().Id;
            var ret = await Client.PutAsJsonAsync<IEnumerable<string>, bool>($"{rid}", ids);
            Assert.True(ret);
        }
    }
}

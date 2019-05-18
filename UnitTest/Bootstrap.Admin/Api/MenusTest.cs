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
            var query = "?sort=Order&order=asc&offset=0&limit=100&parentName=%E6%B5%8B%E8%AF%95%E9%A1%B5%E9%9D%A2&name=%E5%85%B3%E4%BA%8E&category=1&isresource=0&appCode=2&_=1558235377255";
            var qd = await Client.GetAsJsonAsync<QueryData<object>>(query);
            Assert.Single(qd.rows);

            query = "?sort=Name&order=asc&offset=0&limit=100&parentName=%E6%B5%8B%E8%AF%95%E9%A1%B5%E9%9D%A2&name=%E5%85%B3%E4%BA%8E&category=1&isresource=0&appCode=2&_=1558235377255";
            qd = await Client.GetAsJsonAsync<QueryData<object>>(query);
            Assert.Single(qd.rows);

            query = "?sort=ParentName&order=asc&offset=0&limit=100&parentName=%E6%B5%8B%E8%AF%95%E9%A1%B5%E9%9D%A2&name=%E5%85%B3%E4%BA%8E&category=1&isresource=0&appCode=2&_=1558235377255";
            qd = await Client.GetAsJsonAsync<QueryData<object>>(query);
            Assert.Single(qd.rows);

            query = "?sort=CategoryName&order=asc&offset=0&limit=100&parentName=%E6%B5%8B%E8%AF%95%E9%A1%B5%E9%9D%A2&name=%E5%85%B3%E4%BA%8E&category=1&isresource=0&appCode=2&_=1558235377255";
            qd = await Client.GetAsJsonAsync<QueryData<object>>(query);
            Assert.Single(qd.rows);

            query = "?sort=Target&order=asc&offset=0&limit=100&parentName=%E6%B5%8B%E8%AF%95%E9%A1%B5%E9%9D%A2&name=%E5%85%B3%E4%BA%8E&category=1&isresource=0&appCode=2&_=1558235377255";
            qd = await Client.GetAsJsonAsync<QueryData<object>>(query);
            Assert.Single(qd.rows);

            query = "?sort=IsResource&order=asc&offset=0&limit=100&parentName=%E6%B5%8B%E8%AF%95%E9%A1%B5%E9%9D%A2&name=%E5%85%B3%E4%BA%8E&category=1&isresource=0&appCode=2&_=1558235377255";
            qd = await Client.GetAsJsonAsync<QueryData<object>>(query);
            Assert.Single(qd.rows);

            query = "?sort=Application&order=asc&offset=0&limit=100&parentName=%E6%B5%8B%E8%AF%95%E9%A1%B5%E9%9D%A2&name=%E5%85%B3%E4%BA%8E&category=1&isresource=0&appCode=2&_=1558235377255";
            qd = await Client.GetAsJsonAsync<QueryData<object>>(query);
            Assert.Single(qd.rows);
        }

        [Fact]
        public async void PostAndDelete_Ok()
        {
            var ret = await Client.PostAsJsonAsync<BootstrapMenu, bool>(new BootstrapMenu() { Name = "UnitTest-Menu", Application = "0", Category = "0", ParentId = "0", Url = "#", Target = "_self", IsResource = 0 });
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
    }
}

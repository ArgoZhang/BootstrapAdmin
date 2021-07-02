using Bootstrap.DataAccess;
using Longbow.Web.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using Xunit;

namespace Bootstrap.Admin.Api
{
    public class RolesTest : ControllerTest
    {
        public RolesTest(BALoginWebHost factory) : base(factory, "api/Roles") { }

        [Theory]
        [InlineData("RoleName", "asc")]
        [InlineData("RoleName", "desc")]
        public async void Get_Ok(string query, string order)
        {
            // 菜单 系统菜单 系统使用条件
            var qd = await Client.GetFromJsonAsync<QueryData<Role>>($"?sort={query}&order={order}&offset=0&limit=20&roleName=Administrators&description=%E7%B3%BB%E7%BB%9F%E7%AE%A1%E7%90%86%E5%91%98&_=1547625202230");
            Assert.Single(qd.rows);
        }

        [Theory()]
        [InlineData("Administrators")]
        [InlineData("系统管理员")]
        public async void Search_Ok(string search)
        {
            // 菜单 系统菜单 系统使用条件
            var qd = await Client.GetFromJsonAsync<QueryData<Role>>($"?search={search}&sort=&order=&offset=0&limit=20&category=&name=&define=0&_=1547608210979");
            Assert.NotEmpty(qd.rows);
        }

        [Fact]
        public async void PostAndDelete_Ok()
        {
            var resp = await Client.PostAsJsonAsync<Role>("", new Role() { RoleName = "UnitTest-Role", Description = "UnitTest-Desc" });
            var ret = await resp.Content.ReadFromJsonAsync<bool>();
            Assert.True(ret);

            var ids = RoleHelper.Retrieves().Where(d => d.RoleName == "UnitTest-Role").Select(d => d.Id);
            resp = await Client.DeleteAsJsonAsync<IEnumerable<string>>("", ids);
            var ret1 = await resp.Content.ReadFromJsonAsync<bool>();
            Assert.True(ret1);
        }

        [Fact]
        public async void PostById_Ok()
        {
            var uid = UserHelper.Retrieves().Where(u => u.UserName == "Admin").First().Id;
            var gid = GroupHelper.Retrieves().Where(g => g.GroupName == "Admin").First().Id;
            var mid = MenuHelper.RetrieveAllMenus("Admin").Where(m => m.Url == "~/Admin/Index").First().Id;

            var req = await Client.PostAsJsonAsync<string>($"{uid}?type=user", string.Empty);
            var ret = await req.Content.ReadFromJsonAsync<IEnumerable<object>>();
            Assert.NotEmpty(ret);

            req = await Client.PostAsJsonAsync<string>($"{gid}?type=group", string.Empty);
            ret = await req.Content.ReadFromJsonAsync<IEnumerable<object>>(); Assert.NotEmpty(ret);

            req = await Client.PostAsJsonAsync<string>($"{mid}?type=menu", string.Empty);
            ret = await req.Content.ReadFromJsonAsync<IEnumerable<object>>(); Assert.NotEmpty(ret);
        }

        [Fact]
        public async void PutById_Ok()
        {
            var uid = UserHelper.Retrieves().Where(u => u.UserName == "Admin").First().Id;
            var gid = GroupHelper.Retrieves().Where(g => g.GroupName == "Admin").First().Id;
            var mid = MenuHelper.RetrieveAllMenus("Admin").Where(m => m.Url == "~/Admin/Index").First().Id;
            var ids = RoleHelper.Retrieves().Select(r => r.Id);

            var req = await Client.PutAsJsonAsync<IEnumerable<string>>($"{uid}?type=user", ids);
            var ret = await req.Content.ReadFromJsonAsync<bool>();
            Assert.True(ret);

            req = await Client.PutAsJsonAsync<IEnumerable<string>>($"{gid}?type=group", ids);
            ret = await req.Content.ReadFromJsonAsync<bool>(); Assert.True(ret);

            req = await Client.PutAsJsonAsync<IEnumerable<string>>($"{mid}?type=menu", ids);
            ret = await req.Content.ReadFromJsonAsync<bool>(); Assert.True(ret);

            req = await Client.PutAsJsonAsync<IEnumerable<string>>($"{mid}?type=app", ids);
            ret = await req.Content.ReadFromJsonAsync<bool>(); Assert.True(ret);
        }
    }
}

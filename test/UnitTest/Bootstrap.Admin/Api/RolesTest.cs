using Bootstrap.DataAccess;
using Longbow.Web.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
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
            var qd = await Client.GetAsJsonAsync<QueryData<Role>>($"?sort={query}&order={order}&offset=0&limit=20&roleName=Administrators&description=%E7%B3%BB%E7%BB%9F%E7%AE%A1%E7%90%86%E5%91%98&_=1547625202230");
            Assert.Single(qd.rows);
        }

        [Theory()]
        [InlineData("Administrators")]
        [InlineData("系统管理员")]
        public async void Search_Ok(string search)
        {
            // 菜单 系统菜单 系统使用条件
            var qd = await Client.GetAsJsonAsync<QueryData<Role>>($"?search={search}&sort=&order=&offset=0&limit=20&category=&name=&define=0&_=1547608210979");
            Assert.NotEmpty(qd.rows);
        }

        [Fact]
        public async void PostAndDelete_Ok()
        {
            var ret = await Client.PostAsJsonAsync<Role, bool>("", new Role() { RoleName = "UnitTest-Role", Description = "UnitTest-Desc" });
            Assert.True(ret);

            var ids = RoleHelper.Retrieves().Where(d => d.RoleName == "UnitTest-Role").Select(d => d.Id);
            Assert.True(await Client.DeleteAsJsonAsync<IEnumerable<string>, bool>("", ids));
        }

        [Fact]
        public async void PostById_Ok()
        {
            var uid = UserHelper.Retrieves().Where(u => u.UserName == "Admin").First().Id;
            var gid = GroupHelper.Retrieves().Where(g => g.GroupName == "Admin").First().Id;
            var mid = MenuHelper.RetrieveAllMenus("Admin").Where(m => m.Url == "~/Admin/Index").First().Id;

            var ret = await Client.PostAsJsonAsync<string, IEnumerable<object>>($"{uid}?type=user", string.Empty);
            Assert.NotEmpty(ret);

            ret = await Client.PostAsJsonAsync<string, IEnumerable<object>>($"{gid}?type=group", string.Empty);
            Assert.NotEmpty(ret);

            ret = await Client.PostAsJsonAsync<string, IEnumerable<object>>($"{mid}?type=menu", string.Empty);
            Assert.NotEmpty(ret);
        }

        [Fact]
        public async void PutById_Ok()
        {
            var uid = UserHelper.Retrieves().Where(u => u.UserName == "Admin").First().Id;
            var gid = GroupHelper.Retrieves().Where(g => g.GroupName == "Admin").First().Id;
            var mid = MenuHelper.RetrieveAllMenus("Admin").Where(m => m.Url == "~/Admin/Index").First().Id;
            var ids = RoleHelper.Retrieves().Select(r => r.Id);

            var ret = await Client.PutAsJsonAsync<IEnumerable<string>, bool>($"{uid}?type=user", ids);
            Assert.True(ret);

            ret = await Client.PutAsJsonAsync<IEnumerable<string>, bool>($"{gid}?type=group", ids);
            Assert.True(ret);

            ret = await Client.PutAsJsonAsync<IEnumerable<string>, bool>($"{mid}?type=menu", ids);
            Assert.True(ret);

            ret = await Client.PutAsJsonAsync<IEnumerable<string>, bool>($"{mid}?type=app", ids);
            Assert.True(ret);
        }
    }
}

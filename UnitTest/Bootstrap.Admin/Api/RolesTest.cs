using Bootstrap.DataAccess;
using Longbow.Web.Mvc;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace Bootstrap.Admin.Api
{
    public class RolesTest : ControllerTest
    {
        public RolesTest(BAWebHost factory) : base(factory, "api/Roles") { }

        [Fact]
        public async void Get_Ok()
        {
            // 菜单 系统菜单 系统使用条件
            var query = "?sort=RoleName&order=asc&offset=0&limit=20&roleName=Administrators&description=%E7%B3%BB%E7%BB%9F%E7%AE%A1%E7%90%86%E5%91%98&_=1547625202230";
            var qd = await Client.GetAsJsonAsync<QueryData<Group>>(query);
            Assert.Single(qd.rows);
        }

        [Fact]
        public async void PostAndDelete_Ok()
        {
            var ret = await Client.PostAsJsonAsync<Role, bool>(new Role() { RoleName = "UnitTest-Role", Description = "UnitTest-Desc" });
            Assert.True(ret);

            var ids = new Role().Retrieves().Where(d => d.RoleName == "UnitTest-Role").Select(d => d.Id);
            Assert.True(await Client.DeleteAsJsonAsync<IEnumerable<string>, bool>(ids));
        }

        [Fact]
        public async void PostById_Ok()
        {
            var uid = new User().Retrieves().Where(u => u.UserName == "Admin").First().Id;
            var gid = new Group().Retrieves().Where(g => g.GroupName == "Admin").First().Id;
            var mid = new Menu().RetrieveAllMenus("Admin").Where(m => m.Url == "~/Admin/Index").First().Id;

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
            var uid = new User().Retrieves().Where(u => u.UserName == "Admin").First().Id;
            var gid = new Group().Retrieves().Where(g => g.GroupName == "Admin").First().Id;
            var mid = new Menu().RetrieveAllMenus("Admin").Where(m => m.Url == "~/Admin/Index").First().Id;
            var ids = new Role().Retrieves().Select(r => r.Id);

            var ret = await Client.PutAsJsonAsync<IEnumerable<string>, bool>($"{uid}?type=user", ids);
            Assert.True(ret);

            ret = await Client.PutAsJsonAsync<IEnumerable<string>, bool>($"{gid}?type=group", ids);
            Assert.True(ret);

            ret = await Client.PutAsJsonAsync<IEnumerable<string>, bool>($"{mid}?type=menu", ids);
            Assert.True(ret);
        }
    }
}

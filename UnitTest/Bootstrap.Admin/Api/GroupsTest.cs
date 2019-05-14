using Bootstrap.DataAccess;
using Longbow.Web.Mvc;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace Bootstrap.Admin.Api.SqlServer
{
    public class GroupsTest : ControllerTest
    {
        public GroupsTest(BAWebHost factory) : base(factory, "api/Groups") { }

        [Fact]
        public async void Get_Ok()
        {
            // 菜单 系统菜单 系统使用条件
            var query = "?sort=GroupName&order=asc&offset=0&limit=20&groupName=Admin&description=%E7%B3%BB%E7%BB%9F%E9%BB%98%E8%AE%A4%E7%BB%84&_=1547614230481";
            var qd = await Client.GetAsJsonAsync<QueryData<Group>>(query);
            Assert.Single(qd.rows);
        }

        [Fact]
        public async void GetById_Ok()
        {
            var id = new Group().Retrieves().Where(gp => gp.GroupName == "Admin").First().Id;
            var g = await Client.GetAsJsonAsync<Group>(id);
            Assert.Equal("Admin", g.GroupName);
        }

        [Fact]
        public async void PostAndDelete_Ok()
        {
            var ret = await Client.PostAsJsonAsync<Group, bool>("", new Group() { GroupName = "UnitTest-Group", Description = "UnitTest-Desc" });
            Assert.True(ret);

            var ids = new Group().Retrieves().Where(d => d.GroupName == "UnitTest-Group").Select(d => d.Id);
            Assert.True(await Client.DeleteAsJsonAsync<IEnumerable<string>, bool>(ids));
        }

        [Fact]
        public async void PostById_Ok()
        {
            var uid = new User().Retrieves().Where(u => u.UserName == "Admin").First().Id;
            var ret = await Client.PostAsJsonAsync<string, IEnumerable<Group>>($"{uid}?type=user", string.Empty);
            Assert.NotEmpty(ret);

            var rid = new Role().Retrieves().Where(r => r.RoleName == "Administrators").First().Id;
            ret = await Client.PostAsJsonAsync<string, IEnumerable<Group>>($"{rid}?type=role", string.Empty);
            Assert.NotEmpty(ret);
        }

        [Fact]
        public async void PutById_Ok()
        {
            var ids = new Group().Retrieves().Select(g => g.Id);
            var uid = new User().Retrieves().Where(u => u.UserName == "Admin").First().Id;
            var ret = await Client.PutAsJsonAsync<IEnumerable<string>, bool>($"{uid}?type=user", ids);
            Assert.True(ret);

            var rid = new Role().Retrieves().Where(r => r.RoleName == "Administrators").First().Id;
            ret = await Client.PutAsJsonAsync<IEnumerable<string>, bool>($"{rid}?type=role", ids);
            Assert.True(ret);
        }
    }
}

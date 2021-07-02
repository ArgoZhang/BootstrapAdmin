using Bootstrap.DataAccess;
using Longbow.Web.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using Xunit;

namespace Bootstrap.Admin.Api
{
    public class GroupsTest : ControllerTest
    {
        public GroupsTest(BALoginWebHost factory) : base(factory, "api/Groups") { }

        [Fact]
        public async void Get_Ok()
        {
            // 菜单 系统菜单 系统使用条件
            var query = "?sort=GroupName&order=asc&offset=0&limit=20&groupName=Admin&description=%E7%B3%BB%E7%BB%9F%E9%BB%98%E8%AE%A4%E7%BB%84&_=1547614230481";
            await Client.GetFromJsonAsync<QueryData<Group>>(query);
            query = "?sort=GroupName&order=desc&offset=0&limit=20&groupName=Admin&description=%E7%B3%BB%E7%BB%9F%E9%BB%98%E8%AE%A4%E7%BB%84&_=1547614230481";
            var qd = await Client.GetFromJsonAsync<QueryData<Group>>(query);
            Assert.Single(qd.rows);
        }

        [Theory()]
        [InlineData("Admin")]
        [InlineData("系统默认")]
        public async void Search_Ok(string search)
        {
            var qd = await Client.GetFromJsonAsync<QueryData<Group>>($"?search={search}&sort=&order=&offset=0&limit=20&category=&name=&define=0&_=1547608210979");
            Assert.NotEmpty(qd.rows);
        }

        [Fact]
        public async void PostAndDelete_Ok()
        {
            var ret = await Client.PostAsJsonAsync<Group>("", new Group() { GroupCode = "002", GroupName = "UnitTest-Group", Description = "UnitTest-Desc" });
            Assert.True(ret.IsSuccessStatusCode);

            var ids = GroupHelper.Retrieves().Where(d => d.GroupName == "UnitTest-Group").Select(d => d.Id);
            var resp = await Client.DeleteAsJsonAsync<IEnumerable<string>>("", ids);
            var ret1 = await resp.Content.ReadFromJsonAsync<bool>();
            Assert.True(ret1);
        }

        [Fact]
        public async void PostById_Ok()
        {
            var uid = UserHelper.Retrieves().Where(u => u.UserName == "Admin").First().Id;
            var ret = await Client.PostAsJsonAsync<string>($"{uid}?type=user", string.Empty);
            var groups = await ret.Content.ReadFromJsonAsync<IEnumerable<Group>>();
            Assert.NotEmpty(groups);

            var rid = RoleHelper.Retrieves().Where(r => r.RoleName == "Administrators").First().Id;
            ret = await Client.PostAsJsonAsync<string>($"{rid}?type=role", string.Empty);
            groups = await ret.Content.ReadFromJsonAsync<IEnumerable<Group>>();
            Assert.NotEmpty(groups);
        }

        [Fact]
        public async void PutById_Ok()
        {
            var ids = GroupHelper.Retrieves().Select(g => g.Id);
            var uid = UserHelper.Retrieves().Where(u => u.UserName == "Admin").First().Id;
            var ret = await Client.PutAsJsonAsync<IEnumerable<string>>($"{uid}?type=user", ids);
            Assert.True(ret.IsSuccessStatusCode);

            var rid = RoleHelper.Retrieves().Where(r => r.RoleName == "Administrators").First().Id;
            ret = await Client.PutAsJsonAsync<IEnumerable<string>>($"{rid}?type=role", ids);
            Assert.True(ret.IsSuccessStatusCode);
        }
    }
}

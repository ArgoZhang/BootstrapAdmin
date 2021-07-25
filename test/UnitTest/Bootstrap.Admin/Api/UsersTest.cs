using Bootstrap.DataAccess;
using Longbow.Web.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using Xunit;

namespace Bootstrap.Admin.Api
{
    public class UsersTest : ControllerTest
    {
        public UsersTest(BALoginWebHost factory) : base(factory, "api/Users") { }

        [Fact]
        public async void Option_Ok()
        {
            var req = new HttpRequestMessage(HttpMethod.Options, "");
            var resp = await Client.SendAsync(req);
            Assert.Equal(HttpStatusCode.NoContent, resp.StatusCode);
        }

        [Theory]
        [InlineData("DisplayName", "asc")]
        [InlineData("UserName", "asc")]
        [InlineData("RegisterTime", "asc")]
        [InlineData("ApprovedTime", "asc")]
        [InlineData("ApprovedBy", "asc")]
        [InlineData("DisplayName", "desc")]
        [InlineData("UserName", "desc")]
        [InlineData("RegisterTime", "desc")]
        [InlineData("ApprovedTime", "desc")]
        [InlineData("ApprovedBy", "desc")]
        public async void Get_Ok(string query, string order)
        {
            // 菜单 系统菜单 系统使用条件
            var qd = await Client.GetFromJsonAsync<QueryData<object>>($"?sort={query}&order={order}&offset=0&limit=20&name=Admin&displayName=Administrator&_=1547628247338");
            Assert.Single(qd.rows);
        }

        [Theory()]
        [InlineData("Administrator")]
        [InlineData("Admin")]
        [InlineData("系统默认创建")]
        public async void Search_Ok(string search)
        {
            // 菜单 系统菜单 系统使用条件
            var qd = await Client.GetFromJsonAsync<QueryData<object>>($"?search={search}&sort=&order=&offset=0&limit=20&category=&name=&define=0&_=1547608210979");
            Assert.NotEmpty(qd.rows);
        }

        [Fact]
        public async void PostAndDelete_Ok()
        {
            UserHelper.Delete(UserHelper.Retrieves().Where(usr => usr.UserName == "UnitTest_Delete").Select(usr => usr.Id));

            var nusr = new User { UserName = "UnitTest_Delete", Password = "1", DisplayName = "DisplayName", ApprovedBy = "System", ApprovedTime = DateTime.Now, Description = "Desc", Icon = "default.jpg" };
            var resp = await Client.PostAsJsonAsync<User>("", nusr);
            var ret = await resp.Content.ReadFromJsonAsync<bool>();
            Assert.True(ret);

            nusr.Id = UserHelper.Retrieves().First(u => u.UserName == nusr.UserName).Id;
            resp = await Client.PostAsJsonAsync<User>("", nusr);
            ret = await resp.Content.ReadFromJsonAsync<bool>();
            Assert.True(ret);

            var ids = UserHelper.Retrieves().Where(d => d.UserName == nusr.UserName).Select(d => d.Id);
            var resp1 = await Client.DeleteAsJsonAsync<IEnumerable<string>>("", ids);
            var ret1 = await resp1.Content.ReadFromJsonAsync<bool>();
            Assert.True(ret1);
        }

        [Fact]
        public async void PostById_Ok()
        {
            var rid = RoleHelper.Retrieves().Where(r => r.RoleName == "Administrators").First().Id;

            var resp = await Client.PostAsJsonAsync<string>($"{rid}?type=role", string.Empty);
            var ret = await resp.Content.ReadFromJsonAsync<IEnumerable<object>>();
            Assert.NotNull(ret);

            var gid = GroupHelper.Retrieves().Where(r => r.GroupName == "Admin").First().Id;
            resp = await Client.PostAsJsonAsync<string>($"{gid}?type=group", string.Empty);
            ret = await resp.Content.ReadFromJsonAsync<IEnumerable<object>>();
            Assert.NotNull(ret);

            // 创建用户
            var nusr = new User { UserName = "UnitTest_Reset", Password = "1", DisplayName = "DisplayName", ApprovedBy = "System", ApprovedTime = DateTime.Now, Description = "Desc", Icon = "default.jpg" };
            UserHelper.Save(nusr);

            // 申请重置
            UserHelper.ForgotPassword(new ResetUser() { DisplayName = nusr.DisplayName, Reason = "UnitTest", ResetTime = DateTime.Now, UserName = nusr.UserName });

            // 重置操作
            resp = await Client.PostAsJsonAsync<string>($"{nusr.UserName}?type=reset", string.Empty);
            ret = await resp.Content.ReadFromJsonAsync<IEnumerable<object>>();
            Assert.NotNull(ret);

            UserHelper.Delete(UserHelper.Retrieves().Where(usr => usr.UserName == nusr.UserName).Select(usr => usr.Id));
        }

        [Fact]
        public async void PutById_Ok()
        {
            var ids = UserHelper.Retrieves().Where(u => u.UserName == "Admin").Select(u => u.Id);
            var gid = GroupHelper.Retrieves().Where(r => r.GroupName == "Admin").First().Id;
            var resp = await Client.PutAsJsonAsync<IEnumerable<string>>($"{gid}?type=group", ids);
            var ret = await resp.Content.ReadFromJsonAsync<bool>();
            Assert.True(ret);

            var rid = RoleHelper.Retrieves().Where(r => r.RoleName == "Administrators").First().Id;
            resp = await Client.PutAsJsonAsync<IEnumerable<string>>($"{rid}?type=role", ids);
            ret = await resp.Content.ReadFromJsonAsync<bool>();
            Assert.True(ret);
        }
    }
}

using Bootstrap.DataAccess;
using Longbow.Web.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using Xunit;

namespace Bootstrap.Admin.Api
{
    public class UsersTest : ApiWebHost
    {
        private BAWebHost _factory;

        public UsersTest(BAWebHost factory) : base(factory, "Users", true)
        {
            _factory = factory;
        }

        [Fact]
        public async void Option_Ok()
        {
            var req = new HttpRequestMessage(HttpMethod.Options, "");
            var resp = await Client.SendAsync(req);
        }

        [Fact]
        public async void Get_Ok()
        {
            // 菜单 系统菜单 系统使用条件
            var query = "?sort=DisplayName&order=asc&offset=0&limit=20&name=Admin&displayName=Administrator&_=1547628247338";
            var qd = await Client.GetAsJsonAsync<QueryData<object>>(query);
            Assert.Single(qd.rows);
        }

        [Fact]
        public async void PostAndDelete_Ok()
        {
            var user = new User();
            user.Delete(user.Retrieves().Where(usr => usr.UserName == "UnitTest-Delete").Select(usr => usr.Id));

            var nusr = new User { UserName = "UnitTest-Delete", Password = "1", DisplayName = "DisplayName", ApprovedBy = "System", ApprovedTime = DateTime.Now, Description = "Desc", Icon = "default.jpg" };
            var resp = await Client.PostAsJsonAsync<User, bool>("", nusr);
            Assert.True(resp);

            nusr.Id = user.Retrieves().First(u => u.UserName == nusr.UserName).Id;
            resp = await Client.PostAsJsonAsync<User, bool>("", nusr);
            Assert.True(resp);

            var ids = user.Retrieves().Where(d => d.UserName == nusr.UserName).Select(d => d.Id);
            Assert.True(await Client.DeleteAsJsonAsync<IEnumerable<string>, bool>("", ids));
        }

        [Fact]
        public async void PostById_Ok()
        {
            var rid = new Role().Retrieves().Where(r => r.RoleName == "Administrators").First().Id;

            var ret = await Client.PostAsJsonAsync<string, IEnumerable<object>>($"{rid}?type=role", string.Empty);
            Assert.NotNull(ret);

            var gid = new Group().Retrieves().Where(r => r.GroupName == "Admin").First().Id;
            ret = await Client.PostAsJsonAsync<string, IEnumerable<object>>($"{gid}?type=group", string.Empty);
            Assert.NotNull(ret);
        }

        [Fact]
        public async void PutById_Ok()
        {
            var ids = new User().Retrieves().Where(u => u.UserName == "Admin").Select(u => u.Id);
            var gid = new Group().Retrieves().Where(r => r.GroupName == "Admin").First().Id;
            var ret = await Client.PutAsJsonAsync<IEnumerable<string>, bool>($"{gid}?type=group", ids);
            Assert.True(ret);

            var rid = new Role().Retrieves().Where(r => r.RoleName == "Administrators").First().Id;
            ret = await Client.PutAsJsonAsync<IEnumerable<string>, bool>($"{rid}?type=role", ids);
            Assert.True(ret);
        }

        [Fact]
        public async void Put_Ok()
        {
            var usr = new User { UserName = "UnitTest-Change", Password = "1", DisplayName = "DisplayName", ApprovedBy = "System", ApprovedTime = DateTime.Now, Description = "Desc", Icon = "default.jpg" };
            usr.Delete(usr.Retrieves().Where(u => u.UserName == usr.UserName).Select(u => u.Id));
            Assert.True(usr.Save(usr));

            // change theme
            usr.UserStatus = UserStates.ChangeTheme;
            var resp = await Client.PutAsJsonAsync<User, bool>("", usr);
            Assert.False(resp);

            // Login as new user
            await _factory.LoginAsync(Client, "UnitTest-Change", "1");
            resp = await Client.PutAsJsonAsync<User, bool>("", usr);
            Assert.True(resp);

            // change password
            usr.UserStatus = UserStates.ChangePassword;
            usr.NewPassword = "1";
            usr.Password = "1";
            resp = await Client.PutAsJsonAsync<User, bool>("", usr);
            Assert.True(resp);

            // change displayname
            usr.UserStatus = UserStates.ChangeDisplayName;
            resp = await Client.PutAsJsonAsync<User, bool>("", usr);
            Assert.True(resp);

            // delete 
            usr.Delete(new string[] { usr.Id });
        }
    }
}

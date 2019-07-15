using Longbow.Web;
using System;
using System.Collections.Generic;
using System.Reflection;
using Xunit;

namespace Bootstrap.Admin.Api.SqlServer
{
    public class OnlineTest : ControllerTest
    {
        public OnlineTest(BAWebHost factory) : base(factory, "api/OnlineUsers") { }

        [Fact]
        public async void Get_Ok()
        {
            var users = await Client.GetAsJsonAsync<IEnumerable<OnlineUser>>();
            Assert.Single(users);
        }

        [Fact]
        public async void GetById_Ok()
        {
            var urls = await Client.GetAsJsonAsync<IEnumerable<KeyValuePair<DateTime, string>>>("UnitTest");
            Assert.Empty(urls);
        }

        [Fact]
        public async void Put_Ok()
        {
            var ret = await Client.PutAsJsonAsync<string, bool>("");
            Assert.False(ret);

            // 三次 Put 请求后返回真
            ret = await Client.PutAsJsonAsync<string, bool>("");
            ret = await Client.PutAsJsonAsync<string, bool>("");
            ret = await Client.PutAsJsonAsync<string, bool>("");
            Assert.True(ret);
        }
    }
}

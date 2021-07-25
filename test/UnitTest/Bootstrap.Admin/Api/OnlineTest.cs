using Longbow.Web;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Net.Http.Json;
using Xunit;

namespace Bootstrap.Admin.Api
{
    public class OnlineTest : ControllerTest
    {
        public OnlineTest(BALoginWebHost factory) : base(factory, "api/OnlineUsers") { }

        [Fact]
        public async void Get_Ok()
        {
            var users = await Client.GetFromJsonAsync<IEnumerable<OnlineUser>>("", new System.Text.Json.JsonSerializerOptions().AddDefaultConverters());
            Assert.Single(users);
        }

        [Fact]
        public async void GetById_Ok()
        {
            var urls = await Client.GetFromJsonAsync<IEnumerable<KeyValuePair<DateTime, string>>>("UnitTest");
            Assert.Empty(urls);
        }

        [Fact]
        public async void Put_Ok()
        {
            // 三次 Put 请求后返回真
            await Client.PutAsJsonAsync<string>("", "");
            await Client.PutAsJsonAsync<string>("", "");
            await Client.PutAsJsonAsync<string>("", "");
            var ret = await Client.PutAsJsonAsync<string>("", "");
            var req = await ret.Content.ReadFromJsonAsync<bool>();
            Assert.True(req);
        }
    }
}

using System;
using System.Collections.Generic;
using Xunit;

namespace Bootstrap.Admin.Api
{
    public class OnlineTest : ControllerTest
    {
        public OnlineTest(BAWebHost factory) : base(factory, "api/OnlineUsers") { }

        [Fact]
        public async void Post_Ok()
        {
            var usres = await Client.PostAsJsonAsync<string, IEnumerable<OnlineUser>>(string.Empty);
            Assert.Single(usres);
        }

        [Fact]
        public async void Get_Ok()
        {
            var urls = await Client.GetAsJsonAsync<IEnumerable<KeyValuePair<DateTime, string>>>("::1");
            Assert.NotEmpty(urls);
        }
    }
}

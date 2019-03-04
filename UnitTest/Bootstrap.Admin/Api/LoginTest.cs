using Bootstrap.DataAccess;
using System.Collections.Generic;
using System.Net.Http;
using Xunit;

namespace Bootstrap.Admin.Api
{
    public class LoginTest : ControllerTest
    {
        public LoginTest(BAWebHost factory) : base(factory, "api/Login") { }

        [Fact]
        public async void Login_Get()
        {
            var users = await Client.GetAsJsonAsync<IEnumerable<LoginUser>>();
            Assert.NotEmpty(users);
        }

        [Fact]
        public async void Login_Ok()
        {
            var resq = await Client.PostAsJsonAsync("", new { userName = "Admin", password = "123789" });
            var _token = await resq.Content.ReadAsStringAsync();
            Assert.NotNull(_token);
        }

        [Fact]
        public async void Login_Fail()
        {
            var resq = await Client.PostAsJsonAsync("", new { userName = "Admin-NotExists", password = "123789" });
            var _token = await resq.Content.ReadAsStringAsync();
            Assert.Equal("", _token);
        }

        [Fact]
        public async void Option_Ok()
        {
            var req = new HttpRequestMessage(HttpMethod.Options, "");
            var resp = await Client.SendAsync(req);
        }
    }
}

using Bootstrap.DataAccess;
using Longbow.Web.Mvc;
using System.Net.Http;
using Xunit;

namespace Bootstrap.Admin.Api.SqlServer
{
    public class LoginTest : ControllerTest
    {
        public LoginTest(BAWebHost factory) : base(factory, "api/Login") { }

        [Fact]
        public async void Login_Get()
        {
            var users = await Client.GetAsJsonAsync<QueryData<LoginUser>>("?sort=LoginTime&order=&offset=0&limit=20&startTime=&endTime=&loginIp=&_=1560933256621");
            Assert.NotEmpty(users.rows);
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

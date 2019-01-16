using System.Net.Http;
using Xunit;

namespace Bootstrap.Admin.Controllers
{
    public class AccountTest : ControllerWebHost
    {
        private BAWebHost _factory;

        public AccountTest(BAWebHost factory) : base(factory, "Account", false)
        {
            _factory = factory;
        }

        [Fact]
        public async void Login_Fail()
        {
            // login
            var r = await Client.GetAsync("Login");
            Assert.True(r.IsSuccessStatusCode);
            var content = await r.Content.ReadAsStringAsync();
            Assert.Contains("登 陆", content);
        }

        [Fact]
        public async void Login_Admin()
        {
            // login
            var resp = await _factory.LoginAsync(Client);
            Assert.Contains("注销", resp);
        }

        [Fact]
        public async void Logout_Ok()
        {
            // logout
            var r = await Client.GetAsync("Logout");
            Assert.True(r.IsSuccessStatusCode);
            var content = await r.Content.ReadAsStringAsync();
            Assert.Contains("登 陆", content);
        }

        [Fact]
        public async void AccessDenied_Ok()
        {
            // logout
            var r = await Client.GetAsync("AccessDenied");
            Assert.True(r.IsSuccessStatusCode);
            var content = await r.Content.ReadAsStringAsync();
            Assert.Contains("您无权访问本页面请联系网站管理员授权后再查看", content);
        }
    }
}

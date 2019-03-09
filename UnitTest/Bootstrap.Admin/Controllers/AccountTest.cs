using Xunit;

namespace Bootstrap.Admin.Controllers
{
    public class AccountTest : ControllerTest
    {
        public AccountTest(BAWebHost factory) : base(factory, "Account") { }

        [Fact]
        public async void Login_Fail()
        {
            var client = Host.CreateClient();
            var r = await client.GetAsync("/Account/Login");
            Assert.True(r.IsSuccessStatusCode);
            var content = await r.Content.ReadAsStringAsync();
            Assert.Contains("登 陆", content);
        }

        [Fact]
        public async void Login_Admin()
        {
            var resp = await Client.GetAsync("Login");
            var context = await resp.Content.ReadAsStringAsync();
            Assert.Contains("注销", context);
        }

        [Fact]
        public async void Logout_Ok()
        {
            var client = Host.CreateClient();
            var r = await client.GetAsync("/Account/Logout");
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
            Assert.Contains("服务器拒绝处理您的请求", content);
        }
    }
}

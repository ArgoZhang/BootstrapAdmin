using System.Net.Http;
using Xunit;

namespace Bootstrap.Admin
{
    public class AccountTest : IClassFixture<BAWebHost>
    {
        private HttpClient _client;

        public AccountTest(BAWebHost factory)
        {
            _client = factory.CreateClient();
        }

        [Fact]
        public async void Login_Fail()
        {
            // login
            var r = await _client.GetAsync("/Account/Login");
            Assert.True(r.IsSuccessStatusCode);
            var content = await r.Content.ReadAsStringAsync();
            Assert.Contains("登 陆", content);
        }

        [Fact]
        public async void Login_Admin()
        {
            // login
            var r = await _client.GetAsync("/Account/Login");
            Assert.True(r.IsSuccessStatusCode);
            var view = await r.Content.ReadAsStringAsync();
            var tokenTag = "<input name=\"__RequestVerificationToken\" type=\"hidden\" value=\"";
            var index = view.IndexOf(tokenTag);
            view = view.Substring(index + tokenTag.Length);
            index = view.IndexOf("\" /></form>");
            var antiToken = view.Substring(0, index);

            var content = new MultipartFormDataContent();
            content.Add(new StringContent("Admin"), "userName");
            content.Add(new StringContent("123789"), "password");
            content.Add(new StringContent("true"), "remember");
            content.Add(new StringContent(antiToken), "__RequestVerificationToken");
            r = await _client.PostAsync("/Account/Login", content);
            var resp = await r.Content.ReadAsStringAsync();
            Assert.Contains("注销", resp);
        }

        [Fact]
        public async void Logout_Ok()
        {
            // logout
            var r = await _client.GetAsync("/Account/Logout");
            Assert.True(r.IsSuccessStatusCode);
            var content = await r.Content.ReadAsStringAsync();
            Assert.Contains("登 陆", content);
        }

        [Fact]
        public async void AccessDenied_Ok()
        {
            // logout
            var r = await _client.GetAsync("/Account/AccessDenied");
            Assert.True(r.IsSuccessStatusCode);
            var content = await r.Content.ReadAsStringAsync();
            Assert.Contains("您无权访问本页面请联系网站管理员授权后再查看", content);
        }
    }
}

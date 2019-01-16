using System.Net.Http;
using Xunit;

namespace Bootstrap.Admin.Api
{
    public class LoginTest : ApiTest
    {
        public LoginTest(BAWebHost factory) : base(factory, "Login", false)
        {

        }

        [Fact]
        public async void Login_Ok()
        {
            var resq = await Client.PostAsJsonAsync("", new { userName = "Admin", password = "123789" });
            var _token = await resq.Content.ReadAsStringAsync();
            Assert.NotNull(_token);
        }

        [Fact]
        public async void Option_Ok()
        {
            var req = new HttpRequestMessage(HttpMethod.Options, "");
            var resp = await Client.SendAsync(req);
        }
    }
}

using System.Net.Http;
using Xunit;

namespace Bootstrap.Admin
{
    public class LoginTest : IClassFixture<BAWebHost>
    {
        private HttpClient _client;

        public LoginTest(BAWebHost factory)
        {
            _client = factory.CreateClient();
        }

        [Fact]
        public async void Login_Ok()
        {
            var resq = await _client.PostAsJsonAsync("/api/Login", new { userName = "Admin", password = "123789" });
            var _token = await resq.Content.ReadAsStringAsync();
            Assert.NotNull(_token);
        }
    }
}

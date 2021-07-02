using Bootstrap.DataAccess;
using Longbow.Web.Mvc;
using Longbow.Web.SMS;
using Microsoft.Extensions.DependencyInjection;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json;
using Xunit;

namespace Bootstrap.Admin.Api
{
    public class LoginTest : ControllerTest
    {
        public LoginTest(BALoginWebHost factory) : base(factory, "api/Login") { }

        [Fact]
        public async void Login_Get()
        {
            var users = await Client.GetFromJsonAsync<QueryData<LoginUser>>("?sort=LoginTime&order=&offset=0&limit=20&startTime=&endTime=&loginIp=&_=1560933256621", new JsonSerializerOptions().AddDefaultConverters());
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
        public async void Put_Ok()
        {
            var resq = await Client.PutAsync("?phone=", new StringContent(""));
            var payload = await resq.Content.ReadAsStringAsync();
            var resp = System.Text.Json.JsonSerializer.Deserialize<SMSResult>(payload, new System.Text.Json.JsonSerializerOptions() { PropertyNameCaseInsensitive = true });
            Assert.False(resp.Result);

            resq = await Client.PutAsync("?phone=18910281024", new StringContent(""));
            payload = await resq.Content.ReadAsStringAsync();
            resp = System.Text.Json.JsonSerializer.Deserialize<SMSResult>(payload, new System.Text.Json.JsonSerializerOptions() { PropertyNameCaseInsensitive = true });
            Assert.True(resp.Result);
        }

        [Fact]
        public async void Option_Ok()
        {
            var req = new HttpRequestMessage(HttpMethod.Options, "");
            await Client.SendAsync(req);
        }
    }
}

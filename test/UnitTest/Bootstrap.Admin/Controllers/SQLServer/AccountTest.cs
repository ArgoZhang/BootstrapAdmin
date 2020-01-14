using Bootstrap.DataAccess;
using System.Linq;
using System.Net;
using System.Net.Http;
using Xunit;

namespace Bootstrap.Admin.Controllers.SqlServer
{
    public class AccountTest : ControllerTest
    {
        public AccountTest(BAWebHost factory) : base(factory, "Account") { }

        [Fact]
        public async void SystemMode_Test()
        {
            var dict = DictHelper.RetrieveDicts().FirstOrDefault(d => d.Category == "网站设置" && d.Name == "演示系统");
            dict.Code = "1";
            DictHelper.Save(dict);

            var r = await Client.GetAsync("Login");

            // 恢复保护模式
            var db = DbManager.Create();
            db.Execute("Update Dicts Set Code = @0 Where Id = @1", "0", dict.Id);
            Assert.Equal(HttpStatusCode.OK, r.StatusCode);
            var source = await r.Content.ReadAsStringAsync();
            Assert.Contains("演示系统", source);
        }

        [Fact]
        public async void Login_Empty()
        {
            var client = Host.CreateClient();
            var r = await client.GetAsync("/Account/Login");
            Assert.True(r.IsSuccessStatusCode);
            var content = await r.Content.ReadAsStringAsync();
            Assert.Contains("登 录", content);

            r = await client.GetAsync("/Account/Login");
            var view = await r.Content.ReadAsStringAsync();
            var tokenTag = "<input name=\"__RequestVerificationToken\" type=\"hidden\" value=\"";
            var index = view.IndexOf(tokenTag);
            view = view.Substring(index + tokenTag.Length);
            index = view.IndexOf("\" /></form>");
            var antiToken = view.Substring(0, index);

            var loginContent = new MultipartFormDataContent
            {
                { new StringContent(""), "userName" },
                { new StringContent(""), "password" },
                { new StringContent(""), "remember" },
                { new StringContent(antiToken), "__RequestVerificationToken" }
            };
            var req = await client.PostAsync("/Account/Login", loginContent);
            Assert.Equal(HttpStatusCode.OK, req.StatusCode);
            content = await req.Content.ReadAsStringAsync();
            Assert.Contains("登 录", content);
        }

        [Fact]
        public async void Login_Fail()
        {
            var client = Host.CreateClient();
            var r = await client.GetAsync("/Account/Login");
            Assert.True(r.IsSuccessStatusCode);
            var content = await r.Content.ReadAsStringAsync();
            Assert.Contains("登 录", content);

            r = await client.GetAsync("/Account/Login");
            var view = await r.Content.ReadAsStringAsync();
            var tokenTag = "<input name=\"__RequestVerificationToken\" type=\"hidden\" value=\"";
            var index = view.IndexOf(tokenTag);
            view = view.Substring(index + tokenTag.Length);
            index = view.IndexOf("\" /></form>");
            var antiToken = view.Substring(0, index);

            var loginContent = new MultipartFormDataContent
            {
                { new StringContent("Admin"), "userName" },
                { new StringContent("1"), "password" },
                { new StringContent("true"), "remember" },
                { new StringContent(antiToken), "__RequestVerificationToken" }
            };
            r = await client.PostAsync("/Account/Login", loginContent);
            Assert.Equal(HttpStatusCode.OK, r.StatusCode);
            content = await r.Content.ReadAsStringAsync();
            Assert.Contains("登 录", content);

            // 空密码登陆
            view = await r.Content.ReadAsStringAsync();
            tokenTag = "<input name=\"__RequestVerificationToken\" type=\"hidden\" value=\"";
            index = view.IndexOf(tokenTag);
            view = view.Substring(index + tokenTag.Length);
            index = view.IndexOf("\" /></form>");
            antiToken = view.Substring(0, index);
            loginContent = new MultipartFormDataContent
            {
                { new StringContent(""), "userName" },
                { new StringContent(""), "password" },
                { new StringContent(""), "remember" },
                { new StringContent(antiToken), "__RequestVerificationToken" }
            };
            r = await client.PostAsync("/Account/Login", loginContent);
            Assert.Equal(HttpStatusCode.OK, r.StatusCode);
            content = await r.Content.ReadAsStringAsync();
            Assert.Contains("登 录", content);
        }

        [Fact]
        public async void Logout_Ok()
        {
            var client = Host.CreateClient();
            var r = await client.GetAsync("/Account/Logout");
            Assert.True(r.IsSuccessStatusCode);
            var content = await r.Content.ReadAsStringAsync();
            Assert.Contains("登 录", content);
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

        [Fact]
        public async void Lock_Ok()
        {
            var r = await Client.GetAsync("Lock");
            Assert.True(r.IsSuccessStatusCode);
            var content = await r.Content.ReadAsStringAsync();
            Assert.Contains("系统锁屏", content);

            // 第二次调用 跳转到登录页面
            r = await Client.GetAsync("Lock");
            Assert.True(r.IsSuccessStatusCode);
            content = await r.Content.ReadAsStringAsync();
            Assert.Contains("登 录", content);

            // 调用 Post Mobile
            var data = new MultipartFormDataContent
            {
                { new StringContent("13800010001"), "userName" },
                { new StringContent("1234"), "password" },
                { new StringContent("Mobile"), "authType" }
            };
            await Client.PostAsync("Lock", data);
            UserHelper.Delete(UserHelper.Retrieves().Where(u => u.UserName == "13800010001").Select(u => u.Id));

            // 调用Post
            data = new MultipartFormDataContent
            {
                { new StringContent("Admin"), "userName" },
                { new StringContent("123789"), "password" },
                { new StringContent("Cookie"), "authType" }
            };
            await Client.PostAsync("Lock", data);
        }

        [Theory]
        [InlineData("Gitee")]
        [InlineData("GitHub")]
        [InlineData("WeChat")]
        public async void OAuth_Ok(string url)
        {
            var client = Host.CreateClient();
            var r = await client.GetAsync($"/Account/{url}");
            Assert.True(r.IsSuccessStatusCode);
            var content = await r.Content.ReadAsStringAsync();
            Assert.Contains("登 录", content);
        }

        [Fact]
        public async void Mobile_Ok()
        {
            using (var db = DbManager.Create()) db.Execute("delete from Users where UserName = @0", "18910001000");
            var client = Host.CreateClient();
            var r = await client.GetAsync("/Account/Login");
            var view = await r.Content.ReadAsStringAsync();
            var tokenTag = "<input name=\"__RequestVerificationToken\" type=\"hidden\" value=\"";
            var index = view.IndexOf(tokenTag);
            view = view.Substring(index + tokenTag.Length);
            index = view.IndexOf("\" /></form>");
            var antiToken = view.Substring(0, index);

            var content = new MultipartFormDataContent
            {
                { new StringContent("18910001000"), "phone" },
                { new StringContent("1234"), "code" },
                { new StringContent(antiToken), "__RequestVerificationToken" }
            };
            var m = await client.PostAsync("/Account/Mobile", content);
            Assert.True(m.IsSuccessStatusCode);
            var payload = await m.Content.ReadAsStringAsync();
            Assert.DoesNotContain("登 录", payload);

            // login as cookie
            // 调用Post
            var data = new MultipartFormDataContent
            {
                { new StringContent("Admin"), "userName" },
                { new StringContent("123789"), "password" },
                { new StringContent("Cookie"), "authType" }
            };
            m = await Client.PostAsync("Lock", data);
            payload = await m.Content.ReadAsStringAsync();
            Assert.DoesNotContain("登 录", payload);
        }

        [Fact]
        public async void Mobile_Fail()
        {
            using (var db = DbManager.Create()) db.Execute("delete from Users where UserName = @0", "18910001000");

            var client = Host.CreateClient();
            var r = await client.GetAsync("/Account/Login");
            var view = await r.Content.ReadAsStringAsync();
            var tokenTag = "<input name=\"__RequestVerificationToken\" type=\"hidden\" value=\"";
            var index = view.IndexOf(tokenTag);
            view = view.Substring(index + tokenTag.Length);
            index = view.IndexOf("\" /></form>");
            var antiToken = view.Substring(0, index);

            var content = new MultipartFormDataContent
            {
                { new StringContent("18910001000"), "phone" },
                { new StringContent("1000"), "code" },
                { new StringContent(antiToken), "__RequestVerificationToken" }
            };
            var m = await client.PostAsync("/Account/Mobile?AppId=0", content);
            Assert.True(m.IsSuccessStatusCode);
            var payload = await m.Content.ReadAsStringAsync();
            Assert.Contains("登 录", payload);
        }
    }
}

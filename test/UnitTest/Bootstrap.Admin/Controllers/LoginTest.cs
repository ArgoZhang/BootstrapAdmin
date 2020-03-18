using Bootstrap.DataAccess;
using System.Linq;
using System.Net;
using System.Net.Http;
using Xunit;

namespace Bootstrap.Admin.Controllers
{
    [Collection("BA-Logout")]
    public class LogoutTest
    {
        protected HttpClient client { get; set; }

        /// <summary>
        ///
        /// </summary>
        /// <param name="factory"></param>
        /// <param name="baseAddress"></param>
        public LogoutTest(BAWebHost factory, string baseAddress = "/")
        {
            client = factory.CreateClient(baseAddress);
        }

        [Theory]
        [InlineData("Login")]
        [InlineData("Login-Gitee")]
        [InlineData("Login-Blue")]
        [InlineData("Login-Tec")]
        [InlineData("Login-LTE")]
        public async void Login_UI_Ok(string view)
        {
            var r = await client.GetAsync("/Account/Logout");
            Assert.True(r.IsSuccessStatusCode);
            var content = await r.Content.ReadAsStringAsync();
            Assert.Contains("登 录", content);

            r = await client.GetAsync($"/Account/Login?AppId=BA&View={view}");
            content = await r.Content.ReadAsStringAsync();
            Assert.Contains("登 录", content);
        }

        [Fact]
        public async void Login_Empty()
        {
            var r = await client.GetAsync("/Account/Logout");
            Assert.True(r.IsSuccessStatusCode);
            var content = await r.Content.ReadAsStringAsync();
            Assert.Contains("登 录", content);

            r = await client.GetAsync("/Account/Login?AppId=BA&View=Login-Gitee");
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
            var r = await client.GetAsync("/Account/Login?AppId=BA");
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
            var r = await client.GetAsync("/Account/Logout");
            Assert.True(r.IsSuccessStatusCode);
            var content = await r.Content.ReadAsStringAsync();
            Assert.Contains("登 录", content);
        }

        [Theory]
        [InlineData("Gitee")]
        [InlineData("GitHub")]
        [InlineData("WeChat")]
        [InlineData("Alipay")]
        [InlineData("Tencent")]
        public async void OAuth_Ok(string url)
        {
            var r = await client.GetAsync($"/Account/{url}");
            Assert.True(r.IsSuccessStatusCode);
            var content = await r.Content.ReadAsStringAsync();
            Assert.Contains("登 录", content);
        }

        [Fact]
        public async void Lock_Ok()
        {
            // 登陆
            await client.LoginAsync();

            var r = await client.GetAsync("/Account/Lock");
            Assert.True(r.IsSuccessStatusCode);
            var content = await r.Content.ReadAsStringAsync();
            Assert.Contains("系统锁屏", content);

            // 第二次调用 跳转到登录页面
            r = await client.GetAsync("/Account/Lock");
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
            await client.PostAsync("/Account/Lock", data);
            UserHelper.Delete(UserHelper.Retrieves().Where(u => u.UserName == "13800010001").Select(u => u.Id));

            await client.PostAsync("/Account/Logout", data);
        }

        [Fact]
        public async void Mobile_Ok()
        {
            using (var db = DbManager.Create()) db.Execute("delete from Users where UserName = @0", "18910001000");
            var r = await client.GetAsync("/Account/Logout");
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
            m = await client.PostAsync("/Account/Lock", data);
            payload = await m.Content.ReadAsStringAsync();
            Assert.DoesNotContain("登 录", payload);
        }

        [Fact]
        public async void Mobile_Fail()
        {
            using (var db = DbManager.Create()) db.Execute("delete from Users where UserName = @0", "18910001000");
            var r = await client.GetAsync("/Account/Logout");
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

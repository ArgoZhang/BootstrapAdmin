using Microsoft.AspNetCore.Authentication.Cookies;
using Xunit;

namespace Bootstrap.Admin.Controllers
{
    public class HomeTest : ControllerTest
    {
        public HomeTest(BALoginWebHost factory) : base(factory, "Home/Error") { }

        [Theory]
        [InlineData(0)]
        [InlineData(404)]
        [InlineData(500)]
        public async void Error_Ok(int errorCode)
        {
            var r = await Client.GetAsync($"{errorCode}");
            Assert.True(r.IsSuccessStatusCode);
            var content = await r.Content.ReadAsStringAsync();
            if (errorCode == 0)
            {
                Assert.Contains("未处理服务器内部错误", content);
            }
            else if (errorCode == 404)
            {
                Assert.Contains("请求资源未找到", content);
            }
            else
            {
                Assert.Contains("服务器内部错误", content);
            }
        }

        [Fact]
        public async void Error_Redirect_Ok()
        {
            var r = await Client.GetAsync($"/Home/Error/404?{CookieAuthenticationDefaults.ReturnUrlParameter}=/Home/UnitTest");
            Assert.True(r.IsSuccessStatusCode);
            var content = await r.Content.ReadAsStringAsync();
            Assert.Contains("/Home/UnitTest", content);
        }
    }
}

using System.Net;
using System.Net.Http;
using Xunit;

namespace Bootstrap.Admin.Controllers
{
    public class ControllersTest : IClassFixture<BAWebHost>
    {
        private HttpClient _client;

        private BAWebHost _host;

        public ControllersTest(BAWebHost factory)
        {
            _host = factory;
            _client = factory.CreateClient();
        }

        [Fact]
        public async void Login_Fail()
        {
            // login
            _host.Logout();
            _client = _host.CreateClient();
            var r = await _client.GetAsync("/Account/Login");
            Assert.True(r.IsSuccessStatusCode);
            var content = await r.Content.ReadAsStringAsync();
            Assert.Contains("登 陆", content);
        }

        [Fact]
        public void Login_Admin()
        {
            // login
            _host.Logout();
            var resp = _host.Login();
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

        [Theory]
        [InlineData(0)]
        [InlineData(404)]
        [InlineData(500)]
        public async void Home_Error_Ok(int errorCode)
        {
            var r = await _client.GetAsync($"/Home/Error/{errorCode}");
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

        [Theory]
        [InlineData("Index", "欢迎使用后台管理")]
        [InlineData("Users", "用户管理")]
        [InlineData("Groups", "部门管理")]
        [InlineData("Dicts", "字典表维护")]
        [InlineData("Roles", "角色管理")]
        [InlineData("Menus", "菜单管理")]
        [InlineData("Logs", "系统日志")]
        [InlineData("FAIcon", "图标集")]
        [InlineData("IconView", "图标分类")]
        [InlineData("Settings", "网站设置")]
        [InlineData("Notifications", "通知管理")]
        [InlineData("Profiles", "个人中心")]
        [InlineData("Exceptions", "程序异常")]
        [InlineData("Messages", "站内消息")]
        [InlineData("Tasks", "任务管理")]
        [InlineData("Mobile", "客户端测试")]
        public async void View_Ok(string view, string text)
        {
            var r = await _client.GetAsync($"/Admin/{view}");
            Assert.True(r.IsSuccessStatusCode);
            var content = await r.Content.ReadAsStringAsync();
            Assert.Contains(text, content);
        }

        [Fact]
        public async void Admin_Error_Ok()
        {
            var r = await _client.GetAsync("/Admin/Error");
            Assert.False(r.IsSuccessStatusCode);
            Assert.Equal(HttpStatusCode.InternalServerError, r.StatusCode);
        }
    }
}

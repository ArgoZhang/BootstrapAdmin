using System.Net;
using Xunit;

namespace Bootstrap.Admin.Controllers
{
    public class AdminTest : ControllerTest
    {
        public AdminTest(BALoginWebHost factory) : base(factory, "Admin") { }

        [Theory]
        [InlineData("Index", "欢迎使用后台管理")]
        [InlineData("Users", "用户管理")]
        [InlineData("Groups", "部门管理")]
        [InlineData("Dicts", "字典表维护")]
        [InlineData("Roles", "角色管理")]
        [InlineData("Menus", "菜单管理")]
        [InlineData("Logs", "操作日志")]
        [InlineData("Traces", "访问日志")]
        [InlineData("Logins", "登录日志")]
        [InlineData("FAIcon", "图标集")]
        [InlineData("Sidebar", "后台管理")]
        [InlineData("IconView", "图标分类")]
        [InlineData("Settings", "网站设置")]
        [InlineData("Notifications", "通知管理")]
        [InlineData("Profiles", "个人中心")]
        [InlineData("Exceptions", "程序异常")]
        [InlineData("Healths", "健康检查")]
        [InlineData("Messages", "站内消息")]
        [InlineData("Online", "在线用户")]
        [InlineData("Tasks", "任务管理")]
        [InlineData("Mobile", "客户端测试")]
        [InlineData("Analyse", "网站分析")]
        [InlineData("SQL", "SQL日志")]
        public async void View_Ok(string view, string text)
        {
            var r = await Client.GetAsync(view);
            Assert.True(r.IsSuccessStatusCode);
            var content = await r.Content.ReadAsStringAsync();
            Assert.Contains(text, content);
        }

        [Fact]
        public async void Admin_Error_Ok()
        {
            var r = await Client.GetAsync("Error");
            Assert.False(r.IsSuccessStatusCode);
            Assert.Equal(HttpStatusCode.InternalServerError, r.StatusCode);
        }
    }
}

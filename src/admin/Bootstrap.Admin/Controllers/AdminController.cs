using Bootstrap.Admin.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using System;

namespace Bootstrap.Admin.Controllers
{
    /// <summary>
    /// 后台管理控制器
    /// </summary>
    [Authorize]
    public class AdminController : Controller
    {
        /// <summary>
        /// 后台管理首页
        /// </summary>
        /// <returns></returns>
        public ActionResult Index() => View(new NavigatorBarModel(this));

        /// <summary>
        /// 用户维护
        /// </summary>
        /// <returns></returns>
        public ActionResult Users() => View(new NavigatorBarModel(this));

        /// <summary>
        /// 部门维护
        /// </summary>
        /// <returns></returns>
        public ActionResult Groups() => View(new NavigatorBarModel(this));

        /// <summary>
        /// 字典表维护
        /// </summary>
        /// <returns></returns>
        public ActionResult Dicts() => View(new NavigatorBarModel(this));

        /// <summary>
        /// 角色维护
        /// </summary>
        /// <returns></returns>
        public ActionResult Roles() => View(new NavigatorBarModel(this));

        /// <summary>
        /// 菜单维护
        /// </summary>
        /// <returns></returns>
        public ActionResult Menus() => View(new NavigatorBarModel(this));

        /// <summary>
        /// 操作日志
        /// </summary>
        /// <returns></returns>
        public ActionResult Logs() => View(new NavigatorBarModel(this));

        /// <summary>
        /// 脚本日志
        /// </summary>
        /// <returns></returns>
        public ActionResult SQL() => View(new NavigatorBarModel(this));

        /// <summary>
        /// 访问日志
        /// </summary>
        /// <returns></returns>
        public ActionResult Traces() => View(new NavigatorBarModel(this));

        /// <summary>
        /// 登录日志
        /// </summary>
        /// <returns></returns>
        public ActionResult Logins() => View(new NavigatorBarModel(this));

        /// <summary>
        /// FA 图标页面
        /// </summary>
        /// <returns></returns>
        public ActionResult FAIcon() => View(new NavigatorBarModel(this));

        /// <summary>
        /// 健康检查
        /// </summary>
        /// <returns></returns>
        public ActionResult Healths() => View(new NavigatorBarModel(this));

        /// <summary>
        /// 图标视图
        /// </summary>
        /// <returns></returns>
        [AllowAnonymous]
        [ResponseCache(Duration = 600)]
        public PartialViewResult IconView() => PartialView("IconView");

        /// <summary>
        /// 侧边栏局部视图
        /// </summary>
        /// <returns></returns>
        /// <remark>菜单维护页面增删菜单时局部刷新时调用</remark>
        public PartialViewResult Sidebar() => PartialView("Sidebar", new NavigatorBarModel(this));

        /// <summary>
        /// 网站设置
        /// </summary>
        /// <returns></returns>
        public ActionResult Settings() => View(new SettingsModel(this));

        /// <summary>
        /// 通知管理
        /// </summary>
        /// <returns></returns>
        public ActionResult Notifications() => View(new NavigatorBarModel(this));

        /// <summary>
        /// 个人中心
        /// </summary>
        /// <param name="host"></param>
        /// <returns></returns>
        public ActionResult Profiles([FromServices]IWebHostEnvironment host) => View(new ProfilesModel(this, host));

        /// <summary>
        /// 程序异常
        /// </summary>
        /// <returns></returns>
        public ActionResult Exceptions() => View(new NavigatorBarModel(this));

        /// <summary>
        /// 消息通知
        /// </summary>
        /// <returns></returns>
        public ActionResult Messages() => View(new NavigatorBarModel(this));

        /// <summary>
        /// 任务管理
        /// </summary>
        /// <returns></returns>
        public ActionResult Tasks() => View(new TaskModel(this));

        /// <summary>
        /// 客户端测试页面
        /// </summary>
        /// <returns></returns>
        public ActionResult Mobile() => View(new NavigatorBarModel(this));

        /// <summary>
        /// 在线用户
        /// </summary>
        /// <returns></returns>
        public ActionResult Online() => View(new NavigatorBarModel(this));

        /// <summary>
        /// 网站分析统计
        /// </summary>
        /// <returns></returns>
        public ActionResult Analyse() => View(new NavigatorBarModel(this));

        /// <summary>
        /// 用于测试ExceptionFilter
        /// </summary>
        /// <returns></returns>
        public ActionResult Error() => throw new Exception("Customer Excetion UnitTest");
    }
}

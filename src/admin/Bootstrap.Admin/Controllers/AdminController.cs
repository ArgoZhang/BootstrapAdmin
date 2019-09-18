using Bootstrap.Admin.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using System;

namespace Bootstrap.Admin.Controllers
{
    /// <summary>
    /// 
    /// </summary>
    public class AdminController : Controller
    {
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public ActionResult Index() => View(new NavigatorBarModel(this));

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public ActionResult Users() => View(new NavigatorBarModel(this));

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public ActionResult Groups() => View(new NavigatorBarModel(this));

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public ActionResult Dicts() => View(new NavigatorBarModel(this));

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public ActionResult Roles() => View(new NavigatorBarModel(this));

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public ActionResult Menus() => View(new NavigatorBarModel(this));

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public ActionResult Logs() => View(new NavigatorBarModel(this));

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public ActionResult Traces() => View(new NavigatorBarModel(this));

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public ActionResult Logins() => View(new NavigatorBarModel(this));

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public ActionResult FAIcon() => View(new NavigatorBarModel(this));

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public ActionResult Healths() => View(new NavigatorBarModel(this));

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [AllowAnonymous]
        [ResponseCache(Duration = 600)]
        public PartialViewResult IconView() => PartialView("IconView");

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public ActionResult Settings() => View(new ThemeModel(this));

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public ActionResult Notifications() => View(new NavigatorBarModel(this));

        /// <summary>
        /// 
        /// </summary>
        /// <param name="host"></param>
        /// <returns></returns>
        public ActionResult Profiles([FromServices]IWebHostEnvironment host) => View(new ProfilesModel(this, host));

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public ActionResult Exceptions() => View(new NavigatorBarModel(this));

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public ActionResult Messages() => View(new NavigatorBarModel(this));

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public ActionResult Tasks() => View(new TaskModel(this));

        /// <summary>
        /// 
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

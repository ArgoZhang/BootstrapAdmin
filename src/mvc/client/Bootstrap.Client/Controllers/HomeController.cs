using Bootstrap.Client.Models;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;

namespace Bootstrap.Client.Controllers
{
    /// <summary>
    /// 前台主页控制器
    /// </summary>
    [Authorize]
    public class HomeController : Controller
    {
        /// <summary>
        /// 默认视图
        /// </summary>
        /// <returns></returns>
        public IActionResult Index()
        {
            return View(new NavigatorBarModel(this));
        }

        /// <summary>
        /// 演示视图
        /// </summary>
        /// <returns></returns>
        public IActionResult Dummy() => View(new NavigatorBarModel(this));

        /// <summary>
        /// 演示视图
        /// </summary>
        /// <returns></returns>
        public IActionResult Table() => View(new NavigatorBarModel(this));

        /// <summary>
        /// About 视图
        /// </summary>
        /// <returns></returns>
        public IActionResult About()
        {
            return View(new NavigatorBarModel(this));
        }

        /// <summary>
        /// 错误视图
        /// </summary>
        /// <param name="config"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        [AllowAnonymous]
        public IActionResult Error([FromServices]IConfiguration config, int id)
        {
            var options = config.GetBootstrapAdminAuthenticationOptions();
            var uriBuilder = new UriBuilder(options.AuthHost) { Query = QueryString.Create(CookieAuthenticationDefaults.ReturnUrlParameter, $"{Request.Scheme}://{Request.Host}{Request.PathBase}").ToString() };
            uriBuilder.Path = uriBuilder.Path == "/" ? Request.Path.Value : uriBuilder.Path + Request.Path.Value;
            return Redirect(uriBuilder.ToString());
        }
    }
}

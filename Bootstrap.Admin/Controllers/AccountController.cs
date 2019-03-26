using Bootstrap.Admin.Models;
using Bootstrap.DataAccess;
using Longbow.Web;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Bootstrap.Admin.Controllers
{
    /// <summary>
    /// Account controller.
    /// </summary>
    [AllowAnonymous]
    [AutoValidateAntiforgeryToken]
    public class AccountController : Controller
    {
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult Login()
        {
            if (DictHelper.RetrieveSystemModel())
            {
                ViewBag.UserName = "Admin";
                ViewBag.Password = "123789";
            }
            return User.Identity.IsAuthenticated ? (ActionResult)Redirect("~/Home/Index") : View("Login", new ModelBase());
        }

        /// <summary>
        /// Login the specified userName, password and remember.
        /// </summary>
        /// <returns>The login.</returns>
        /// <param name="onlineUserSvr"></param>
        /// <param name="userName">User name.</param>
        /// <param name="password">Password.</param>
        /// <param name="remember">Remember.</param>
        [HttpPost]
        public async Task<IActionResult> Login([FromServices]IOnlineUsers onlineUserSvr, string userName, string password, string remember)
        {
            if (UserHelper.Authenticate(userName, password, loginUser => CreateLoginUser(onlineUserSvr, HttpContext, loginUser)))
            {
                var identity = new ClaimsIdentity(CookieAuthenticationDefaults.AuthenticationScheme);
                identity.AddClaim(new Claim(ClaimTypes.Name, userName));
                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(identity), new AuthenticationProperties { ExpiresUtc = DateTimeOffset.Now.AddDays(DictHelper.RetrieveCookieExpiresPeriod()), IsPersistent = remember == "true" });
                // redirect origin url
                var originUrl = Request.Query[CookieAuthenticationDefaults.ReturnUrlParameter].FirstOrDefault() ?? "~/Home/Index";
                return Redirect(originUrl);
            }
            return View("Login", new ModelBase());
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="onlineUserSvr"></param>
        /// <param name="context"></param>
        /// <param name="loginUser"></param>
        internal static void CreateLoginUser(IOnlineUsers onlineUserSvr, HttpContext context, LoginUser loginUser)
        {
            var agent = new UserAgent(context.Request.Headers["User-Agent"]);
            loginUser.Ip = (context.Connection.RemoteIpAddress ?? IPAddress.IPv6Loopback).ToString();
            loginUser.City = onlineUserSvr.RetrieveLocaleByIp(loginUser.Ip);
            loginUser.Browser = $"{agent.Browser.Name} {agent.Browser.Version}";
            loginUser.OS = $"{agent.OS.Name} {agent.OS.Version}";
        }

        /// <summary>
        /// Logout this instance.
        /// </summary>
        /// <returns>The logout.</returns>
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return Redirect("~" + CookieAuthenticationDefaults.LoginPath);
        }

        /// <summary>
        /// Accesses the denied.
        /// </summary>
        /// <returns>The denied.</returns>
        [ResponseCache(Duration = 600)]
        public ActionResult AccessDenied() => View("Error", ErrorModel.CreateById(403));
    }
}
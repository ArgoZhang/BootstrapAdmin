using Longbow.Configuration;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Web;

namespace Bootstrap.Client.Controllers
{
    /// <summary>
    /// Account controller.
    /// </summary>
    [AllowAnonymous]
    public class AccountController : Controller
    {
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public ActionResult Login()
        {
            var originUrl = Request.Query[CookieAuthenticationDefaults.ReturnUrlParameter].FirstOrDefault();
            if (!string.IsNullOrEmpty(originUrl)) originUrl = $"?{CookieAuthenticationDefaults.ReturnUrlParameter}={HttpUtility.UrlEncode(originUrl)}";
            return Redirect($"{ConfigurationManager.AppSettings["AuthHost"]}{CookieAuthenticationDefaults.LoginPath}{originUrl}");
        }
        /// <summary>
        /// Logout this instance.
        /// </summary>
        /// <returns>The logout.</returns>
        public IActionResult Logout()
        {
            return Redirect($"{ConfigurationManager.AppSettings["AuthHost"]}{CookieAuthenticationDefaults.LogoutPath}");
        }
        /// <summary>
        /// Accesses the denied.
        /// </summary>
        /// <returns>The denied.</returns>
        [ResponseCache(Duration = 600)]
        public ActionResult AccessDenied()
        {
            return View();
        }
    }
}
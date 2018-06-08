using Bootstrap.Admin.Models;
using Bootstrap.DataAccess;
using Bootstrap.Security;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Bootstrap.Admin.Controllers
{
    /// <summary>
    /// Account controller.
    /// </summary>
    [AllowAnonymous]
    public class AccountController : Controller
    {
        /// <summary>
        /// Login the specified userName, password and remember.
        /// </summary>
        /// <returns>The login.</returns>
        /// <param name="userName">User name.</param>
        /// <param name="password">Password.</param>
        /// <param name="remember">Remember.</param>
        public async Task<IActionResult> Login(string userName, string password, string remember)
        {
            if (!string.IsNullOrEmpty(userName) && BootstrapUser.Authenticate(userName, password))
            {
                var identity = new ClaimsIdentity(CookieAuthenticationDefaults.AuthenticationScheme);
                identity.AddClaim(new Claim(ClaimTypes.Name, userName));
                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(identity), new AuthenticationProperties() { IsPersistent = remember == "true" });
                return Redirect("~/");
            }
            var mobile = true; //Request.Browser.IsMobileDevice;
            var model = "IPad"; //Request.Browser.MobileDeviceModel;
            return mobile && model != "IPad" ? View("Loginm", new ModelBase()) : View("Login", new ModelBase());
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
        public ActionResult AccessDenied()
        {
            return View();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public ActionResult Register(User p)
        {
            if (string.IsNullOrEmpty(p.UserName) || string.IsNullOrEmpty(p.Password) || string.IsNullOrEmpty(p.DisplayName) || string.IsNullOrEmpty(p.Description)) return View();
            p.UserStatus = 1;
            return UserHelper.SaveUser(p) ? (ActionResult)Redirect("~/html/RegResult.html") : View();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public ActionResult Mobile()
        {
            return View();
        }
    }
}
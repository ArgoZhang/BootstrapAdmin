using Bootstrap.Admin.Models;
using Bootstrap.DataAccess;
using Longbow.GiteeAuth;
using Longbow.GitHubAuth;
using Longbow.Web;
using Longbow.WeChatAuth;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
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
        private const string MobileSchema = "Mobile";
        /// <summary>
        /// 系统锁屏界面
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<ActionResult> Lock()
        {
            if (!User.Identity.IsAuthenticated) return Login();

            var authenticationType = User.Identity.AuthenticationType;
            await HttpContext.SignOutAsync();
            var urlReferrer = Request.Headers["Referer"].FirstOrDefault();
            return View(new LockModel(this)
            {
                AuthenticationType = authenticationType,
                ReturnUrl = WebUtility.UrlEncode(string.IsNullOrEmpty(urlReferrer) ? CookieAuthenticationDefaults.LoginPath.Value : urlReferrer)
            });
        }

        /// <summary>
        /// 系统锁屏界面
        /// </summary>
        /// <param name="provider"></param>
        /// <param name="userName"></param>
        /// <param name="password"></param>
        /// <param name="authType"></param>
        /// <returns></returns>
        [HttpPost]
        [IgnoreAntiforgeryToken]
        public Task<IActionResult> Lock([FromServices]ISMSProvider provider, string userName, string password, string authType)
        {
            // 根据不同的登陆方式
            Task<IActionResult> ret;
            if (authType == MobileSchema) ret = Mobile(provider, userName, password);
            else ret = Login(userName, password, string.Empty);
            return ret;
        }

        /// <summary>
        /// 系统登录方法
        /// </summary>
        /// <param name="appId"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult Login([FromQuery] string appId = "0")
        {
            if (DictHelper.RetrieveSystemModel())
            {
                ViewBag.UserName = "Admin";
                ViewBag.Password = "123789";
            }
            return User.Identity.IsAuthenticated ? (ActionResult)Redirect("~/Home/Index") : View("Login", new LoginModel(appId));
        }

        /// <summary>
        /// 短信验证登陆方法
        /// </summary>
        /// <param name="provider"></param>
        /// <param name="phone"></param>
        /// <param name="code"></param>
        /// <returns></returns>
        [HttpPost()]
        public async Task<IActionResult> Mobile([FromServices]ISMSProvider provider, string phone, string code)
        {
            var auth = provider.Validate(phone, code);
            HttpContext.Log(phone, auth);
            if (auth)
            {
                var user = UserHelper.Retrieves().FirstOrDefault(u => u.UserName == phone);
                if (user == null)
                {
                    user = new User()
                    {
                        ApprovedBy = "Mobile",
                        ApprovedTime = DateTime.Now,
                        DisplayName = "手机用户",
                        UserName = phone,
                        Password = code,
                        Icon = "default.jpg",
                        Description = "手机用户",
                        App = provider.Option.App
                    };
                    UserHelper.Save(user);

                    // 根据配置文件设置默认角色
                    var roles = RoleHelper.Retrieves().Where(r => provider.Option.Roles.Any(rl => rl.Equals(r.RoleName, StringComparison.OrdinalIgnoreCase))).Select(r => r.Id);
                    RoleHelper.SaveByUserId(user.Id, roles);
                }
            }
            return auth ? await SignInAsync(phone, true, MobileSchema) : RedirectLogin();
        }

        private IActionResult RedirectLogin()
        {
            var query = Request.Query.Aggregate(new Dictionary<string, string>(), (d, v) =>
            {
                d.Add(v.Key, v.Value.ToString());
                return d;
            });
            return Redirect(QueryHelpers.AddQueryString(Request.PathBase + CookieAuthenticationDefaults.LoginPath, query));
        }

        /// <summary>
        /// Login the specified userName, password and remember.
        /// </summary>
        /// <returns>The login.</returns>
        /// <param name="userName">User name.</param>
        /// <param name="password">Password.</param>
        /// <param name="remember">Remember.</param>
        /// <param name="appId"></param>
        [HttpPost]
        public async Task<IActionResult> Login(string userName, string password, string remember, string appId = "0")
        {
            var auth = UserHelper.Authenticate(userName, password);
            HttpContext.Log(userName, auth);
            return auth ? await SignInAsync(userName, remember == "true") : View("Login", new LoginModel(appId) { AuthFailed = true });
        }

        private async Task<IActionResult> SignInAsync(string userName, bool persistent, string authenticationScheme = CookieAuthenticationDefaults.AuthenticationScheme)
        {
            var identity = new ClaimsIdentity(authenticationScheme);
            identity.AddClaim(new Claim(ClaimTypes.Name, userName));
            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(identity), new AuthenticationProperties { ExpiresUtc = DateTimeOffset.Now.AddDays(DictHelper.RetrieveCookieExpiresPeriod()), IsPersistent = persistent });

            // redirect origin url
            var originUrl = Request.Query[CookieAuthenticationDefaults.ReturnUrlParameter].FirstOrDefault() ?? "~/Home/Index";
            return Redirect(originUrl);
        }

        /// <summary>
        /// Logout this instance.
        /// </summary>
        /// <param name="appId"></param>
        /// <returns>The logout.</returns>
        [HttpGet]
        public async Task<IActionResult> Logout([FromQuery]string appId = "0")
        {
            await HttpContext.SignOutAsync();
            return Redirect(QueryHelpers.AddQueryString(Request.PathBase + CookieAuthenticationDefaults.LoginPath, "AppId", appId));
        }

        /// <summary>
        /// Accesses the denied.
        /// </summary>
        /// <returns>The denied.</returns>
        [ResponseCache(Duration = 600)]
        [HttpGet]
        public ActionResult AccessDenied() => View("Error", ErrorModel.CreateById(403));

        /// <summary>
        /// Gitee 认证
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IActionResult Gitee([FromServices]IConfiguration config)
        {
            var enabled = config.GetValue($"{nameof(GiteeOptions)}:Enabled", false);
            return Challenge(enabled ? GiteeDefaults.AuthenticationScheme : CookieAuthenticationDefaults.AuthenticationScheme);
        }

        /// <summary>
        /// GitHub 认证
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IActionResult GitHub([FromServices]IConfiguration config)
        {
            var enabled = config.GetValue($"{nameof(GitHubOptions)}:Enabled", false);
            return Challenge(enabled ? GitHubDefaults.AuthenticationScheme : CookieAuthenticationDefaults.AuthenticationScheme);
        }

        /// <summary>
        /// WeChat 认证
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IActionResult WeChat([FromServices]IConfiguration config)
        {
            var enabled = config.GetValue($"{nameof(GitHubOptions)}:Enabled", false);
            return Challenge(enabled ? WeChatDefaults.AuthenticationScheme : CookieAuthenticationDefaults.AuthenticationScheme);
        }
    }
}

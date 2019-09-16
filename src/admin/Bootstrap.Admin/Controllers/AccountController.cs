using Bootstrap.Admin.Models;
using Bootstrap.DataAccess;
using Longbow.GiteeAuth;
using Longbow.GitHubAuth;
using Longbow.Security.Cryptography;
using Longbow.Web;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
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
        /// 系统锁屏界面
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<ActionResult> Lock()
        {
            if (!User.Identity.IsAuthenticated) return Login();

            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            var urlReferrer = Request.Headers["Referer"].FirstOrDefault();
            return View(new LockModel(this)
            {
                ReturnUrl = WebUtility.UrlEncode(string.IsNullOrEmpty(urlReferrer) ? CookieAuthenticationDefaults.LoginPath.Value : urlReferrer)
            });
        }

        /// <summary>
        /// 系统锁屏界面
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [IgnoreAntiforgeryToken]
        public Task<IActionResult> Lock([FromServices]IOnlineUsers onlineUserSvr, [FromServices]IIPLocatorProvider ipLocator, string userName, string password) => Login(onlineUserSvr, ipLocator, userName, password, string.Empty);

        /// <summary>
        /// 系统登录方法
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
            return User.Identity.IsAuthenticated ? (ActionResult)Redirect("~/Home/Index") : View("Login", new LoginModel());
        }

        /// <summary>
        /// 短信验证登陆方法
        /// </summary>
        /// <param name="onlineUserSvr"></param>
        /// <param name="ipLocator"></param>
        /// <param name="configuration"></param>
        /// <param name="phone"></param>
        /// <param name="code"></param>
        /// <returns></returns>
        [HttpPost()]
        public async Task<IActionResult> Mobile([FromServices]IOnlineUsers onlineUserSvr, [FromServices]IIPLocatorProvider ipLocator, [FromServices]IConfiguration configuration, [FromQuery]string phone, [FromQuery]string code)
        {
            var option = configuration.GetSection(nameof(SMSOptions)).Get<SMSOptions>();
            if (UserHelper.AuthenticateMobile(phone, code, option.MD5Key, loginUser => CreateLoginUser(onlineUserSvr, ipLocator, HttpContext, loginUser)))
            {
                var identity = new ClaimsIdentity(CookieAuthenticationDefaults.AuthenticationScheme);
                identity.AddClaim(new Claim(ClaimTypes.Name, phone));
                identity.AddClaim(new Claim(ClaimTypes.Role, "Default"));
                await HttpContext.SignInAsync(new ClaimsPrincipal(identity));

                if (UserHelper.RetrieveUserByUserName(identity) == null)
                {
                    var user = new User()
                    {
                        ApprovedBy = "Mobile",
                        ApprovedTime = DateTime.Now,
                        DisplayName = "手机用户",
                        UserName = phone,
                        Password = LgbCryptography.GenerateSalt(),
                        Icon = "default.jpg",
                        Description = "手机用户",
                        App = "2"
                    };
                    UserHelper.Save(user);
                    CacheCleanUtility.ClearCache(cacheKey: UserHelper.RetrieveUsersDataKey);
                }

                // redirect origin url
                var originUrl = Request.Query[CookieAuthenticationDefaults.ReturnUrlParameter].FirstOrDefault() ?? "~/Home/Index";
                return Redirect(originUrl);
            }
            return View("Login", new LoginModel() { AuthFailed = true });
        }

        /// <summary>
        /// Login the specified userName, password and remember.
        /// </summary>
        /// <returns>The login.</returns>
        /// <param name="onlineUserSvr"></param>
        /// <param name="ipLocator"></param>
        /// <param name="userName">User name.</param>
        /// <param name="password">Password.</param>
        /// <param name="remember">Remember.</param>
        [HttpPost]
        public async Task<IActionResult> Login([FromServices]IOnlineUsers onlineUserSvr, [FromServices]IIPLocatorProvider ipLocator, string userName, string password, string remember)
        {
            if (UserHelper.Authenticate(userName, password, loginUser => CreateLoginUser(onlineUserSvr, ipLocator, HttpContext, loginUser)))
            {
                var identity = new ClaimsIdentity(CookieAuthenticationDefaults.AuthenticationScheme);
                identity.AddClaim(new Claim(ClaimTypes.Name, userName));
                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(identity), new AuthenticationProperties { ExpiresUtc = DateTimeOffset.Now.AddDays(DictHelper.RetrieveCookieExpiresPeriod()), IsPersistent = remember == "true" });
                // redirect origin url
                var originUrl = Request.Query[CookieAuthenticationDefaults.ReturnUrlParameter].FirstOrDefault() ?? "~/Home/Index";
                return Redirect(originUrl);
            }
            return View("Login", new LoginModel() { AuthFailed = true });
        }

        /// <summary>
        /// 创建登录用户信息
        /// </summary>
        /// <param name="onlineUserSvr"></param>
        /// <param name="ipLocator"></param>
        /// <param name="context"></param>
        /// <param name="loginUser"></param>
        internal static void CreateLoginUser(IOnlineUsers onlineUserSvr, IIPLocatorProvider ipLocator, HttpContext context, LoginUser loginUser)
        {
            loginUser.UserAgent = context.Request.Headers["User-Agent"];
            var agent = new UserAgent(loginUser.UserAgent);
            loginUser.Ip = context.Connection.RemoteIpAddress.ToIPv4String();
            loginUser.City = ipLocator.Locate(loginUser.Ip);
            loginUser.Browser = $"{agent.Browser?.Name} {agent.Browser?.Version}";
            loginUser.OS = $"{agent.OS?.Name} {agent.OS?.Version}";
        }

        /// <summary>
        /// Logout this instance.
        /// </summary>
        /// <returns>The logout.</returns>
        [HttpGet]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync();
            return Redirect(Request.PathBase + CookieAuthenticationDefaults.LoginPath);
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
            var enabled = config.GetValue($"{nameof(GiteeOptions)}:Eanbeld", false);
            return Challenge(enabled ? GiteeDefaults.AuthenticationScheme : CookieAuthenticationDefaults.AuthenticationScheme);
        }

        /// <summary>
        /// GitHub 认证
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IActionResult GitHub([FromServices]IConfiguration config)
        {
            var enabled = config.GetValue($"{nameof(GitHubOptions)}:Eanbeld", false);
            return Challenge(enabled ? GitHubDefaults.AuthenticationScheme : CookieAuthenticationDefaults.AuthenticationScheme);
        }
    }
}

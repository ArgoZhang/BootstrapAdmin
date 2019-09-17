using Bootstrap.Admin.Models;
using Bootstrap.DataAccess;
using Longbow.GiteeAuth;
using Longbow.GitHubAuth;
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
        /// <param name="userName"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        [HttpPost]
        [IgnoreAntiforgeryToken]
        public Task<IActionResult> Lock(string userName, string password)
        {
            // 根据不同的登陆方式
            return Login(userName, password, string.Empty);
        }

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
        /// <param name="ipLocator"></param>
        /// <param name="configuration"></param>
        /// <param name="phone"></param>
        /// <param name="code"></param>
        /// <returns></returns>
        [HttpPost()]
        public async Task<IActionResult> Mobile([FromServices]IConfiguration configuration, [FromQuery]string phone, [FromQuery]string code)
        {
            var option = configuration.GetSection(nameof(SMSOptions)).Get<SMSOptions>();
            var auth = SMSHelper.Validate(phone, code, option.MD5Key);
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
                        App = option.App
                    };
                    UserHelper.Save(user);

                    // 根据配置文件设置默认角色
                    var roles = RoleHelper.Retrieves().Where(r => option.Roles.Any(rl => rl.Equals(r.RoleName, StringComparison.OrdinalIgnoreCase))).Select(r => r.Id);
                    RoleHelper.SaveByUserId(user.Id, roles);
                }
            }
            return auth ? await SignInAsync(phone, true) : View("Login", new LoginModel() { AuthFailed = true });
        }

        /// <summary>
        /// Login the specified userName, password and remember.
        /// </summary>
        /// <returns>The login.</returns>
        /// <param name="userName">User name.</param>
        /// <param name="password">Password.</param>
        /// <param name="remember">Remember.</param>
        [HttpPost]
        public async Task<IActionResult> Login(string userName, string password, string remember)
        {
            var auth = UserHelper.Authenticate(userName, password);
            HttpContext.Log(userName, auth);
            return auth ? await SignInAsync(userName, remember == "true") : View("Login", new LoginModel() { AuthFailed = true });
        }

        private async Task<IActionResult> SignInAsync(string userName, bool persistent)
        {
            var identity = new ClaimsIdentity(CookieAuthenticationDefaults.AuthenticationScheme);
            identity.AddClaim(new Claim(ClaimTypes.Name, userName));
            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(identity), new AuthenticationProperties { ExpiresUtc = DateTimeOffset.Now.AddDays(DictHelper.RetrieveCookieExpiresPeriod()), IsPersistent = persistent });

            // redirect origin url
            var originUrl = Request.Query[CookieAuthenticationDefaults.ReturnUrlParameter].FirstOrDefault() ?? "~/Home/Index";
            return Redirect(originUrl);
        }

        /// <summary>
        /// 创建登录用户信息
        /// </summary>
        /// <param name="ipLocator"></param>
        /// <param name="context"></param>
        /// <param name="loginUser"></param>
        internal static void CreateLoginUser(IIPLocatorProvider ipLocator, HttpContext context, LoginUser loginUser)
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
    }
}

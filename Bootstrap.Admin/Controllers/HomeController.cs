using Bootstrap.Admin.Models;
using Bootstrap.DataAccess;
using Bootstrap.Security;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Bootstrap.Admin.Controllers
{
    /// <summary>
    /// 
    /// </summary>
    public class HomeController : Controller
    {
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            var v = new HeaderBarModel(User.Identity) { HomeUrl = DictHelper.RetrieveHomeUrl() };
            return v.HomeUrl.StartsWith("~/") ? (ActionResult)View(v) : Redirect(v.HomeUrl);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public async Task<IActionResult> Lock()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            var user = BootstrapUser.RetrieveUserByUserName(User.Identity.Name);
            return View(new LockModel
            {
                UserName = user.UserName,
                DisplayName = user.DisplayName,
                ReturnUrl = Request.Path
            });
        }
    }
}
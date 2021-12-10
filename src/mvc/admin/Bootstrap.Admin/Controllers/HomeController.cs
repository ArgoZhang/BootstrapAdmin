using Bootstrap.Admin.Models;
using Bootstrap.DataAccess;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace Bootstrap.Admin.Controllers
{
    /// <summary>
    /// Home Controller
    /// </summary>
    [Authorize]
    public class HomeController : Controller
    {
        /// <summary>
        /// Index View
        /// </summary>
        /// <returns></returns>
        public IActionResult Index()
        {
            var model = new HeaderBarModel(User.Identity!.Name);
            var homeUrl = DictHelper.RetrieveHomeUrl(User.Identity.Name, model.AppId);
            var useBlazor = DictHelper.RetrieveEnableBlazor();
            return homeUrl.Equals("~/Home/Index", System.StringComparison.OrdinalIgnoreCase) ? (useBlazor ? Redirect("~/Pages") : (IActionResult)View(model)) : Redirect(homeUrl);
        }

        /// <summary>
        /// Error View
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [AllowAnonymous]
        public IActionResult Error(int? id = 0)
        {
            var model = ErrorModel.CreateById(id ?? 0);
            if (id != 403)
            {
                var returnUrl = Request.Query[CookieAuthenticationDefaults.ReturnUrlParameter].ToString();
                if (!string.IsNullOrEmpty(returnUrl)) model.ReturnUrl = returnUrl;
            }
            return View(model);
        }
    }
}

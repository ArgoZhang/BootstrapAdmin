using Bootstrap.Admin.Models;
using Bootstrap.DataAccess;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Bootstrap.Admin.Controllers
{
    /// <summary>
    /// Home Controller
    /// </summary>
    [AllowAnonymous]
    public class HomeController : Controller
    {
        /// <summary>
        /// Index View
        /// </summary>
        /// <returns></returns>
        public IActionResult Index()
        {
            var userName = User.Identity.Name;
            if (string.IsNullOrEmpty(userName)) return Redirect(Request.PathBase + CookieAuthenticationDefaults.LoginPath);

            var model = new HeaderBarModel(userName);
            if (string.IsNullOrEmpty(model.UserName)) return Redirect(Request.PathBase + CookieAuthenticationDefaults.LogoutPath);

            var homeUrl = DictHelper.RetrieveHomeUrl(model.AppId);
            return homeUrl.Equals("~/Home/Index", System.StringComparison.OrdinalIgnoreCase) ? (IActionResult)View(model) : Redirect(homeUrl);
        }

        /// <summary>
        /// Error View
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public IActionResult Error(int id)
        {
            var model = ErrorModel.CreateById(id);
            if (id != 403)
            {
                var returnUrl = Request.Query[CookieAuthenticationDefaults.ReturnUrlParameter].ToString();
                if (!string.IsNullOrEmpty(returnUrl)) model.ReturnUrl = returnUrl;
            }
            return View(model);
        }
    }
}

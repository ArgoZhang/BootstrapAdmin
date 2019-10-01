using Bootstrap.Admin.Models;
using Bootstrap.DataAccess;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

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
        public IActionResult Index()
        {
            var model = new HeaderBarModel(User.Identity);
            if (string.IsNullOrEmpty(model.UserName)) return Redirect(Request.PathBase + CookieAuthenticationDefaults.LogoutPath);
            var url = DictHelper.RetrieveHomeUrl(model.AppId);
            return url.Equals("~/Home/Index", System.StringComparison.OrdinalIgnoreCase) ? (IActionResult)View(model) : Redirect(url);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [AllowAnonymous]
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

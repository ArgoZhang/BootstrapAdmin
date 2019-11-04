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
        public IActionResult Index([FromServices]IConfiguration configuration)
        {
            var model = new HeaderBarModel(User.Identity.Name);
            var homeUrl = DictHelper.RetrieveHomeUrl(model.AppId);
            var useBlazor = configuration.GetValue("UseBlazor", false);
            return useBlazor ? Redirect("~/Admin/Home") : homeUrl.Equals("~/Home/Index", System.StringComparison.OrdinalIgnoreCase) ? (IActionResult)View(model) : Redirect(homeUrl);
        }

        /// <summary>
        /// Error View
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

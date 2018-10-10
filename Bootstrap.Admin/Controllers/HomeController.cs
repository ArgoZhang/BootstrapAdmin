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
            var url = DictHelper.RetrieveHomeUrl();
            return url.Equals("~/Home/Index", System.StringComparison.OrdinalIgnoreCase) ? (IActionResult)View(new HeaderBarModel(User.Identity)) : Redirect(url);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [AllowAnonymous]
        public IActionResult Error(int id)
        {
            ViewBag.ReturnUrl = Request.Query[CookieAuthenticationDefaults.ReturnUrlParameter].ToString() ?? Url.Content("~/Home/Index");
            return id == 404 ? View("NotFound") : View();
        }
    }
}
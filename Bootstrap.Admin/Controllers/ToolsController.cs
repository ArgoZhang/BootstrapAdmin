using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Bootstrap.Admin.Controllers
{
    /// <summary>
    /// 
    /// </summary>
    [AllowAnonymous]
    public class ToolsController : Controller
    {
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public IActionResult Index()
        {
            var returnUrl = Request.Query[CookieAuthenticationDefaults.ReturnUrlParameter].ToString();
            return Redirect(returnUrl);
        }
    }
}
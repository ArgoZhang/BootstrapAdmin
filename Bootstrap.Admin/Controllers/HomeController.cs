using Bootstrap.Admin.Models;
using System.Web.Mvc;
using System.Web.Security;

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
            var v = new HeaderBarModel();
            return View(v);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public ActionResult Rules()
        {
            var v = new HeaderBarModel();
            v.BreadcrumbName = "规则设置";
            return View(v);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public ActionResult Terminals()
        {
            var v = new HeaderBarModel();
            v.BreadcrumbName = "输入口设置";
            return View(v);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="username"></param>
        /// <param name="password"></param>
        /// <param name="remember"></param>
        /// <returns></returns>
        [AllowAnonymous]
        public ActionResult Login(string username, string password, string remember)
        {
            if (username == "Argo")
            {
                FormsAuthentication.RedirectFromLoginPage(username, false);
            }
            return View();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("Login");
        }
    }
}
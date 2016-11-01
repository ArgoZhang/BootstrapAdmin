using Bootstrap.Admin.Models;
using Bootstrap.DataAccess;
using Longbow.Security.Principal;
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
            v.ShowMenu = "hide";
            return View(v);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="password"></param>
        /// <param name="remember"></param>
        /// <returns></returns>
        [AllowAnonymous]
        public ActionResult Login(string userName, string password, string remember)
        {
            //UNDONE: 本方法有严重安全漏洞，发布前需要修正
            var model = new LoginModel();
            model.UserName = userName;
            if (LgbPrincipal.IsAdmin(userName) || UserHelper.Authenticate(userName, password))
            {
                LgbPrincipal.SavePrincipalCookie(new LgbUser() { RealUserName = userName });
                FormsAuthentication.RedirectFromLoginPage(userName, false);
            }
            return View(model);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [AllowAnonymous]
        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("Login");
        }
    }
}
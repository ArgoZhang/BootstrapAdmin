using Bootstrap.Admin.Models;
using Bootstrap.DataAccess;
using Longbow.Security.Principal;
using System;
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
            var v = new ContentModel();
            return View(v);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public ActionResult Lock(LockModel model)
        {
            if (!string.IsNullOrEmpty(model.Password))
            {
                return RedirectToAction("Login", new { userName = model.UserName, password = model.Password });
            }
            var user = UserHelper.RetrieveUsersByName(User.Identity.Name);
            model.UserName = user.UserName;
            model.DisplayName = user.DisplayName;
            return View(model);
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
            if (string.IsNullOrEmpty(userName)) return View(model);
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
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [AllowAnonymous]
        public ActionResult Register(User p)
        {
            if (string.IsNullOrEmpty(p.UserName) || string.IsNullOrEmpty(p.Password) || string.IsNullOrEmpty(p.DisplayName) || string.IsNullOrEmpty(p.Description)) return View();
            p.UserStatus = 1;
            var result = UserHelper.SaveUser(p);
            if (result)
            {
                return Redirect("/Content/html/RegResult.html");
            }

            else
                return View();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [AllowAnonymous]
        public ActionResult Mobile()
        {
            return View();
        }
    }
}
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
            v.HomeUrl = DictHelper.RetrieveHomeUrl();
            if (v.HomeUrl.StartsWith("~/"))
            {
                v.Menus = MenuHelper.RetrieveLinksByUserName(User.Identity.Name);
                return View(v);
            }
            else return Redirect(v.HomeUrl);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public ActionResult Lock()
        {
            var user = UserHelper.RetrieveUsersByName(User.Identity.Name);
            var model = new LockModel();
            model.UserName = user.UserName;
            model.DisplayName = user.DisplayName;
            model.ReturnUrl = Url.Encode(Request.UrlReferrer.AbsoluteUri);
            FormsAuthentication.SignOut();
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
            var model = new LoginModel();
            if (string.IsNullOrEmpty(userName)) return View(model);
            model.UserName = userName;
            if (LgbPrincipal.IsAdmin(userName, password) || UserHelper.Authenticate(userName, password))
            {
                FormsAuthentication.RedirectFromLoginPage(userName, remember == "true");
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
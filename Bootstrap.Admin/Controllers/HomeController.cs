using Bootstrap.Admin.Models;
using Bootstrap.DataAccess;
using Bootstrap.Security;
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
            var v = new HeaderBarModel { HomeUrl = DictHelper.RetrieveHomeUrl() };
            return v.HomeUrl.StartsWith("~/") ? (ActionResult)View(v) : Redirect(v.HomeUrl);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public ActionResult Lock()
        {
            FormsAuthentication.SignOut();
            var user = UserHelper.RetrieveUsersByName(User.Identity.Name);
            return View(new LockModel
            {
                UserName = user.UserName,
                DisplayName = user.DisplayName,
                ReturnUrl = Url.Encode(Request.UrlReferrer == null ? FormsAuthentication.DefaultUrl : Request.UrlReferrer.AbsoluteUri)
            });
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="login"></param>
        /// <returns></returns>
        [AllowAnonymous]
        public ActionResult Login(LoginModel login)
        {
            FormsAuthentication.SignOut();
            if (!string.IsNullOrEmpty(login.UserName) && (LgbPrincipal.Authenticate(login.UserName, login.Password) || BootstrapUser.Authenticate(login.UserName, login.Password)))
            {
                FormsAuthentication.RedirectFromLoginPage(login.UserName, login.Remember == "true");
                return new EmptyResult();
            }
            return View(login);
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
            return UserHelper.SaveUser(p) ? (ActionResult)Redirect("~/Content/html/RegResult.html") : View();
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
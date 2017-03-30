using Bootstrap.Admin.Models;
using System.Web.Mvc;

namespace Bootstrap.Admin.Controllers
{
    /// <summary>
    /// 
    /// </summary>
    public class AdminController : Controller
    {
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            return View(new NavigatorBarModel("~/Admin/Index"));
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public ActionResult Users()
        {
            return View(new NavigatorBarModel("~/Admin/Users"));
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public ActionResult Groups()
        {
            return View(new NavigatorBarModel("~/Admin/Groups"));
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public ActionResult Dicts()
        {
            return View(new NavigatorBarModel("~/Admin/Dicts"));
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public ActionResult Roles()
        {
            return View(new NavigatorBarModel("~/Admin/Roles"));
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public ActionResult Menus()
        {
            return View(new NavigatorBarModel("~/Admin/Menus"));
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public ActionResult Logs()
        {
            return View(new NavigatorBarModel("~/Admin/Logs"));
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public ActionResult FAIcon()
        {
            return View(new NavigatorBarModel("~/Admin/FAIcon"));
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [OutputCache(CacheProfile = "IconView")]
        public PartialViewResult IconView()
        {
            Response.Cache.SetOmitVaryStar(true);
            return PartialView("IconView");
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public ActionResult Settings()
        {
            return View(new NavigatorBarModel("~/Admin/Settings"));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public ActionResult Notifications()
        {
            return View(new NavigatorBarModel("~/Admin/Notifications"));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public ActionResult Profiles()
        {
            return View(new ProfilesModel("~/Admin/Profiles"));
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public ActionResult Exceptions()
        {
            return View(new NavigatorBarModel("~/Admin/Exceptions"));
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public ActionResult Messages()
        {
            return View(new NavigatorBarModel("~/Admin/Messages"));
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public ActionResult Tasks()
        {
            return View(new NavigatorBarModel("~/Admin/Tasks"));
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public ActionResult Mobile()
        {
            return View(new NavigatorBarModel("~/Admin/Mobile"));
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public ActionResult Api()
        {
            return View(new NavigatorBarModel("~/Admin/Api"));
        }
    }
}

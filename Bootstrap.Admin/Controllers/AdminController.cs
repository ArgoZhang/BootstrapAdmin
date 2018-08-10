using Bootstrap.Admin.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

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
            return View(new NavigatorBarModel(this));
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public ActionResult Users()
        {
            return View(new NavigatorBarModel(this));
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public ActionResult Groups()
        {
            return View(new NavigatorBarModel(this));
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public ActionResult Dicts()
        {
            return View(new NavigatorBarModel(this));
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public ActionResult Roles()
        {
            return View(new NavigatorBarModel(this));
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public ActionResult Menus()
        {
            return View(new NavigatorBarModel(this));
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public ActionResult Logs()
        {
            return View(new NavigatorBarModel(this));
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public ActionResult FAIcon()
        {
            return View(new NavigatorBarModel(this));
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [AllowAnonymous]
        [ResponseCache(Duration = 600)]
        public PartialViewResult IconView()
        {
            return PartialView("IconView");
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public ActionResult Settings()
        {
            return View(new NavigatorBarModel(this));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public ActionResult Notifications()
        {
            return View(new NavigatorBarModel(this));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public ActionResult Profiles()
        {
            return View(new ProfilesModel(this));
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public ActionResult Exceptions()
        {
            return View(new NavigatorBarModel(this));
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public ActionResult Messages()
        {
            return View(new NavigatorBarModel(this));
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public ActionResult Tasks()
        {
            return View(new NavigatorBarModel(this));
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public ActionResult Mobile()
        {
            return View(new NavigatorBarModel(this));
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public ActionResult Api()
        {
            return View(new NavigatorBarModel(this));
        }
    }
}

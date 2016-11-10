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
            var v = new NavigatorBarModel("~/Admin/Index");
            return View(v);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public ActionResult Users()
        {
            var v = new NavigatorBarModel("~/Admin/Users");
            return View(v);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public ActionResult Groups()
        {
            var v = new NavigatorBarModel("~/Admin/Groups");
            return View(v);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public ActionResult Dicts()
        {
            var v = new NavigatorBarModel("~/Admin/Dicts");
            return View(v);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public ActionResult Roles()
        {
            var v = new NavigatorBarModel("~/Admin/Roles");
            return View(v);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public ActionResult Menus()
        {
            var v = new NavigatorBarModel("~/Admin/Menus");
            return View(v);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public ActionResult Logs()
        {
            var v = new NavigatorBarModel("~/Admin/Logs");
            return View(v);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [AllowAnonymous]
        public ActionResult FAIcon()
        {
            return View();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public ActionResult Profiles()
        {
            var v = new NavigatorBarModel("~/Admin/Profiles");
            return View(v);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public ActionResult News()
        {
            var v = new NavigatorBarModel("~/Admin/News");
            return View(v);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public ActionResult Infos()
        {
            var v = new NavigatorBarModel("~/Admin/Infos");
            return View(v);
        }
    }
}

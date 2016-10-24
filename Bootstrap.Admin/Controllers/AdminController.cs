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
            var v = new NavigatorBarModel();
            v.ShowMenu = "hide";
            v.HomeUrl = "~/Admin";
            return View(v);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public ActionResult Users()
        {
            var v = new NavigatorBarModel();
            v.BreadcrumbName = "用户管理";
            v.ShowMenu = "hide";
            v.Menus[1].Active = "active";
            v.HomeUrl = "~/Admin";
            return View(v);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public ActionResult Groups()
        {
            var v = new NavigatorBarModel();
            v.BreadcrumbName = "部门管理";
            v.ShowMenu = "hide";
            v.Menus[3].Active = "active";
            v.HomeUrl = "~/Admin";
            return View(v);
        }
        public ActionResult Roles()
        {
            var v = new NavigatorBarModel();
            v.BreadcrumbName = "角色管理";
            v.ShowMenu = "hide";
            v.Menus[2].Active = "active";
            v.HomeUrl = "~/Admin";
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
    }
}

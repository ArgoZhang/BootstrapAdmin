using Bootstrap.Admin.Models;
using Bootstrap.DataAccess;
using System.Web.Mvc;

namespace Bootstrap.Admin.Controllers
{
    public class AdminController : Controller
    {
        // GET: Admin
        public ActionResult Index()
        {
            var v = new ModelBase();
            v.Header = new HeaderBarModel();
            v.Header.UserName = "Argo Zhang";
            v.Navigator = new NavigatorBarModel();
            return View(v);
        }

        public ActionResult Users()
        {
            var v = new UserModel();
            v.Header = new HeaderBarModel();
            v.Header.UserName = "Argo Zhang";
            v.Header.BreadcrumbName = "用户管理";
            v.Navigator = new NavigatorBarModel();
            return View(v);
        }
    }
}

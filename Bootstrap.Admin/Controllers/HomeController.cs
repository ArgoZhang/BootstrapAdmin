using System.Web.Mvc;

namespace Bootstrap.Admin.Controllers
{
    public class HomeController : Controller
    {
        // GET: Home
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Rules()
        {
            return View();
        }
        public ActionResult Terminals()
        {
            return View();
        }
        public ActionResult Login()
        {
            return View();
        }
    }
}
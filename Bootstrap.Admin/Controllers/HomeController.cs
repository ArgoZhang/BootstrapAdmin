using Bootstrap.Admin.Models;
using Bootstrap.DataAccess;
using Microsoft.AspNetCore.Mvc;

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
            var v = new HeaderBarModel(User.Identity) { HomeUrl = DictHelper.RetrieveHomeUrl() };
            return v.HomeUrl.StartsWith("~/") ? (ActionResult)View(v) : Redirect(v.HomeUrl);
        }
    }
}
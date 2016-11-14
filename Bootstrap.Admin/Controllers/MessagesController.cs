using Bootstrap.Admin.Models;
using Bootstrap.DataAccess;
using System.Web.Mvc;

namespace Bootstrap.Admin.Controllers
{
    public class MessagesController : Controller
    {
        public ActionResult Inbox()
        {       
            var v = new NavigatorBarModel("~/Admin/Messages/Inbox");
            v.MessageList = MessageHelper.Inbox(User.Identity.Name);
            return View(v);
        }
        public ActionResult SendMail()
        {
            var v = new NavigatorBarModel("~/Admin/Messages/SendMail");
            v.MessageList = MessageHelper.SendMail(User.Identity.Name);
            return View(v);
        }
        public ActionResult Mark()
        {
            var v = new NavigatorBarModel("~/Admin/Messages/Mark");
            v.MessageList = MessageHelper.Mark(User.Identity.Name);
            return View(v);
        }
        public ActionResult Trash()
        {
            var v = new NavigatorBarModel("~/Admin/Messages/Trash");
            v.MessageList = MessageHelper.Trash(User.Identity.Name);
            return View(v);
        }

    }
}
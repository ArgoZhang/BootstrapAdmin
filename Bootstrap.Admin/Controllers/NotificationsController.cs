using Bootstrap.DataAccess;
using System.Collections.Generic;
using System.Web.Http;
using System.Linq;

namespace Bootstrap.Admin.Controllers
{
    public class NotificationsController : ApiController
    {
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public Notifications Get()
        {
            var ret = new Notifications();
            NotificationHelper.RetrieveNotifications().AsParallel().ForAll(n =>
            {
                if (n.Category == "0") ret.Users.Add(n);
                else if (n.Category == "1") ret.Apps.Add(n);
                else if (n.Category == "2") ret.Dbs.Add(n);
            });
            return ret;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        [HttpGet]
        public Notifications Get(string id)
        {
            var ret = new Notifications();
            NotificationHelper.RetrieveNotifications().AsParallel().ForAll(n =>
            {
                if (id != n.Category) return;
                if (n.Category == "0") ret.Users.Add(n);
                else if (n.Category == "1") ret.Apps.Add(n);
                else if (n.Category == "2") ret.Dbs.Add(n);
            });
            return ret;
        }

        public class Notifications
        {
            public Notifications()
            {
                Users = new List<Notification>();
                Apps = new List<Notification>();
                Dbs = new List<Notification>();
            }
            public List<Notification> Users { get; set; }

            public List<Notification> Apps { get; set; }

            public List<Notification> Dbs { get; set; }
        }
    }
}
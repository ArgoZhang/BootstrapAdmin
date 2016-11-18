using Bootstrap.DataAccess;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;

namespace Bootstrap.Admin.Controllers
{
    public class MessagesController : ApiController
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        [HttpGet]
        public IEnumerable<Message> Get(string id)
        {
            var ret = new List<Message>();
            if (id == "inbox") ret = MessageHelper.Inbox(User.Identity.Name).ToList();
            return ret;
        }
    }
}
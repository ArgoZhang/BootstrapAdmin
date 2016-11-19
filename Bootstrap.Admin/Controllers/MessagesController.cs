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
            switch(id)
            { 
                case "inbox": ret = MessageHelper.Inbox(User.Identity.Name).ToList();
                    break;
                case "sendmail": ret = MessageHelper.SendMail(User.Identity.Name).ToList();
                    break;
                case "mark": ret = MessageHelper.Mark(User.Identity.Name).ToList();
                    break;
                case "trash": ret = MessageHelper.Trash(User.Identity.Name).ToList();
                    break;
             }
            return ret;
        }
    }
}
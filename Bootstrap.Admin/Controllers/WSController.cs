using Bootstrap.DataAccess;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;

namespace Bootstrap.Admin.Controllers
{
    public class WSController : ApiController
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        [HttpGet]
        public List<MessageBody> Get()
        {
            return NotificationHelper.MessagePool.ToList();
        }
    }
}
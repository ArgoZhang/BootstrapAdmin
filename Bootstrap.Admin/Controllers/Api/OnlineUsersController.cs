using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace Bootstrap.Admin.Controllers.Api
{
    /// <summary>
    /// 
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class OnlineUsersController : ControllerBase
    {
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [HttpPost()]
        public IEnumerable<OnlineUser> Post([FromServices]IOnlineUsers onlineUSers)
        {
            return onlineUSers.OnlineUsers;
        }
    }
}

using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Bootstrap.Admin.Controllers.Api
{
    /// <summary>
    /// 在线用户接口
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class OnlineUsersController : ControllerBase
    {
        /// <summary>
        /// 获取所有在线用户数据
        /// </summary>
        /// <returns></returns>
        [HttpPost()]
        public IEnumerable<OnlineUser> Post([FromServices]IOnlineUsers onlineUSers)
        {
            return onlineUSers.OnlineUsers;
        }

        /// <summary>
        /// 获取指定IP地址的在线用户请求地址明细数据
        /// </summary>
        /// <param name="id"></param>
        /// <param name="onlineUSers"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public IEnumerable<KeyValuePair<DateTime, string>> Get(string id, [FromServices]IOnlineUsers onlineUSers)
        {
            var user = onlineUSers.OnlineUsers.FirstOrDefault(u => u.Ip == id);
            return user?.RequestUrls ?? new KeyValuePair<DateTime, string>[0];
        }
    }
}

using Bootstrap.Admin.Query;
using Bootstrap.DataAccess;
using Longbow.Web;
using Longbow.Web.Mvc;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace Bootstrap.Admin.Controllers.Api
{
    /// <summary>
    /// 
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class LogsController : ControllerBase
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        [HttpGet]
        public QueryData<Log> Get([FromQuery]QueryLogOption value)
        {
            return value.RetrieveData();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="onlineUserSvr"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        [HttpPost]
        public bool Post([FromServices]IOnlineUsers onlineUserSvr, [FromBody]Log value)
        {
            var agent = new UserAgent(Request.Headers["User-Agent"]);
            value.Ip = HttpContext.Connection.RemoteIpAddress?.ToString();
            value.Browser = $"{agent.Browser.Name} {agent.Browser.Version}";
            value.OS = $"{agent.OS.Name} {agent.OS.Version}";
            value.City = onlineUserSvr.RetrieveLocaleByIp(value.Ip);
            value.UserName = User.Identity.Name;
            if (string.IsNullOrEmpty(value.Ip)) value.Ip = IPAddress.IPv6Loopback.ToString();
            return LogHelper.Save(value);
        }
    }
}
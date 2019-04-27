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
        /// <param name="ipLocator"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        [HttpPost]
        public bool Post([FromServices]IOnlineUsers onlineUserSvr, [FromServices]IIPLocatorProvider ipLocator, [FromBody]Log value)
        {
            value.UserAgent = Request.Headers["User-Agent"];
            var agent = new UserAgent(value.UserAgent);
            value.Ip = (HttpContext.Connection.RemoteIpAddress ?? IPAddress.IPv6Loopback).ToString();
            value.Browser = $"{agent.Browser?.Name} {agent.Browser?.Version}";
            value.OS = $"{agent.OS?.Name} {agent.OS?.Version}";
            value.City = ipLocator.Locate(value.Ip);
            value.UserName = User.Identity.Name;
            return LogHelper.Save(value);
        }
    }
}
using Bootstrap.Admin.Query;
using Bootstrap.DataAccess;
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
        /// <param name="value"></param>
        /// <returns></returns>
        [HttpPost]
        public bool Post([FromBody]Log value)
        {
            value.ClientAgent = Request.Headers["User-Agent"];
            value.ClientIp = (HttpContext.Connection.RemoteIpAddress ?? IPAddress.IPv6Loopback).ToString();
            value.UserName = User.Identity.Name;
            return LogHelper.Save(value);
        }
    }
}
using Bootstrap.Admin.Query;
using Bootstrap.DataAccess;
using Longbow.Web;
using Longbow.Web.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace Bootstrap.Admin.Controllers.Api
{
    /// <summary>
    /// 操作日志控制器
    /// </summary>
    [Route("api/[controller]")]
    [Authorize]
    [ApiController]
    public class LogsController : ControllerBase
    {
        /// <summary>
        /// 前台获取操作日志数据调用
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        [HttpGet]
        public QueryData<Log> Get([FromQuery]QueryLogOption value)
        {
            return value.RetrieveData();
        }

        /// <summary>
        /// 操作日志记录方法
        /// </summary>
        /// <param name="ipLocator"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        [HttpPost]
        public bool Post([FromServices]IIPLocatorProvider ipLocator, [FromBody]Log value)
        {
            value.UserAgent = Request.Headers["User-Agent"];
            var agent = new UserAgent(value.UserAgent);
            value.Ip = HttpContext.Connection.RemoteIpAddress?.ToIPv4String() ?? "";
            value.Browser = $"{agent.Browser?.Name} {agent.Browser?.Version}";
            value.OS = $"{agent.OS?.Name} {agent.OS?.Version}";
            value.City = ipLocator.Locate(value.Ip);
            value.UserName = User.Identity?.Name ?? string.Empty;
            return LogHelper.Save(value);
        }
    }
}

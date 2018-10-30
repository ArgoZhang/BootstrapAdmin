using Bootstrap.Admin.Query;
using Bootstrap.DataAccess;
using Longbow.Web.Mvc;
using Microsoft.AspNetCore.Mvc;
using System.Linq;

namespace Bootstrap.Admin.Controllers.Api
{
    /// <summary>
    /// 
    /// </summary>
    [Route("api/[controller]")]
    public class LogsController : Controller
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        [HttpGet]
        public QueryData<Log> Get(QueryLogOption value)
        {
            return value.RetrieveData();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public Log Get(string id)
        {
            return LogHelper.RetrieveLogs().FirstOrDefault(t => t.Id == id);
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
            value.ClientIp = HttpContext.Connection.RemoteIpAddress.ToString();
            value.UserName = User.Identity.Name;
            return LogHelper.SaveLog(value);
        }
    }
}
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
        public Log Get(int id)
        {
            return LogHelper.RetrieveLogs().FirstOrDefault(t => t.Id == id);
        }

        [HttpPost]
        public bool Post(Log value)
        {
            var request = Request;
            //value.ClientAgent = request.UserAgent;
            //value.ClientIp = request.UserHostAddress;
            value.UserName = User.Identity.Name;
            return LogHelper.SaveLog(value);
        }
    }
}
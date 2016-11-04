using Bootstrap.Admin.Models;
using Bootstrap.DataAccess;
using System.Linq;
using System.Web.Http;

namespace Bootstrap.Admin.Controllers
{
    public class LogsController : ApiController
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        [HttpGet]
        public QueryData<Log> Get([FromUri]QueryLogOption value)
        {
            return value.RetrieveData();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        public Log Get(int id)
        {
            return LogHelper.RetrieveLogs().FirstOrDefault(t => t.ID == id);
        }

        [HttpPost]
        public bool Post([FromBody]Log value)
        {

            value.OperationIp = LogHelper.GetClientIp();
            value.OperationTime = System.DateTime.Now;
            return LogHelper.SaveLog(value);
        }
    }
}
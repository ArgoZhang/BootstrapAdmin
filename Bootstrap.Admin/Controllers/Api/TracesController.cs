using Bootstrap.Admin.Query;
using Bootstrap.DataAccess;
using Longbow.Web.Mvc;
using Microsoft.AspNetCore.Mvc;

namespace Bootstrap.Admin.Controllers.Api
{
    /// <summary>
    /// 
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class TracesController : ControllerBase
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        [HttpGet]
        public QueryData<Trace> Get([FromQuery]QueryTraceOptions value)
        {
            return value.RetrieveData();
        }
    }
}

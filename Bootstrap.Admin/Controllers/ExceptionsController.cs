using Bootstrap.Admin.Models;
using Bootstrap.DataAccess;
using System.Web.Http;

namespace Bootstrap.Admin.Controllers
{
    public class ExceptionsController : ApiController
    {
        /// <summary>
        /// 显示所有异常
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        public QueryData<Exceptions> Get([FromUri]QueryExceptionOption value)
        {
            return value.RetrieveData();
        }
    }
}

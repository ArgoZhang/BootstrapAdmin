using Bootstrap.Admin.Models;
using Bootstrap.DataAccess;
using System.Linq;
using System.Web.Http;

namespace Bootstrap.Admin.Controllers
{
    public class DictsController : ApiController
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        [HttpGet]
        public QueryData<Dict> Get([FromUri]QueryDictOption value)
        {
            return value.RetrieveData();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        public Dict Get(int id)
        {
            return DictHelper.RetrieveDicts().FirstOrDefault(t => t.ID == id);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        [HttpPost]
        public bool Post([FromBody]Dict value)
        {
            return DictHelper.SaveDict(value);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        [HttpDelete]
        public bool Delete([FromBody]string value)
        {
            return DictHelper.DeleteDict(value);
        }
    }
}

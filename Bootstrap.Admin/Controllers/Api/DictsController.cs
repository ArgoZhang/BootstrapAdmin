using Bootstrap.Admin.Query;
using Bootstrap.DataAccess;
using Bootstrap.Security;
using Longbow.Web.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Bootstrap.Admin.Controllers.Api
{
    /// <summary>
    /// 
    /// </summary>
    [Route("api/[controller]")]
    public class DictsController : Controller
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        [HttpGet]
        public QueryData<BootstrapDict> Get(QueryDictOption value)
        {
            return value.RetrieveData();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        [HttpPost]
        public bool Post([FromBody]BootstrapDict value)
        {
            return DictHelper.SaveDict(value);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        [HttpDelete]
        [Authorize(Roles = "Administrators")]
        public object Delete(string value)
        {
            var result = DictHelper.DeleteDict(value);
            return new { result, msg = result ? "成功！" : "失败" };
        }
    }
}
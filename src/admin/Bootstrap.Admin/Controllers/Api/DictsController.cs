using Bootstrap.Admin.Query;
using Bootstrap.DataAccess;
using Bootstrap.Security;
using Longbow.Web.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Collections.Generic;

namespace Bootstrap.Admin.Controllers.Api
{
    /// <summary>
    /// 字典表维护控制器
    /// </summary>
    [Route("api/[controller]")]
    [Authorize]
    [ApiController]
    public class DictsController : ControllerBase
    {
        /// <summary>
        /// 获取所有字典表数据方法
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        [HttpGet]
        public QueryData<BootstrapDict> Get([FromQuery]QueryDictOption value)
        {
            return value.RetrieveData();
        }
        /// <summary>
        /// 保存字典方法
        /// </summary>
        /// <param name="value"></param>
        [HttpPost]
        [ButtonAuthorize(Url = "~/Admin/Dicts", Auth = "add,edit")]
        public bool Post([FromBody]BootstrapDict value)
        {
            return DictHelper.Save(value);
        }
        /// <summary>
        /// 删除字典项方法
        /// </summary>
        /// <param name="value"></param>
        [HttpDelete]
        [Authorize(Roles = "Administrators")]
        [ButtonAuthorize(Url = "~/Admin/Dicts", Auth = "del")]
        public bool Delete([FromBody]IEnumerable<string> value)
        {
            return DictHelper.Delete(value);
        }
    }
}

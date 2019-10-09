using Bootstrap.Admin.Query;
using Bootstrap.DataAccess;
using Bootstrap.Security;
using Longbow.Web.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace Bootstrap.Admin.Controllers.Api
{
    /// <summary>
    /// 
    /// </summary>
    [Route("api/[controller]")]
    [Authorize]
    [ApiController]
    public class DictsController : ControllerBase
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        [HttpGet]
        public QueryData<BootstrapDict> Get([FromQuery]QueryDictOption value)
        {
            return value.RetrieveData();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        [HttpPost]
        [ButtonAuthorize(Url = "~/Admin/Dicts", Auth = "add,edit")]
        public bool Post([FromBody]BootstrapDict value)
        {
            return DictHelper.Save(value);
        }
        /// <summary>
        /// 
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

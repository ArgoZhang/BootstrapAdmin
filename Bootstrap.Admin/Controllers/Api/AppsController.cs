using Bootstrap.DataAccess;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace Bootstrap.Admin.Controllers.Api
{
    /// <summary>
    /// 
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class AppsController : ControllerBase
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost("{id}")]
        public IEnumerable<App> Post(string id) => AppHelper.RetrievesByRoleId(id);

        /// <summary>
        /// 保存应用程序授权
        /// </summary>
        /// <param name="id"></param>
        /// <param name="appIds"></param>
        /// <returns></returns>
        [HttpPut("{id}")]
        public bool Put(string id, [FromBody]IEnumerable<string> appIds) => AppHelper.SaveByRoleId(id, appIds);
    }
}

using Bootstrap.DataAccess;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace Bootstrap.Admin.Controllers.Api
{
    /// <summary>
    /// 应用程序控制器
    /// </summary>
    [Route("api/[controller]")]
    [Authorize]
    [ApiController]
    public class AppsController : ControllerBase
    {
        /// <summary>
        /// 通过角色ID获取其授权的所有应用程序集合
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public IEnumerable<DataAccess.App> Get(string id) => AppHelper.RetrievesByRoleId(id);
    }
}

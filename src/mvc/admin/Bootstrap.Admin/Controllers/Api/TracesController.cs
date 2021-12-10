using Bootstrap.Admin.Query;
using Bootstrap.DataAccess;
using Longbow.Web;
using Longbow.Web.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Bootstrap.Admin.Controllers.Api
{
    /// <summary>
    /// 在线用户跟踪控制器
    /// </summary>
    [Route("api/[controller]")]
    [Authorize]
    [ApiController]
    public class TracesController : ControllerBase
    {
        /// <summary>
        /// 客户端脚本获取在线用户数据
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        [HttpGet]
        public QueryData<Trace> Get([FromQuery]QueryTraceOptions value)
        {
            return value.RetrieveData();
        }

        /// <summary>
        /// 在线用户访问保存方法，前台系统调用
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        [HttpPost]
        public bool Post([FromBody]OnlineUser user)
        {
            TraceHelper.Save(HttpContext, user);
            return true;
        }
    }
}

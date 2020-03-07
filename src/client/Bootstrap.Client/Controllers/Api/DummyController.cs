using Bootstrap.Client.DataAccess;
using Bootstrap.Client.Query;
using Longbow.Web.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace Bootstrap.Admin.Controllers.Api
{
    /// <summary>
    /// Dummy 表维护控制器
    /// </summary>
    [Route("api/[controller]")]
    [Authorize]
    [ApiController]
    public class DummyController : ControllerBase
    {
        /// <summary>
        /// 获取所有 Dummy 表数据方法
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        [HttpGet]
        public QueryData<Dummy> Get([FromQuery]QueryDummyOption value) => value.Retrieves();

        /// <summary>
        /// 保存方法
        /// </summary>
        /// <param name="value"></param>
        [HttpPost]
        public bool Post([FromBody]Dummy value) => DummyHelper.Save(value);

        /// <summary>
        /// 删除指定 ID 集合的 Dummy 数据
        /// </summary>
        /// <param name="value"></param>
        [HttpDelete]
        public bool Delete([FromBody]IEnumerable<string> value) => DummyHelper.Delete(value);
    }
}

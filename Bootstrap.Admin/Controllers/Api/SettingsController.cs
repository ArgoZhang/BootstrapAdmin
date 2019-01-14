using Bootstrap.DataAccess;
using Bootstrap.Security;
using Longbow.Cache;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace Bootstrap.Admin.Controllers.Api
{
    /// <summary>
    /// 
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class SettingsController : ControllerBase
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        [HttpPost]
        public bool Post([FromBody]BootstrapDict value)
        {
            //保存个性化设置
            return DictHelper.SaveSettings(value);
        }
        /// <summary>
        /// 
        /// </summary>
        [HttpGet]
        public IEnumerable<ICacheCorsItem> Get()
        {
            return CacheManager.CorsSites;
        }
    }
}
using Bootstrap.DataAccess;
using Longbow.Cache;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.Linq;

namespace Bootstrap.Admin.Controllers.Api
{
    /// <summary>
    /// 
    /// </summary>
    [Route("api/[controller]")]
    public class SettingsController : Controller
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        [HttpPost]
        public bool Post(JObject value)
        {
            //保存个性化设置
            dynamic json = value;
            return DictHelper.SaveSettings((string)json.name, (string)json.code, (string)json.category);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        [HttpGet]
        public IEnumerable<ICacheCorsItem> Get()
        {
            return CacheManager.CorsSites;
        }
    }
}
using Bootstrap.DataAccess;
using Longbow.Caching.Configuration;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;

namespace Bootstrap.Admin.Controllers
{
    /// <summary>
    /// 
    /// </summary>
    public class SettingsController : ApiController
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        [HttpPost]
        public bool Post([FromBody]JObject value)
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
        public IEnumerable<CacheListElement> Get([FromUri]JObject value)
        {
            var section = CacheListSection.GetSection();
            return section.Items.Where(item => item.Enabled);
        }
    }
}
using Bootstrap.Admin.Models;
using Bootstrap.DataAccess;
using Longbow.Security.Principal;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;

namespace Bootstrap.Admin.Controllers
{
    public class DictsController : ApiController
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        [HttpGet]
        public QueryData<Dict> Get([FromUri]QueryDictOption value)
        {
            return value.RetrieveData();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        public Dict Get(int id)
        {
            return DictHelper.RetrieveDicts().FirstOrDefault(t => t.ID == id);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        [HttpPost]
        public bool Post([FromBody]Dict value)
        {
            return DictHelper.SaveDict(value);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public IEnumerable<string> Post(int id, [FromBody]JObject value)
        {
            var ret = new List<string>();
            dynamic json = value;
            switch ((string)json.type)
            {
                case "category":
                    ret = DictHelper.RetrieveCategories().ToList();
                    break;
                default:
                    break;
            }
            return ret;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        [HttpDelete]
        public dynamic Delete([FromBody]string value)
        {
            if (!LgbPrincipal.IsAdmin(User.Identity.Name) && !User.IsInRole("Administrators")) return new { result = false, msg = "当前用户权限不够" };
            var result = DictHelper.DeleteDict(value);
            return new { result = result, msg = result ? "成功！" : "失败" };
        }
    }
}
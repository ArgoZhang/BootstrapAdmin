using Bootstrap.Admin.Models;
using Bootstrap.DataAccess;
using Bootstrap.Security;
using Longbow.Security.Principal;
using Longbow.Web.Mvc;
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
        public QueryData<BootstrapDict> Get([FromUri]QueryDictOption value)
        {
            return value.RetrieveData();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        public BootstrapDict Get(int id)
        {
            return DictHelper.RetrieveDicts().FirstOrDefault(t => t.Id == id);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        [HttpPost]
        public bool Post([FromBody]BootstrapDict value)
        {
            return DictHelper.SaveDict(value);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [AllowAnonymous]
        public IEnumerable<BootstrapDict> Post(int id, [FromBody]JObject value)
        {
            IEnumerable<BootstrapDict> ret = new List<BootstrapDict>();
            dynamic json = value;
            switch ((string)json.type)
            {
                case "category":
                    ret = DictHelper.RetrieveCategories();
                    break;
                case "css":
                    ret = DictHelper.RetrieveWebCss();
                    break;
                case "activeCss":
                    ret = DictHelper.RetrieveActiveCss();
                    break;
                default:
                    break;
            }
            return ret;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        [HttpDelete]
        public object Delete([FromBody]string value)
        {
            if (!LgbPrincipal.IsAdmin(User)) return new { result = false, msg = "当前用户权限不够" };
            var result = DictHelper.DeleteDict(value);
            return new { result = result, msg = result ? "成功！" : "失败" };
        }
    }
}
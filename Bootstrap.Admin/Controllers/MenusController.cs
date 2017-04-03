using Bootstrap.Admin.Models;
using Bootstrap.DataAccess;
using Bootstrap.Security;
using Longbow.Web.Mvc;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;

namespace Bootstrap.Admin.Controllers
{
    public class MenusController : ApiController
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        [HttpGet]
        public QueryData<BootstrapMenu> Get([FromUri]QueryMenuOption value)
        {
            return value.RetrieveData(User.Identity.Name);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        [HttpPost]
        public bool Post([FromBody]BootstrapMenu value)
        {
            return MenuHelper.SaveMenu(value);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        [HttpDelete]
        public bool Delete([FromBody]string value)
        {
            return MenuHelper.DeleteMenu(value);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        [HttpPost]
        public IEnumerable<BootstrapMenu> Post(int id, [FromBody]JObject value)
        {
            var ret = new List<BootstrapMenu>();
            dynamic json = value;
            switch ((string)json.type)
            {
                case "role":
                    ret = MenuHelper.RetrieveMenusByRoleId(id).ToList();
                    break;
                case "user":
                    ret = BootstrapMenu.RetrieveAllMenus(User.Identity.Name).ToList();
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
        /// <param name="value"></param>
        /// <returns></returns>
        [HttpPut]
        public bool Put(int id, [FromBody]JObject value)
        {
            var ret = false;
            dynamic json = value;
            string menuIds = json.menuIds.ToString();
            switch ((string)json.type)
            {
                case "role":
                    ret = MenuHelper.SaveMenusByRoleId(id, menuIds);
                    break;
                default:
                    break;
            }
            return ret;
        }
    }
}
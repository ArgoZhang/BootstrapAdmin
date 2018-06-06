using Bootstrap.Admin.Query;
using Bootstrap.DataAccess;
using Bootstrap.Security;
using Longbow.Web.Mvc;
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
    public class MenusController : Controller
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        [HttpGet]
        public QueryData<BootstrapMenu> Get(QueryMenuOption value)
        {
            return value.RetrieveData(User.Identity.Name);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        [HttpPost]
        public bool Post(BootstrapMenu value)
        {
            return MenuHelper.SaveMenu(value);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        [HttpDelete]
        public bool Delete(string value)
        {
            return MenuHelper.DeleteMenu(value);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        [HttpPost("{id}")]
        public IEnumerable<BootstrapMenu> Post(int id, JObject value)
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
        [HttpPut("{id}")]
        public bool Put(int id, JObject value)
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
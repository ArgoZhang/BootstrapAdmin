using Bootstrap.Admin.Query;
using Bootstrap.DataAccess;
using Bootstrap.Security;
using Longbow.Web.Mvc;
using Microsoft.AspNetCore.Mvc;
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
        /// 获得所有菜单列表调用
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        [HttpGet]
        public QueryData<BootstrapMenu> Get(QueryMenuOption value)
        {
            return value.RetrieveData(User.Identity.Name);
        }
        /// <summary>
        /// 保存菜单调用
        /// </summary>
        /// <param name="value"></param>
        [HttpPost]
        public bool Post([FromBody]BootstrapMenu value)
        {
            return MenuHelper.SaveMenu(value);
        }
        /// <summary>
        /// 删除菜单调用
        /// </summary>
        /// <param name="value"></param>
        [HttpDelete]
        public bool Delete([FromBody]IEnumerable<string> value)
        {
            return MenuHelper.DeleteMenu(value);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        [HttpPost("{id}")]
        public IEnumerable<BootstrapMenu> Post(string id, [FromQuery]string type)
        {
            var ret = new List<BootstrapMenu>();
            switch (type)
            {
                case "role":
                    ret = MenuHelper.RetrieveMenusByRoleId(id).ToList();
                    break;
                case "user":
                    ret = MenuHelper.RetrieveMenus(User.Identity.Name).ToList();
                    break;
                default:
                    break;
            }
            return ret;
        }
        /// <summary>
        /// 角色管理页面分配菜单时调用
        /// </summary>
        /// <param name="id">角色ID</param>
        /// <param name="value">菜单ID集合</param>
        /// <returns></returns>
        [HttpPut("{id}")]
        public bool Put(string id, [FromBody]IEnumerable<string> value)
        {
            return MenuHelper.SaveMenusByRoleId(id, value);
        }
    }
}
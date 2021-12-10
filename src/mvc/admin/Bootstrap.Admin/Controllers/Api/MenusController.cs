using Bootstrap.Admin.Query;
using Bootstrap.DataAccess;
using Bootstrap.Security;
using Longbow.Web.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Collections.Generic;

namespace Bootstrap.Admin.Controllers.Api
{
    /// <summary>
    /// 
    /// </summary>
    [Route("api/[controller]")]
    [Authorize]
    [ApiController]
    public class MenusController : ControllerBase
    {
        /// <summary>
        /// 获得所有菜单列表调用
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        [HttpGet]
        public QueryData<object> Get([FromQuery]QueryMenuOption value)
        {
            return value.RetrieveData(User.Identity!.Name);
        }

        /// <summary>
        /// 保存菜单调用
        /// </summary>
        /// <param name="value"></param>
        [HttpPost]
        [ButtonAuthorize(Url = "~/Admin/Menus", Auth = "add,edit")]
        public bool Post([FromBody]BootstrapMenu value)
        {
            return MenuHelper.Save(value);
        }

        /// <summary>
        /// 删除菜单调用
        /// </summary>
        /// <param name="value"></param>
        [HttpDelete]
        [ButtonAuthorize(Url = "~/Admin/Menus", Auth = "del")]
        public bool Delete([FromBody]IEnumerable<string> value)
        {
            return MenuHelper.Delete(value);
        }

        /// <summary>
        /// 角色管理菜单授权按钮调用
        /// </summary>
        /// <param name="id">角色ID</param>
        /// <param name="type">type=role时，角色管理菜单授权调用；type=user时，菜单管理编辑页面父类菜单按钮调用</param>
        /// <returns></returns>
        [HttpPost("{id}")]
        public IEnumerable<object> Post(string id, [FromQuery]string type)
        {
            IEnumerable<object> ret = new List<object>();
            switch (type)
            {
                case "role":
                    ret = MenuHelper.RetrieveMenusByRoleId(id);
                    break;
                case "user":
                    ret = MenuHelper.RetrieveMenus(User.Identity!.Name);
                    break;
            }
            return ret;
        }

        /// <summary>
        /// 角色管理菜单授权保存按钮调用
        /// </summary>
        /// <param name="id">菜单ID</param>
        /// <param name="roleIds">角色ID集合</param>
        /// <returns></returns>
        [HttpPut("{id}")]
        [ButtonAuthorize(Url = "~/Admin/Menus", Auth = "assignRole")]
        public bool Put(string id, [FromBody]IEnumerable<string> roleIds)
        {
            return RoleHelper.SavaByMenuId(id, roleIds);
        }
    }
}

using Bootstrap.Admin.Query;
using Bootstrap.DataAccess;
using Longbow.Web.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Collections.Generic;
using System.Linq;


namespace Bootstrap.Admin.Controllers.Api
{
    /// <summary>
    /// 角色维护控制器
    /// </summary>
    [Route("api/[controller]")]
    [Authorize]
    [ApiController]
    public class RolesController : ControllerBase
    {
        /// <summary>
        /// 获取所有角色数据
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        [HttpGet]
        public QueryData<object> Get([FromQuery]QueryRoleOption value)
        {
            return value.RetrieveData();
        }
        /// <summary>
        /// 通过指定用户ID/部门ID/菜单ID获得所有角色集合，已经授权的有checked标记
        /// </summary>
        /// <param name="id">用户ID/部门ID/菜单ID</param>
        /// <param name="type">类型</param>
        /// <returns></returns>
        [HttpPost("{id}")]
        public IEnumerable<object> Post(string id, [FromQuery]string type)
        {
            IEnumerable<Role> ret = new Role[0];
            switch (type)
            {
                case "user":
                    ret = RoleHelper.RetrievesByUserId(id);
                    break;
                case "group":
                    ret = RoleHelper.RetrievesByGroupId(id);
                    break;
                case "menu":
                    ret = RoleHelper.RetrievesByMenuId(id);
                    break;
            }
            return ret.Select(m => new { m.Id, m.Checked, m.RoleName, m.Description });
        }
        /// <summary>
        /// 保存角色授权方法
        /// </summary>
        /// <param name="id">角色ID</param>
        /// <param name="values">选中的ID集合</param>
        /// <param name="type">type=menu时，菜单维护页面对角色授权弹框保存按钮调用</param>
        /// <returns></returns>
        [HttpPut("{id}")]
        [ButtonAuthorize(Url = "~/Admin/Roles", Auth = "assignUser,assignGroup,assignMenu,assignApp")]
        public bool Put(string id, [FromBody]IEnumerable<string> values, [FromQuery]string type)
        {
            var ret = false;
            switch (type)
            {
                case "user":
                    ret = UserHelper.SaveByRoleId(id, values);
                    break;
                case "group":
                    ret = GroupHelper.SaveByRoleId(id, values);
                    break;
                case "menu":
                    ret = MenuHelper.SaveMenusByRoleId(id, values);
                    break;
                case "app":
                    ret = AppHelper.SaveByRoleId(id, values);
                    break;
            }
            return ret;
        }
        /// <summary>
        /// 保存角色方法
        /// </summary>
        /// <param name="value"></param>
        [HttpPost]
        [ButtonAuthorize(Url = "~/Admin/Roles", Auth = "add,edit")]
        public bool Post([FromBody]Role value)
        {
            return RoleHelper.Save(value);
        }
        /// <summary>
        /// 删除角色方法
        /// </summary>
        /// <param name="value"></param>
        [HttpDelete]
        [ButtonAuthorize(Url = "~/Admin/Roles", Auth = "del")]
        public bool Delete([FromBody]IEnumerable<string> value)
        {
            return RoleHelper.Delete(value);
        }
    }
}

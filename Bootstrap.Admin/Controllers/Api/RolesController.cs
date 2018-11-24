using Bootstrap.Admin.Query;
using Bootstrap.DataAccess;
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
    [ApiController]
    public class RolesController : ControllerBase
    {
        /// <summary>
        /// 
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
            IEnumerable<Role> ret = new List<Role>();
            switch (type)
            {
                case "user":
                    ret = RoleHelper.RetrieveRolesByUserId(id);
                    break;
                case "group":
                    ret = RoleHelper.RetrieveRolesByGroupId(id);
                    break;
                case "menu":
                    ret = RoleHelper.RetrieveRolesByMenuId(id);
                    break;
                default:
                    break;
            }
            return ret.Select(m => new { m.Id, m.Checked, m.RoleName, m.Description });
        }
        /// <summary>
        /// 保存角色
        /// </summary>
        /// <param name="id">用户ID/部门ID/菜单ID</param>
        /// <param name="roleIds">选中的角色ID集合</param>
        /// <param name="type">type=menu时，菜单维护页面对角色授权弹框保存按钮调用</param>
        /// <returns></returns>
        [HttpPut("{id}")]
        public bool Put(string id, [FromBody]IEnumerable<string> roleIds, [FromQuery]string type)
        {
            var ret = false;
            switch (type)
            {
                case "user":
                    ret = RoleHelper.SaveRolesByUserId(id, roleIds);
                    break;
                case "group":
                    ret = RoleHelper.SaveRolesByGroupId(id, roleIds);
                    break;
                case "menu":
                    ret = RoleHelper.SavaRolesByMenuId(id, roleIds);
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
        [HttpPost]
        public bool Post([FromBody]Role value)
        {
            return RoleHelper.SaveRole(value);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        [HttpDelete]
        public bool Delete([FromBody]IEnumerable<string> value)
        {
            return RoleHelper.DeleteRole(value);
        }
    }
}

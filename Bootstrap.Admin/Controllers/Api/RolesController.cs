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
    public class RolesController : Controller
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        [HttpGet]
        public QueryData<Role> Get(QueryRoleOption value)
        {
            return value.RetrieveData();
        }
        /// <summary>
        /// 通过指定ID获得所有角色集合
        /// </summary>
        /// <param name="id">用户ID/部门ID/菜单ID</param>
        /// <param name="type">类型</param>
        /// <returns></returns>
        [HttpPost("{id}")]
        public IEnumerable<Role> Post(int id, [FromQuery]string type)
        {
            var ret = new List<Role>();
            switch (type)
            {
                case "user":
                    ret = RoleHelper.RetrieveRolesByUserId(id).ToList();
                    break;
                case "group":
                    ret = RoleHelper.RetrieveRolesByGroupId(id).ToList();
                    break;
                case "menu":
                    ret = RoleHelper.RetrieveRolesByMenuId(id).ToList();
                    break;
                default:
                    break;
            }
            return ret;
        }
        /// <summary>
        /// 根据GroupID获取
        /// </summary>
        /// <param name="id"></param>
        /// <param name="roleIds"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        [HttpPut("{id}")]
        public bool Put(int id, [FromBody]IEnumerable<int> roleIds, [FromQuery]string type)
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
        public bool Delete([FromBody]IEnumerable<int> value)
        {
            return RoleHelper.DeleteRole(value);
        }
    }
}

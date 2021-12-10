using Bootstrap.Admin.Query;
using Bootstrap.DataAccess;
using Longbow.Web.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Bootstrap.Admin.Controllers.Api
{
    /// <summary>
    /// 用户控制器
    /// </summary>
    [Route("api/[controller]")]
    [Authorize]
    [ApiController]
    public class UsersController : ControllerBase
    {
        /// <summary>
        /// 调用获取所有用户信息 用户管理查询按钮
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        [HttpGet]
        public QueryData<object> Get([FromQuery]QueryUserOption value)
        {
            return value.RetrieveData();
        }

        /// <summary>
        /// 用户相关授权操作
        /// </summary>
        /// <param name="id">主键</param>
        /// <param name="type">类型 如角色、部门</param>
        /// <returns></returns>
        [HttpPost("{id}")]
        public IEnumerable<object> Post(string id, [FromQuery]string type) => type switch
        {
            "role" => UserHelper.RetrievesByRoleId(id).Select(p => new
            {
                p.Id,
                p.DisplayName,
                p.UserName,
                p.Checked
            }).OrderBy(u => u.DisplayName),
            "group" => UserHelper.RetrievesByGroupId(id),
            "reset" => UserHelper.RetrieveResetReasonsByUserName(id).Select(u => new { u.Key, u.Value }),
            _ => new string[0]
        };

        /// <summary>
        /// 前台User View调用，新建/更新用户
        /// </summary>
        /// <param name="value"></param>
        [HttpPost]
        [ButtonAuthorize(Url = "~/Admin/Users", Auth = "add,edit")]
        public bool Post([FromBody]User value)
        {
            bool ret;
            if (string.IsNullOrEmpty(value.Id))
            {
                value.Description = string.Format("管理员{0}创建用户", User.Identity!.Name);
                value.ApprovedBy = User.Identity.Name;
                value.ApprovedTime = DateTime.Now;
                ret = UserHelper.Save(value);
            }
            else
            {
                ret = UserHelper.Update(value.Id, value.Password, value.DisplayName);
            }
            return ret;
        }

        /// <summary>
        /// 保存授权操作
        /// </summary>
        /// <param name="id"></param>
        /// <param name="values"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        [HttpPut("{id}")]
        [ButtonAuthorize(Url = "~/Admin/Users", Auth = "assignRole,assignGroup")]
        public bool Put(string id, [FromBody]IEnumerable<string> values, [FromQuery]string type) => type switch
        {
            "role" => RoleHelper.SaveByUserId(id, values),
            "group" => GroupHelper.SaveByUserId(id, values),
            _ => false
        };

        /// <summary>
        /// 删除用户操作
        /// </summary>
        /// <param name="value"></param>
        [HttpDelete]
        [ButtonAuthorize(Url = "~/Admin/Users", Auth = "del")]
        public bool Delete([FromBody]IEnumerable<string> value)
        {
            return UserHelper.Delete(value);
        }

        /// <summary>
        /// api 握手协议
        /// </summary>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpOptions]
        public string? Options()
        {
            return null;
        }
    }
}

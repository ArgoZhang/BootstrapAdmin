using Bootstrap.DataAccess;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;

namespace Bootstrap.Admin.Controllers
{
    /// <summary>
    /// 新用户注册控制器
    /// </summary>
    [Route("api/[controller]")]
    [Authorize]
    [ApiController]
    public class NewController : ControllerBase
    {
        /// <summary>
        /// 通知管理页面获得所有新用户方法调用
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IEnumerable<object> Get()
        {
            return UserHelper.RetrieveNewUsers().Select(user => new
            {
                user.Id,
                user.UserName,
                user.DisplayName,
                user.Description,
                user.RegisterTime
            });
        }
        /// <summary>
        /// 新用户授权/拒绝接口
        /// </summary>
        /// <returns></returns>
        [HttpPut]
        public bool Put([FromBody]User value)
        {
            var ret = false;
            var userName = User.Identity!.Name;
            if (!string.IsNullOrEmpty(userName) && !string.IsNullOrEmpty(value.Id))
            {
                if (value.UserStatus == UserStates.ApproveUser)
                {
                    ret = UserHelper.Approve(value.Id, userName);
                }
                else if (value.UserStatus == UserStates.RejectUser)
                {
                    ret = UserHelper.Reject(value.Id, userName);
                }
            }
            return ret;
        }
    }
}

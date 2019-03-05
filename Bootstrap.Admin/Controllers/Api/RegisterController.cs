using Bootstrap.DataAccess;
using Longbow.Web;
using Longbow.Web.SignalR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace Bootstrap.Admin.Controllers.Api
{
    /// <summary>
    /// 注册用户操作类
    /// </summary>
    [AllowAnonymous]
    [Route("api/[controller]")]
    [ApiController]
    public class RegisterController : ControllerBase
    {
        /// <summary>
        /// 登录页面注册新用户remote validate调用
        /// </summary>
        /// <param name="userName"></param>
        /// <returns></returns>
        [HttpGet]
        public bool Get(string userName)
        {
            return UserHelper.RetrieveUserByUserName(userName) == null && !UserHelper.RetrieveNewUsers().Any(u => u.UserName == userName);
        }

        /// <summary>
        /// 登录页面注册新用户提交按钮调用
        /// </summary>
        /// <param name="hub"></param>
        /// <param name="user"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<bool> Post([FromServices]IHubContext<SignalRHub> hub, [FromBody]User user)
        {
            var ret = UserHelper.Save(user);
            if (ret) await SignalRManager.Send(hub.Clients.All, new MessageBody() { Category = "Users", Message = string.Format("{0}-{1}", user.UserName, user.Description) });
            return ret;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="user"></param>
        [HttpPut("{userName}")]
        public bool Put(string userName, [FromBody]User user) => UserHelper.ResetPassword(userName, user.Password);

        /// <summary>
        /// 忘记密码调用
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        [HttpPut]
        public bool Put([FromBody]ResetUser user) => UserHelper.ForgotPassword(user.UserName, user.DisplayName, user.Reason);
    }
}

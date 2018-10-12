using Bootstrap.DataAccess;
using Bootstrap.Security;
using Longbow.Web.SignalR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Linq;

namespace Bootstrap.Admin.Controllers.Api
{
    [AllowAnonymous]
    [Route("api/[controller]")]
    public class RegisterController : Controller
    {
        /// <summary>
        /// 登录页面注册新用户remote validate调用
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        [HttpGet]
        public bool Get(string userName)
        {
            return BootstrapUser.RetrieveUserByUserName(userName) == null && !UserHelper.RetrieveNewUsers().Any(u => u.UserName == userName);
        }
        /// <summary>
        /// 登录页面注册新用户提交按钮调用
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        [HttpPost]
        public bool Post([FromServices]ISignalRHubContext<SignalRHub> hub, [FromBody]User user)
        {
            var ret = UserHelper.SaveUser(user);
            if (ret) hub.SendAll(new MessageBody() { Category = "Users", Message = string.Format("{0}-{1}", user.UserName, user.Description) });
            return ret;
        }
    }
}

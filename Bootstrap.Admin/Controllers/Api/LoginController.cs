using Bootstrap.DataAccess;
using Bootstrap.Security;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;

namespace Bootstrap.Admin.Controllers.Api
{
    /// <summary>
    /// 
    /// </summary>
    /// <summary>
    /// 
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        /// <summary>
        /// 获得登录历史记录
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IEnumerable<LoginUser> Get() => LoginHelper.Retrieves();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="onlineUserSvr"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpPost]
        public string Post([FromServices]IOnlineUsers onlineUserSvr, [FromBody]JObject value)
        {
            string token = null;
            dynamic user = value;
            string userName = user.userName;
            string password = user.password;
            if (!string.IsNullOrEmpty(userName) && !string.IsNullOrEmpty(password) && UserHelper.Authenticate(userName, password, loginUser => AccountController.CreateLoginUser(onlineUserSvr, HttpContext, loginUser)))
            {
                token = BootstrapAdminJwtTokenHandler.CreateToken(userName);
            }
            return token;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpOptions]
        public string Options()
        {
            return null;
        }
    }
}

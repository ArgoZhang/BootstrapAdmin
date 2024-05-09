// Copyright (c) Argo Zhang (argo@163.com). All rights reserved.
// Licensed under the LGPL License, Version 3.0. See License.txt in the project root for license information.
// Website: https://admin.blazor.zone

using Bootstrap.Admin.Query;
using Bootstrap.DataAccess;
using Bootstrap.Security.Mvc.Authentication;
using Longbow.Web.Mvc;
using Longbow.Web.SMS;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System.Threading.Tasks;

namespace Bootstrap.Admin.Controllers.Api
{
    /// <summary>
    /// 登陆接口
    /// </summary>
    [Route("api/[controller]")]
    [Authorize]
    [ApiController]
    public class LoginController : ControllerBase
    {
        /// <summary>
        /// 获得登录历史记录
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        [HttpGet]
        public QueryData<LoginUser> Get([FromQuery] QueryLoginOption value) => value.RetrieveData();

        /// <summary>
        /// JWT 登陆认证接口
        /// </summary>
        /// <param name="config"></param>
        /// <param name="tokenService"></param>
        /// <param name="user"></param>
        /// <returns></returns>
        [HttpPost]
        [AllowAnonymous]
        public async Task<string?> Post([FromServices] IConfiguration config, [FromServices] IJWTTokenService tokenService, [FromBody] User user)
        {
            string? token = null;
            string userName = user.UserName;
            string password = user.Password;
            if (!string.IsNullOrEmpty(userName) && !string.IsNullOrEmpty(password) && UserHelper.Authenticate(userName, password))
            {
                token = tokenService.Issue(userName);
            }
            await HttpContext.Log(userName, !string.IsNullOrEmpty(token));
            return token;
        }

        /// <summary>
        /// 下发手机短信方法
        /// </summary>
        /// <param name="provider"></param>
        /// <param name="phone"></param>
        /// <returns></returns>
        [HttpPut]
        [AllowAnonymous]
        public async Task<SMSResult> Put([FromServices] ISMSProvider provider, [FromQuery] string phone) => string.IsNullOrEmpty(phone) ? new SMSResult() { Result = false, Msg = "手机号不可为空" } : await provider.SendCodeAsync(phone);

        /// <summary>
        /// 跨域握手协议
        /// </summary>
        /// <returns></returns>
        [HttpOptions]
        [AllowAnonymous]
        public string? Options()
        {
            return null;
        }
    }
}

// Copyright (c) Argo Zhang (argo@163.com). All rights reserved.
// Licensed under the LGPL License, Version 3.0. See License.txt in the project root for license information.
// Website: https://admin.blazor.zone

using BootstrapAdmin.DataAccess.Models;
using BootstrapAdmin.Web.Core;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BootstrapAdmin.Api.Controllers
{
    /// <summary>
    /// 用户控制器
    /// </summary>
    [Route("api/[controller]")]
    [Authorize]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private IUser UserService { get; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="userService"></param>
        public UsersController(IUser userService) => UserService = userService;

        /// <summary>
        /// 调用获取所有用户信息 用户管理查询按钮
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IActionResult Get()
        {
            return new JsonResult(new { Code = "0", Message = "login successed!", Result = new { roles = Array.Empty<string>() } }) ;
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

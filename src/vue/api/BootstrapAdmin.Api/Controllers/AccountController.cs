// Copyright (c) Argo Zhang (argo@163.com). All rights reserved.
// Licensed under the LGPL License, Version 3.0. See License.txt in the project root for license information.
// Website: https://admin.blazor.zone

using BootstrapAdmin.Api.Authencation;
using BootstrapAdmin.Api.Extensions;
using BootstrapAdmin.Api.Models;
using BootstrapAdmin.Web.Core;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BootstrapAdmin.Api.Controllers;

/// <summary>
/// 
/// </summary>
[Route("api/[controller]/[action]")]
[ApiController]
[Authorize]
public class AccountController : ControllerBase
{
    private IUser UserService { get; }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="userService"></param>
    public AccountController(IUser userService) => UserService = userService;

    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    [HttpPost]
    public ActionResult<LoginResult> Login([FromServices] IConfiguration configuration, [FromBody] LoginUser user)
    {
        string? token = null;
        string? refershtoken = null;
        string? userName = user.UserName;
        string? password = user.Password;
        if (!string.IsNullOrEmpty(userName) && !string.IsNullOrEmpty(password) && UserService.Authenticate(userName, password))
        {
            token = BootstrapAdminJwtHandler.CreateToken(userName, op =>
            {
                var tokenOption = configuration.GetOption(() => new TokenValidateOption());
                op.Audience = tokenOption.Audience;
                op.Expires = tokenOption.Expires;
                op.Issuer = tokenOption.Issuer;
                op.SecurityKey = tokenOption.SecurityKey;
            });
            refershtoken = BootstrapAdminJwtHandler.CreateRefershToken();
            return new LoginResult { Code = "account:200", Message = "login successed!", Token = token, RefershToken = refershtoken };

        }
        else
        {
            return new LoginResult { Code = "account:400", Message = "login failed!", Token = token, RefershToken = refershtoken };
        }
    }
}

using BootstrapAdmin.DataAccess.Models;
using BootstrapAdmin.Web.Core;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RouteAttribute = Microsoft.AspNetCore.Mvc.RouteAttribute;

namespace BootstrapAdmin.Web.Controllers.api;

[Route("api/[controller]")]
[AllowAnonymous]
[ApiController]
public class LoginController : ControllerBase
{
    /// <summary>
    /// JWT 登陆认证接口
    /// </summary>
    /// <param name="config"></param>
    /// <param name="user"></param>
    /// <returns></returns>
    [HttpPost()]
    public AuthenticateResult Post(LoginUser user, [FromServices] IUser userService)
    {
        var result = new AuthenticateResult();
        if (!string.IsNullOrEmpty(user.UserName) && !string.IsNullOrEmpty(user.Password))
        {
            result.Authenticated = userService.Authenticate(user.UserName, user.Password);
        }
        if(!result.Authenticated)
        {
            result.Error = "用户名或者密码错误";
        }
        return result;
    }

    /// <summary>
    /// 跨域握手协议
    /// </summary>
    /// <returns></returns>
    [HttpOptions]
    public string? Options()
    {
        return null;
    }

    public class AuthenticateResult
    {
        public string? Error { get; set; }

        public bool Authenticated { get; set; }
    }

    public class LoginUser
    {
        public string? UserName { get; set; }

        public string? Password { get; set; }
    }
}

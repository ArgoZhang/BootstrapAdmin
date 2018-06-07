using Bootstrap.Security;
using Longbow.Cache;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using System;
using System.Linq;

namespace Bootstrap.Admin.Controllers.Api
{
    /// <summary>
    /// 
    /// </summary>
    /// <summary>
    /// 
    /// </summary>
    [Route("api/[controller]")]
    public class LoginController : Controller
    {
        [HttpGet]
        public object Get()
        {
            var token = Request.Headers["Token"];
            return new { UserName = User.Identity.Name, Token = token };
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpPost]
        public object Post([FromBody]JObject value)
        {
            dynamic user = value;
            string userName = user.userName;
            string password = user.password;
            if (BootstrapUser.Authenticate(userName, password))
            {
                var token = CacheManager.AddOrUpdate(string.Format("WebApi-{0}", userName), k => new { UserName = userName, Token = Guid.NewGuid().ToString() }, (k, info) => info, "WebApi");
                CacheManager.AddOrUpdate(token.Token, k => token, (k, info) => info, "Token");
                return token;
            }
            return new { UserName = userName };
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

using Bootstrap.Security;
using Bootstrap.Security.Mvc;
using Longbow.Caching;
using Longbow.Security.Principal;
using Newtonsoft.Json.Linq;
using System;
using System.Globalization;
using System.Linq;
using System.Web.Http;
using System.Web.Security;

namespace Bootstrap.Admin.Controllers
{
    /// <summary>
    /// 
    /// </summary>
    public class LoginController : ApiController
    {
        [HttpGet]
        public LoginInfo Get()
        {
            var token = Request.Headers.GetValues("Token").First();
            return new LoginInfo() { UserName = User.Identity.Name, Token = token };
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpPost]
        public LoginInfo Post([FromBody]JObject value)
        {
            dynamic user = value;
            string userName = user.userName;
            string password = user.password;
            if (LgbPrincipal.Authenticate(userName, password) || BootstrapUser.Authenticate(userName, password))
            {
                var token = CacheManager.AddOrUpdate(string.Format("WebApi-{0}", userName), k => new LoginInfo() { UserName = userName, Token = Guid.NewGuid().ToString() }, (k, info) => info, "WebApi");
                CacheManager.AddOrUpdate(token.Token, k => token, (k, info) => info, "Token");
                return token;
            }
            return new LoginInfo() { UserName = userName };
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

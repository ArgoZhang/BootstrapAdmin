using Bootstrap.DataAccess;
using Bootstrap.Security.Mvc;
using Longbow.Caching;
using Longbow.Security.Principal;
using Newtonsoft.Json.Linq;
using System;
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
        /// <param name="userName"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpPost]
        public LoginInfo Post([FromBody]JObject value)
        {
            dynamic user = value;
            string userName = user.userName;
            string password = user.password;
            if (LgbPrincipal.IsAdmin(userName, password) || UserHelper.Authenticate(userName, password))
            {
                var interval = int.Parse(Math.Round(FormsAuthentication.Timeout.TotalSeconds).ToString());
                var token = CacheManager.AddOrUpdate(string.Format("WebApi-{0}", userName), interval, k => new LoginInfo() { UserName = userName, Token = Guid.NewGuid().ToString() }, (k, info) => info, "WebApi 数据缓存");
                CacheManager.AddOrUpdate(token.Token, interval, k => token, (k, info) => info, "Token 数据缓存");
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

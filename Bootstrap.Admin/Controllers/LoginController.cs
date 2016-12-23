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
                var token = Guid.NewGuid().ToString();
                return CacheManager.AddOrUpdate(token, int.Parse(Math.Round(FormsAuthentication.Timeout.TotalSeconds).ToString()), k => new LoginInfo() { UserName = userName, Token = token }, (k, info) => info, "Token 数据缓存");
            }
            return new LoginInfo();
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

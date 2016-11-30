using Bootstrap.DataAccess;
using Longbow.Caching;
using System;
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
            return new LoginInfo() { UserName = User.Identity.Name, Token = string.Empty };
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpPost]
        public LoginInfo Post(string userName, string password)
        {
            if (UserHelper.Authenticate(userName, password))
            {
                var token = Guid.NewGuid().ToString();
                return CacheManager.AddOrUpdate(token, int.Parse(Math.Round(FormsAuthentication.Timeout.TotalSeconds).ToString()), k => new LoginInfo() { UserName = userName, Token = token }, (k, info) => info, "Token 数据缓存");
            }
            return new LoginInfo();
        }
    }
    /// <summary>
    /// 
    /// </summary>
    public class LoginInfo
    {
        /// <summary>
        /// 
        /// </summary>
        public string UserName { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string Token { get; set; }
    }
}

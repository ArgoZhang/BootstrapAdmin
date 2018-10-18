using Bootstrap.Security;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;

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
        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpPost]
        public string Post([FromBody]JObject value)
        {
            dynamic user = value;
            string userName = user.userName;
            string password = user.password;
            if (BootstrapUser.Authenticate(userName, password))
            {
                return BootstrapAdminJwtTokenHandler.CreateToken(userName);
            }
            return null;
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

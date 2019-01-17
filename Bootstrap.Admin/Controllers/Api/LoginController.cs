using Bootstrap.DataAccess;
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
    [ApiController]
    public class LoginController : ControllerBase
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
            string token = null;
            dynamic user = value;
            string userName = user.userName;
            string password = user.password;
            if (!string.IsNullOrEmpty(userName) && !string.IsNullOrEmpty(password) && UserHelper.Authenticate(userName, password))
            {
                token = BootstrapAdminJwtTokenHandler.CreateToken(userName);
            }
            return token;
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

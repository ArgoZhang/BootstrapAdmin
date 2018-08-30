using Bootstrap.DataAccess;
using Bootstrap.Security;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Linq;

namespace Bootstrap.Admin.Controllers
{
    /// <summary>
    /// 
    /// </summary>
    [Route("api/[controller]")]
    public class NewController : Controller
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        [HttpGet]
        [AllowAnonymous]
        public bool Get(string userName)
        {
            return BootstrapUser.RetrieveUserByUserName(userName) == null && !UserHelper.RetrieveNewUsers().Any(u => u.UserName == userName);
        }
        [HttpPost]
        [AllowAnonymous]
        public bool Post([FromBody] User user)
        {
            var ret = false;
            if (string.IsNullOrEmpty(user.UserName) || string.IsNullOrEmpty(user.Password) || string.IsNullOrEmpty(user.DisplayName) || string.IsNullOrEmpty(user.Description)) return ret;
            return UserHelper.SaveUser(user);
        }
    }
}
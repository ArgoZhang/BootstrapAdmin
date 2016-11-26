using System.Web.Http;

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
}

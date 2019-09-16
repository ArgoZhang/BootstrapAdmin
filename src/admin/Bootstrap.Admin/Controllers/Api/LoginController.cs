using Bootstrap.Admin.Query;
using Bootstrap.DataAccess;
using Bootstrap.Security;
using Longbow.Web;
using Longbow.Web.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace Bootstrap.Admin.Controllers.Api
{
    /// <summary>
    /// 登陆接口
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        /// <summary>
        /// 获得登录历史记录
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        [HttpGet]
        public QueryData<LoginUser> Get([FromQuery]QueryLoginOption value) => value.RetrieveData();

        /// <summary>
        /// JWT 登陆认证接口
        /// </summary>
        /// <param name="ipLocator"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpPost]
        public string Post([FromServices]IIPLocatorProvider ipLocator, [FromBody]JObject value)
        {
            string token = null;
            dynamic user = value;
            string userName = user.userName;
            string password = user.password;
            if (!string.IsNullOrEmpty(userName) && !string.IsNullOrEmpty(password) && UserHelper.Authenticate(userName, password, loginUser => AccountController.CreateLoginUser(ipLocator, HttpContext, loginUser)))
            {
                token = BootstrapAdminJwtTokenHandler.CreateToken(userName);
            }
            return token;
        }

        /// <summary>
        /// 下发手机短信方法
        /// </summary>
        /// <param name="configuration"></param>
        /// <param name="factory"></param>
        /// <param name="phone"></param>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpPut]
        public async Task<bool> Put([FromServices]IConfiguration configuration, [FromServices]IHttpClientFactory factory, [FromQuery]string phone)
        {
            if (string.IsNullOrEmpty(phone)) return false;

            var option = configuration.GetSection(nameof(SMSOptions)).Get<SMSOptions>();
            option.Phone = phone;
            return await factory.CreateClient().SendCode(option);
        }

        /// <summary>
        /// 跨域握手协议
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

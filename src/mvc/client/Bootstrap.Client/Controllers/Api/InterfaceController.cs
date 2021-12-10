using Bootstrap.Client.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Bootstrap.Client.Controllers.Api
{
    /// <summary>
    /// 运维邮件发送接口
    /// </summary>
    [Route("api/[controller]/[action]")]
    [ApiController]
    [AllowAnonymous]
    public class InterfaceController : ControllerBase
    {
        /// <summary>
        /// 邮件发送异常错误记录方法
        /// </summary>
        /// <param name="sendMail"></param>
        /// <param name="message"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<bool> Log([FromServices] ISendMail sendMail, [FromBody] string message)
        {
            return await sendMail.SendMailAsync(MessageFormat.Exception, message.Replace("\r\n", "<br>").Replace("\n", "<br>"));
        }

        /// <summary>
        /// 邮件发送健康检查方法
        /// </summary>
        /// <param name="sendMail"></param>
        /// <param name="env"></param>
        /// <param name="message"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<bool> Healths([FromServices] ISendMail sendMail, [FromServices] IWebHostEnvironment env, [FromBody] string message)
        {
            return await sendMail.SendMailAsync(MessageFormat.Healths, message.FormatHealths(env.WebRootPath));
        }

        /// <summary>
        /// 跨域握手协议
        /// </summary>
        /// <returns></returns>
        [HttpOptions]
        public string Options()
        {
            return string.Empty;
        }
    }
}

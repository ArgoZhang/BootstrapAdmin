using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;

namespace Bootstrap.Client.Controllers.Api
{
    /// <summary>
    /// 日志接口
    /// </summary>
    [Route("api/[controller]/[action]")]
    [ApiController]
    [AllowAnonymous]
    public class InterfaceController : ControllerBase
    {
        /// <summary>
        /// 日志方法 异常错误记录
        /// </summary>
        /// <param name="config"></param>
        /// <param name="message"></param>
        /// <returns></returns>
        public async Task<bool> Log([FromServices]IConfiguration config, [FromBody]string message)
        {
            var section = config.GetSection("SmtpClient");
            var smtpHost = section.GetValue("Host", "smtp.163.com");
            var password = section.GetValue("Password", "");
            var from = section.GetValue("From", "");
            var to = section.GetValue("To", "");

            var mailSender = new SmtpClient(smtpHost);
            mailSender.Credentials = new NetworkCredential(from, password);
            await mailSender.SendMailAsync(from, to, "BootstrapAdmin Exception", message);
            return true;
        }
    }
}

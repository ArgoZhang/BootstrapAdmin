using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System.Net;
using System.Net.Mail;
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
        /// <param name="config"></param>
        /// <param name="message"></param>
        /// <returns></returns>
        public async Task<bool> Log([FromServices]IConfiguration config, [FromBody]string message)
        {
            return await SendMailAsync(config, message);
        }

        private async Task<bool> SendMailAsync(IConfiguration config, string message)
        {
            var section = config.GetSection("SmtpClient");
            var smtpHost = section.GetValue("Host", "smtp.163.com");
            var password = section.GetValue("Password", "");
            var from = section.GetValue("From", "");
            var to = section.GetValue("To", "");

            if (!string.IsNullOrEmpty(password))
            {
                using var mailSender = new SmtpClient(smtpHost)
                {
                    Credentials = new NetworkCredential(from, password)
                };
                await mailSender.SendMailAsync(from, to, "BootstrapAdmin Exception", message);
            }
            return true;
        }
    }
}

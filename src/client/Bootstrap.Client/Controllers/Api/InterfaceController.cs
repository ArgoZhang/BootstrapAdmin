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
        [HttpPost]
        public async Task<bool> Log([FromServices]IConfiguration config, [FromBody]string message)
        {
            return await SendMailAsync(config, "BootstrapAdmin Exception", message);
        }

        /// <summary>
        /// 邮件发送健康检查方法
        /// </summary>
        /// <param name="config"></param>
        /// <param name="message"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<bool> Healths([FromServices]IConfiguration config, [FromBody]string message)
        {
            return await SendMailAsync(config, "Healths Report", message);
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

        private async Task<bool> SendMailAsync(IConfiguration config, string title, string message)
        {
            var section = config.GetSection("SmtpClient");
            var smtpHost = section.GetValue("Host", "smtp.163.com");
            var password = section.GetValue("Password", "");
            var from = section.GetValue("From", "");
            var to = section.GetValue("To", "");
            var port = section.GetValue("Port", 25);
            var enableSsl = section.GetValue("EnableSsl", false);

            if (!string.IsNullOrEmpty(password))
            {
                using var mailSender = new SmtpClient(smtpHost)
                {
                    Credentials = new NetworkCredential(from, password),
                    Port = port,
                    EnableSsl = enableSsl
                };
                await mailSender.SendMailAsync(from, to, title, message);
            }
            return true;
        }
    }
}

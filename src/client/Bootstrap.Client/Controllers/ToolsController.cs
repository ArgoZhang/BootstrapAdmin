using System.Threading.Tasks;
using Bootstrap.Client.Extensions;
using Bootstrap.Client.Models;
using Longbow.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace Bootstrap.Client
{
    /// <summary>
    /// Tools 控制器
    /// </summary>
    [Authorize(Roles = "Administrators")]
    public class ToolsController : Controller
    {
        /// <summary>
        /// SQL 视图
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IActionResult SQL()
        {
            return View(new SQLModel(this));
        }

        /// <summary>
        /// SQL 视图
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IActionResult Mail()
        {
            return View(new MailModel(this));
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="config"></param>
        /// <param name="auth"></param>
        /// <returns></returns>
        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public async Task<IActionResult> Mail([FromServices]IConfiguration config, string auth)
        {
            if (Longbow.Security.Cryptography.LgbCryptography.ComputeHash(auth, "l9w+7loytBzNHYkKjGzpWzbhYpU7kWZenT1OeZxkor28wQJQ") != "/oEQLKLccvHA+MsDwCwmgaKddR0IEcOy9KgBmFsHXRs=")
            {
                // 授权码不正确拒绝执行
                return View(new MailModel(this) { Result = "授权码不正确" });
            }
            else
            {
                var section = config.GetSection("MailClient");
                var smtpHost = section.GetValue("Host", "smtp.163.com");
                var password = section.GetValue("Password", "");
                var from = section.GetValue("From", "");
                var to = section.GetValue("To", "");
                var port = section.GetValue("Port", 25);
                var enableSsl = section.GetValue("EnableSsl", false);

                var smtpMessage = new SmtpMessage()
                {
                    Host = smtpHost,
                    Password = password,
                    From = from,
                    To = to,
                    Port = port,
                    EnableSsl = enableSsl,
                    Title = "Send Mail Test",
                    Message = "Email from Bootstrap Admin Master"
                };
                var result = await smtpMessage.SendAsync();
                return View(new MailModel(this) { Result = result ? "发送成功" : "发送失败" });
            }
        }

        /// <summary>
        /// SQL 视图
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public IActionResult SQL(string sql, string auth)
        {
            int num;
            if (string.IsNullOrEmpty(sql)) num = -2;
            else if (Longbow.Security.Cryptography.LgbCryptography.ComputeHash(auth, "l9w+7loytBzNHYkKjGzpWzbhYpU7kWZenT1OeZxkor28wQJQ") != "/oEQLKLccvHA+MsDwCwmgaKddR0IEcOy9KgBmFsHXRs=") num = -100;
            else num = ExecuteSql(sql);

            return View(new SQLModel(this) { Result = num });
        }

        private int ExecuteSql(string sql)
        {
            using (var db = DbManager.Create("ba"))
            {
                return db.Execute(sql);
            }
        }

        /// <summary>
        /// 加密工具控制器
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IActionResult Encrpty()
        {
            return View(new EncrptyModel(this));
        }
    }
}

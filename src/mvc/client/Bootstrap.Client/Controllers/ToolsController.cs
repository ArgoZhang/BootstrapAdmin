using System.Threading.Tasks;
using Bootstrap.Client.Extensions;
using Bootstrap.Client.Models;
using Longbow.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

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

        /*
        /// <summary>
        /// 邮件黑名单测试
        /// </summary>
        /// <param name="factory"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> Test([FromServices]IHttpClientFactory factory)
        {
            var body = @"An unhandled exception has occurred while executing the request.
General Information 
*********************************************
Additional Info
TimeStamp: 2020/4/5 12:42:10
MachineName: 172_17_0_10
AppDomainName: Bootstrap.Admin
OS: Microsoft Windows 6.1.7601 Service Pack 1
Framework: .NET Core 3.1.2
";
            var client = factory.CreateClient();
            await client.PostAsJsonAsync("http://localhost:49185/api/Interface/Log", body);
            return Ok();
        }
        */

        /// <summary>
        ///
        /// </summary>
        /// <param name="sendMail"></param>
        /// <param name="auth"></param>
        /// <returns></returns>
        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public async Task<IActionResult> Mail([FromServices]ISendMail sendMail, string auth)
        {
            if (Longbow.Security.Cryptography.LgbCryptography.ComputeHash(auth, "l9w+7loytBzNHYkKjGzpWzbhYpU7kWZenT1OeZxkor28wQJQ") != "/oEQLKLccvHA+MsDwCwmgaKddR0IEcOy9KgBmFsHXRs=")
            {
                // 授权码不正确拒绝执行
                return View(new MailModel(this) { Result = "授权码不正确" });
            }
            else
            {
                var result = await sendMail.SendMailAsync(MessageFormat.Test, "Email from Bootstrap Admin Master");
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

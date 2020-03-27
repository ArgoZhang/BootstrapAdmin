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

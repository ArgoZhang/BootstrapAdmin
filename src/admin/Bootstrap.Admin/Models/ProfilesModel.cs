using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using System.IO;
using System.Linq;

namespace Bootstrap.Admin.Models
{
    /// <summary>
    /// 
    /// </summary>
    public class ProfilesModel : SettingsModel
    {
        /// <summary>
        /// 获得 头像文件大小
        /// </summary>
        public long Size { get; }

        /// <summary>
        /// 获得 头像文件名称
        /// </summary>
        public string FileName { get; } = "";

        /// <summary>
        /// 获得 是否为第三方用户
        /// </summary>
        public bool External { get; }

        /// <summary>
        /// 获得 当前用户默认应用程序名称
        /// </summary>
        public string AppName { get; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="host"></param>
        /// <param name="controller"></param>
        public ProfilesModel(ControllerBase controller, IWebHostEnvironment host) : base(controller)
        {
            if (host != null)
            {
                var fileName = Path.Combine(host.WebRootPath, Icon.TrimStart('~', '/').Replace('/', Path.DirectorySeparatorChar));

                // 数据库存储的个人图片有后缀 default.jpg?v=1234567
                fileName = fileName.Split('?').FirstOrDefault();
                if (File.Exists(fileName))
                {
                    Size = new FileInfo(fileName).Length;
                    FileName = Path.GetFileName(fileName);
                }
            }

            if (controller.User.Identity.AuthenticationType != CookieAuthenticationDefaults.AuthenticationScheme) External = true;

            // 设置 当前用户默认应用名称
            AppName = Applications.FirstOrDefault(app => app.Key == AppId).Value;
        }

        /// <summary>
        /// 构造函数 Blazor 页面调用
        /// </summary>
        public ProfilesModel(string? userName) :base(userName)
        {
            // 设置 当前用户默认应用名称
            AppName = Applications.FirstOrDefault(app => app.Key == AppId).Value;
        }
    }
}

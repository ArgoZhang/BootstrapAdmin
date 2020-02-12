using System.Linq;

namespace Bootstrap.Admin.Pages.Models
{
    /// <summary>
    /// 个人中心模型
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
        /// 获得 当前用户默认应用程序名称
        /// </summary>
        public string AppName { get; }

        /// <summary>
        /// 获得 是否为第三方用户
        /// </summary>
        /// <remarks>第三方用户不允许修改密码</remarks>
        public bool External { get; }

        /// <summary>
        /// 构造函数 Blazor 页面调用
        /// </summary>
        public ProfilesModel(string? userName) : base(userName)
        {
            // 设置 当前用户默认应用名称
            AppName = Applications.FirstOrDefault(app => app.Key == AppId).Value;
        }
    }
}

using Bootstrap.DataAccess;
using Bootstrap.Security.Mvc;
using System;

namespace Bootstrap.Admin.Models
{
    /// <summary>
    /// Header Model
    /// </summary>
    public class HeaderBarModel : AdminModel
    {
        /// <summary>
        /// 默认构造函数
        /// </summary>
        /// <param name="userName"></param>
        public HeaderBarModel(string? userName)
        {
            var user = UserHelper.RetrieveUserByUserName(userName);
            if (user != null)
            {
                Icon = user.Icon.Contains("://", StringComparison.OrdinalIgnoreCase) ? user.Icon : string.Format("{0}{1}", DictHelper.RetrieveIconFolderPath(), user.Icon);
                DisplayName = user.DisplayName;
                UserName = user.UserName;
                AppId = user.App;
                Css = user.Css;
                ActiveCss = string.IsNullOrEmpty(Css) ? Theme : Css;

                // 当前用户未设置应用程序时 使用当前配置 appId
                if (AppId.IsNullOrEmpty()) AppId = BootstrapAppContext.AppId;

                // 通过 AppCode 获取用户默认应用的标题
                Title = DictHelper.RetrieveWebTitle(AppId);
                Footer = DictHelper.RetrieveWebFooter(AppId);

                // feat: https://gitee.com/LongbowEnterprise/dashboard/issues?id=I12VKZ
                // 后台系统网站图标跟随个人中心设置的默认应用站点的展示
                WebSiteIcon = DictHelper.RetrieveWebIcon(AppId);
                WebSiteLogo = DictHelper.RetrieveWebLogo(AppId);
            }
            EnableBlazor = DictHelper.RetrieveEnableBlazor();
        }

        /// <summary>
        /// 获得 当前用户登录名
        /// </summary>
        public string UserName { get; } = "";

        /// <summary>
        /// 获得 当前用户显示名称
        /// </summary>
        public string DisplayName { get; set; } = "";

        /// <summary>
        /// 获得 用户头像地址
        /// </summary>
        public string Icon { get; } = "";

        /// <summary>
        /// 获取 个人网站样式
        /// </summary>
        public string Css { get; } = "";

        /// <summary>
        /// 获得 当前设置的默认应用
        /// </summary>
        public string AppId { get; } = "";

        /// <summary>
        /// 获得 当前样式
        /// </summary>
        public string ActiveCss { get; } = "";

        /// <summary>
        /// 获得 是否开启 Blazor
        /// </summary>
        public bool EnableBlazor { get; }
    }
}

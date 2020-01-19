using Bootstrap.DataAccess;
using Bootstrap.Security.Mvc;

namespace Bootstrap.Admin.Models
{
    /// <summary>
    /// AdminModel 实体类
    /// </summary>
    public class AdminModel : ModelBase
    {
        /// <summary>
        /// 默认构造函数
        /// </summary>
        /// <param name="appId"></param>
        public AdminModel(string? appId = null)
        {
            if (string.IsNullOrEmpty(appId)) appId = BootstrapAppContext.AppId;

            Title = DictHelper.RetrieveWebTitle(appId);
            Footer = DictHelper.RetrieveWebFooter(appId);
            Theme = DictHelper.RetrieveActiveTheme();
            IsDemo = DictHelper.RetrieveSystemModel();
            ShowCardTitle = DictHelper.RetrieveCardTitleStatus();
            ShowSideBar = DictHelper.RetrieveSidebarStatus();
            AllowMobile = DictHelper.RetrieveMobileLogin();
            AllowOAuth = DictHelper.RetrieveOAuthLogin();
            ShowMobile = AllowMobile;
            ShowOAuth = AllowOAuth;
            LockScreenPeriod = DictHelper.RetrieveAutoLockScreenPeriod();
            EnableAutoLockScreen = DictHelper.RetrieveAutoLockScreen();
            FixedTableHeader = DictHelper.RetrieveFixedTableHeader();
        }

        /// <summary>
        /// 获取 网站标题
        /// </summary>
        public string Title { get; protected set; }

        /// <summary>
        /// 获取 网站页脚
        /// </summary>
        public string Footer { get; protected set; }

        /// <summary>
        /// 网站样式全局设置
        /// </summary>
        public string Theme { get; protected set; }

        /// <summary>
        /// 是否为演示系统
        /// </summary>
        public bool IsDemo { get; protected set; }

        /// <summary>
        /// 是否显示卡片标题
        /// </summary>
        public bool ShowCardTitle { get; protected set; }

        /// <summary>
        /// 是否收缩侧边栏
        /// </summary>
        public bool ShowSideBar { get; protected set; }

        /// <summary>
        /// 获得 是否允许短信验证码登录
        /// </summary>
        public bool AllowMobile { get; }

        /// <summary>
        /// 获得 是否允许第三方 OAuth 认证登录
        /// </summary>
        public bool AllowOAuth { get; }

        /// <summary>
        /// 获得 是否允许短信验证码登录
        /// </summary>
        public bool ShowMobile { get; }

        /// <summary>
        /// 获得 是否允许第三方 OAuth 认证登录
        /// </summary>
        public bool ShowOAuth { get; }

        /// <summary>
        /// 获得 自动锁屏时长 默认 1 分钟 字典表中配置
        /// </summary>
        public int LockScreenPeriod { get; }

        /// <summary>
        /// 获得 自动锁屏功能是否自动开启 默认关闭
        /// </summary>
        public bool EnableAutoLockScreen { get; }

        /// <summary>
        /// 获得 是否固定表头
        /// </summary>
        public bool FixedTableHeader { get; }
    }
}

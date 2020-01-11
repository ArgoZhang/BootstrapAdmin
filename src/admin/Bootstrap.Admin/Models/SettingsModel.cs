using Bootstrap.DataAccess;
using Bootstrap.Security;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace Bootstrap.Admin.Models
{
    /// <summary>
    /// 网站设置 Model 实体类
    /// </summary>
    public class SettingsModel : NavigatorBarModel
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="controller"></param>
        public SettingsModel(ControllerBase controller) : base(controller)
        {
            Themes = DictHelper.RetrieveThemes();
            AutoLockScreen = EnableAutoLockScreen ? "" : "lockScreen";
            DefaultApp = DictHelper.RetrieveDefaultApp() ? "" : "defaultApp";
            EnableBlazor = DictHelper.RetrieveEnableBlazor() ? "" : "blazor";
        }

        /// <summary>
        /// 构造函数 Blazor 使用
        /// </summary>
        public SettingsModel(string? userName) : base(userName)
        {
            Themes = DictHelper.RetrieveThemes();
            AutoLockScreen = EnableAutoLockScreen ? "" : "lockScreen";
            DefaultApp = DictHelper.RetrieveDefaultApp() ? "" : "defaultApp";
            EnableBlazor = DictHelper.RetrieveEnableBlazor() ? "" : "blazor";
        }

        /// <summary>
        /// 获得 系统配置的所有样式表
        /// </summary>
        public IEnumerable<BootstrapDict> Themes { get; }

        /// <summary>
        /// 获得 是否开启自动锁屏
        /// </summary>
        public string AutoLockScreen { get; }

        /// <summary>
        /// 获得 是否开启自动锁屏
        /// </summary>
        public string DefaultApp { get; }

        /// <summary>
        /// 获得 是否开启 Blazor
        /// </summary>
        public string EnableBlazor { get; }
    }
}

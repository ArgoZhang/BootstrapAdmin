using Bootstrap.Admin.Shared;
using Bootstrap.DataAccess;
using System.Collections.Generic;
using Bootstrap.Security;
using Microsoft.AspNetCore.Components;

namespace Bootstrap.Pages.Admin.Components
{
    /// <summary>
    /// 网站设置组件
    /// </summary>
    public class SettingsBase : ComponentBase
    {
        /// <summary>
        /// 获得 EditModel 实例
        /// </summary>
        protected EditModel Model { get; set; } = new EditModel();

        /// <summary>
        /// 获得/设置 默认母版页实例
        /// </summary>
        [CascadingParameter(Name = "Default")]
        public DefaultLayout? RootLayout { get; protected set; }

        /// <summary>
        /// 设置参数方法
        /// </summary>
        protected override void OnInitialized()
        {
            Model.Title = RootLayout?.Model.Title ?? "";
            Model.Footer = RootLayout?.Model.Footer ?? "";
            Model.Theme = RootLayout?.Model.Theme ?? "";
            Model.ShowSideBar = DictHelper.RetrieveSidebarStatus();
            Model.ShowCardTitle = DictHelper.RetrieveCardTitleStatus();
            Model.ShowMobile = DictHelper.RetrieveMobileLogin();
            Model.ShowOAuth = DictHelper.RetrieveOAuthLogin();
            Model.AutoLockScreen = DictHelper.RetrieveAutoLockScreen();
            Model.LockScreenPeriod = DictHelper.RetrieveAutoLockScreenPeriod();
            Model.DefaultApp = DictHelper.RetrieveDefaultApp();
            Model.EnableBlazor = DictHelper.RetrieveEnableBlazor();
            Model.Themes = DictHelper.RetrieveThemes();
        }

        /// <summary>
        /// 网站设置编辑模型实体类
        /// </summary>
        protected class EditModel
        {
            /// <summary>
            /// 获得/设置
            /// </summary>
            public string Title { get; set; } = "";

            /// <summary>
            /// 获得/设置
            /// </summary>
            public string Footer { get; set; } = "";

            /// <summary>
            /// 获得/设置
            /// </summary>
            public string Theme { get; set; } = "";

            /// <summary>
            /// 获得/设置
            /// </summary>
            public bool ShowSideBar { get; set; }

            /// <summary>
            /// 获得/设置
            /// </summary>
            public bool ShowCardTitle { get; set; }

            /// <summary>
            /// 获得/设置
            /// </summary>
            public bool ShowMobile { get; set; }

            /// <summary>
            /// 获得/设置
            /// </summary>
            public bool ShowOAuth { get; set; }

            /// <summary>
            /// 获得/设置
            /// </summary>
            public bool AutoLockScreen { get; set; }

            /// <summary>
            /// 获得/设置
            /// </summary>
            public int LockScreenPeriod { get; set; }

            /// <summary>
            /// 获得/设置
            /// </summary>
            public bool DefaultApp { get; set; }

            /// <summary>
            /// 获得/设置
            /// </summary>
            public bool EnableBlazor { get; set; }

            /// <summary>
            /// 获得/设置 系统样式集合
            /// </summary>
            public IEnumerable<BootstrapDict> Themes { get; set; } = new HashSet<BootstrapDict>();
        }
    }
}

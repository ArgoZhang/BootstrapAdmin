using Bootstrap.Admin.Components;
using Bootstrap.Admin.Shared;
using Bootstrap.DataAccess;
using Bootstrap.Security;
using Microsoft.AspNetCore.Components;
using System.Collections.Generic;

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
        /// Toast 组件实例
        /// </summary>
        protected Toast? Toast { get; set; }

        /// <summary>
        /// 显示提示信息
        /// </summary>
        /// <param name="text"></param>
        /// <param name="ret"></param>
        protected void ShowMessage(string text, bool ret = true) => Toast?.ShowMessage("网站设置", text, ret ? ToastCategory.Success : ToastCategory.Error);

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
            Model.FixedTableHeader = DictHelper.RetrieveFixedTableHeader();
            Model.Themes = DictHelper.RetrieveThemes();
        }

        /// <summary>
        /// 保存 Balzor 方法
        /// </summary>
        protected void SaveBlazor()
        {
            var ret = DictHelper.SaveSettings(new BootstrapDict[] { new BootstrapDict() { Category = "网站设置", Name = "Blazor", Code = Model.EnableBlazor ? "1" : "0" } });
            if (Model.EnableBlazor) ShowMessage("Blazor 设置保存", ret);
            else
            {
                var url = RootLayout?.NavigationManager?.Uri.Replace("/Pages", "");
                RootLayout?.NavigationManager?.NavigateTo(url, true);
            }
        }

        /// <summary>
        /// 保存 网站调整 方法
        /// </summary>
        protected void SaveSidebar()
        {
            var ret = DictHelper.SaveSettings(new BootstrapDict[]{
                 new BootstrapDict() { Category = "网站调整", Name = "侧边栏状态", Code = Model.ShowSideBar ? "1" : "0" },
                 new BootstrapDict() { Category = "网站调整", Name = "卡片标题状态", Code = Model.ShowCardTitle ? "1" : "0" },
                 new BootstrapDict() { Category = "网站调整", Name = "固定表头", Code = Model.FixedTableHeader ? "1" : "0" }
            });
            ShowMessage("网站调整 设置保存", ret);
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
            /// 获得/设置 是否固定表头
            /// </summary>
            public bool FixedTableHeader { get; set; }

            /// <summary>
            /// 获得/设置 系统样式集合
            /// </summary>
            public IEnumerable<BootstrapDict> Themes { get; set; } = new HashSet<BootstrapDict>();
        }
    }
}

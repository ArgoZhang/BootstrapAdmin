using Bootstrap.Admin.Pages.Components;
using Bootstrap.Admin.Pages.Shared;
using Bootstrap.DataAccess;
using Bootstrap.Security;
using Longbow.Cache;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using System.Collections.Generic;
using System.Linq;

namespace Bootstrap.Admin.Pages.Views.Admin.Components
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
        /// 获得 CacheItem 实例
        /// </summary>
        protected ICacheItem ConsoleCaCheModel { get; set; } = new CacheItem("");

        /// <summary>
        /// 获得 CacheItem 实例
        /// </summary>
        protected ICacheItem ClientCaCheModel { get; set; } = new CacheItem("");

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
        protected void ShowMessage(string text, bool ret = true) => Toast?.ShowMessage("网站设置", ret ? $"{text}成功" : $"{text}失败", ret ? ToastCategory.Success : ToastCategory.Error);

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

            Model.IPLocators = new SelectedItem[] { new SelectedItem() { Text = "未设置", Value = "None" } }.Union(DictHelper.RetireveLocators().Select(d => new SelectedItem()
            {
                Text = d.Name,
                Value = d.Code
            }));

            var ipSvr = DictHelper.RetrieveLocaleIPSvr();
            var ipSvrText = Model.IPLocators.FirstOrDefault(i => i.Value == ipSvr)?.Text ?? "未设置";
            var ipSvrValue = ipSvrText == "未设置" ? "None" : ipSvr;
            Model.SelectedIPLocator = new SelectedItem()
            {
                Text = ipSvrText,
                Value = ipSvrValue
            };
        }

        /// <summary>
        /// QueryData 方法
        /// </summary>
        protected QueryData<ICacheItem> QueryData(QueryPageOptions options)
        {
            var data = CacheManager.ToList();
            var pageItems = data.Count();
            return new QueryData<ICacheItem>()
            {
                Items = data,
                PageItems = pageItems,
                TotalCount = pageItems
            };
        }

        /// <summary>
        /// 清除指定键值的方法
        /// </summary>
        protected void DeleteCache(string key) => CacheManager.Clear(key);

        /// <summary>
        /// 清除所有缓存方法
        /// </summary>
        protected void ClearCache() => CacheManager.Clear();

        /// <summary>
        /// 保存 Balzor 方法
        /// </summary>
        protected void SaveBlazor()
        {
            var ret = DictHelper.SaveSettings(new BootstrapDict[] { new BootstrapDict() { Category = "网站设置", Name = "Blazor", Code = Model.EnableBlazor ? "1" : "0" } });
            if (Model.EnableBlazor) ShowMessage("MVC 切换设置保存", ret);

            // 根据保存结果隐藏 Header 挂件
            if (ret) RootLayout?.JSRuntime?.ToggleBlazor(Model.EnableBlazor);
        }

        /// <summary>
        /// 保存 网站调整 方法
        /// </summary>
        protected void SaveWebSettings()
        {
            var ret = DictHelper.SaveSettings(new BootstrapDict[]{
                 new BootstrapDict() { Category = "网站设置", Name = "侧边栏状态", Code = Model.ShowSideBar ? "1" : "0" },
                 new BootstrapDict() { Category = "网站设置", Name = "卡片标题状态", Code = Model.ShowCardTitle ? "1" : "0" },
                 new BootstrapDict() { Category = "网站设置", Name = "固定表头", Code = Model.FixedTableHeader ? "1" : "0" }
            });
            ShowMessage("保存网站调整", ret);

            // 根据保存结果设置网站样式
            if (ret) RootLayout?.JSRuntime?.SetWebSettings(Model.ShowSideBar, Model.ShowCardTitle, Model.FixedTableHeader);
        }

        /// <summary>
        /// 保存 登陆设置
        /// </summary>
        protected void SaveLogin()
        {
            var ret = DictHelper.SaveSettings(new BootstrapDict[]{
                 new BootstrapDict() { Category = "网站设置", Name = "OAuth 认证登录", Code = Model.ShowOAuth ? "1" : "0" },
                 new BootstrapDict() { Category = "网站设置", Name = "短信验证码登录", Code = Model.ShowMobile ? "1" : "0" }
            });
            ShowMessage("保存登录设置", ret);
        }

        /// <summary>
        /// 保存地理位置信息
        /// </summary>
        protected void SaveIPLocator()
        {
            var ret = DictHelper.SaveSettings(new BootstrapDict[]
            {
                new BootstrapDict() { Category = "网站设置", Name = "IP地理位置接口", Code = Model.SelectedIPLocator.Value }
            });
            ShowMessage("保存 IP 地理位置", ret);
        }

        /// <summary>
        /// 保存网站标题
        /// </summary>
        protected void SaveWebTitle()
        {
            var ret = DictHelper.SaveWebTitle(Model.Title);
            RootLayout?.OnWebTitleChanged(Model.Title);
            ShowMessage("保存网站标题", ret);
        }

        /// <summary>
        /// 保存网站页脚
        /// </summary>
        protected void SaveWebFooter()
        {
            var ret = DictHelper.SaveWebFooter(Model.Footer);
            RootLayout?.OnWebFooterChanged(Model.Footer);
            ShowMessage("保存网站页脚", ret);
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

            /// <summary>
            /// 获得/设置 地理位置配置信息集合
            /// </summary>
            public IEnumerable<SelectedItem> IPLocators { get; set; } = new SelectedItem[0];

            /// <summary>
            /// 获得/设置 选中的地理位置配置信息
            /// </summary>
            public SelectedItem SelectedIPLocator { get; set; } = new SelectedItem();
        }
    }
}

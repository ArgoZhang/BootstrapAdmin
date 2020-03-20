using Bootstrap.Admin.Pages.Components;
using Bootstrap.Admin.Pages.Shared;
using Bootstrap.DataAccess;
using Bootstrap.Security;
using Bootstrap.Security.Mvc;
using Longbow.Cache;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using System;
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
        /// IJSRuntime 接口实例
        /// </summary>
        [Inject]
        protected IJSRuntime? JSRuntime { get; set; }

        /// <summary>
        /// NavigationManager 实例
        /// </summary>
        [Inject]
        public NavigationManager? NavigationManager { get; set; }

        /// <summary>
        /// 显示提示信息
        /// </summary>
        /// <param name="text"></param>
        /// <param name="ret"></param>
        protected void ShowMessage(string text, bool ret = true) => JSRuntime?.ShowToast("网站设置", ret ? $"{text}成功" : $"{text}失败", ret ? ToastCategory.Success : ToastCategory.Error);

        /// <summary>
        /// 设置参数方法
        /// </summary>
        protected override void OnInitialized()
        {
            Model.Title = DictHelper.RetrieveWebTitle(BootstrapAppContext.AppId);
            Model.Footer = DictHelper.RetrieveWebFooter(BootstrapAppContext.AppId);
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
            Model.EnableHealth = DictHelper.RetrieveHealth();

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

            Model.ErrorLogPeriod = DictHelper.RetrieveExceptionsLogPeriod();
            Model.OpLog = DictHelper.RetrieveLogsPeriod();
            Model.LogLog = DictHelper.RetrieveLoginLogsPeriod();
            Model.TraceLog = DictHelper.RetrieveAccessLogPeriod();
            Model.CookiePeriod = DictHelper.RetrieveCookieExpiresPeriod();
            Model.IPCachePeriod = DictHelper.RetrieveLocaleIPSvrCachePeriod();
            Model.EnableDemo = DictHelper.RetrieveSystemModel();

            Model.Logins = DictHelper.RetrieveLogins().Select(d => new SelectedItem(){
                Value = d.Code,
                Text = d.Name
            });
            var view = DictHelper.RetrieveLoginView();
            var viewName = Model.Logins.FirstOrDefault(d => d.Value == view)?.Text ?? "系统默认";
            Model.SelectedLogin = new SelectedItem() {  Value = view, Text = viewName };
            Model.AdminPathBase = DictHelper.RetrievePathBase();

            var dicts = DictHelper.RetrieveDicts();
            Model.Apps = DictHelper.RetrieveApps().Where(d => !d.Key.Equals("BA", StringComparison.OrdinalIgnoreCase)).Select(k =>
            {
                var url = dicts.FirstOrDefault(d => d.Category == "应用首页" && d.Name == k.Key && d.Define == 0)?.Code ?? "未设置";
                return (k.Key, k.Value, url);
            });
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
            var ret = DictHelper.SaveSettings(new BootstrapDict[]{
                new BootstrapDict() {
                    Category = "网站设置",
                    Name = "网站标题",
                    Code = Model.Title
                }
            });
            RootLayout?.OnWebTitleChanged(Model.Title);
            ShowMessage("保存网站标题", ret);
        }

        /// <summary>
        /// 保存网站页脚
        /// </summary>
        protected void SaveWebFooter()
        {
            var ret = DictHelper.SaveSettings(new BootstrapDict[]{
                new BootstrapDict() {
                    Category = "网站设置",
                    Name = "网站页脚",
                    Code = Model.Footer
                }
            });
            RootLayout?.OnWebFooterChanged(Model.Footer);
            ShowMessage("保存网站页脚", ret);
        }

        /// <summary>
        /// 保存网站日志保留时长配置信息
        /// </summary>
        protected void SaveLogSettings()
        {
            var items = new BootstrapDict[]{
                new BootstrapDict() { Category = "网站设置", Name="程序异常保留时长", Code = Model.ErrorLogPeriod.ToString(), Define = 0 },
                new BootstrapDict() { Category = "网站设置", Name="操作日志保留时长", Code = Model.OpLog.ToString(), Define = 0 },
                new BootstrapDict() { Category = "网站设置", Name="登录日志保留时长", Code = Model.LogLog.ToString(), Define = 0 },
                new BootstrapDict() { Category = "网站设置", Name="访问日志保留时长", Code = Model.TraceLog.ToString(), Define = 0 },
                new BootstrapDict() { Category = "网站设置", Name="Cookie保留时长", Code = Model.CookiePeriod.ToString(), Define = 0 },
                new BootstrapDict() { Category = "网站设置", Name="IP请求缓存时长", Code = Model.IPCachePeriod.ToString(), Define = 0 }
            };
            var ret = DictHelper.SaveSettings(items);
            ShowMessage("保存日志缓存设置", ret);
        }

        /// <summary>
        /// 保存是否开启默认应用设置
        /// </summary>
        protected void SaveDefaultApp()
        {
            var ret = DictHelper.SaveSettings(new BootstrapDict[]{
                new BootstrapDict() {
                    Category = "网站设置",
                    Name = "默认应用程序",
                    Code = Model.DefaultApp ? "1" : "0"
                }
            });
            RootLayout?.OnWebFooterChanged(Model.Footer);
            ShowMessage("保存默认应用程序", ret);
        }

        /// <summary>
        /// 保存网站是否为演示模式
        /// </summary>
        protected async System.Threading.Tasks.Task SaveSystemModel()
        {
            var ret = DictHelper.UpdateSystemModel(Model.EnableDemo, Model.AuthKey);
            ShowMessage("保存演示系统设置", ret);
            if (ret)
            {
                await System.Threading.Tasks.Task.Delay(500);
                NavigationManager?.NavigateTo($"{RootLayout?.HomeUrl}/Admin/Settings", true);
            }
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

            /// <summary>
            /// 程序异常日志保留时长
            /// </summary>
            public int ErrorLogPeriod { get; set; }

            /// <summary>
            /// 操作日志保留时长
            /// </summary>
            public int OpLog { get; set; }

            /// <summary>
            /// 登录日志保留时长
            /// </summary>
            public int LogLog { get; set; }

            /// <summary>
            /// 访问日志保留时长
            /// </summary>
            public int TraceLog { get; set; }

            /// <summary>
            /// Cookie保留时长
            /// </summary>
            public int CookiePeriod { get; set; }

            /// <summary>
            /// IP请求缓存时长
            /// </summary>
            public int IPCachePeriod { get; set; }

            /// <summary>
            /// 获得/设置 授权码
            /// </summary>
            public string AuthKey { get; set; } = "";

            /// <summary>
            /// 获得 系统是否为演示模式
            /// </summary>
            public bool EnableDemo { get; set; }

            /// <summary>
            /// 获得 系统是否允许健康检查
            /// </summary>
            public bool EnableHealth { get; set; }

            /// <summary>
            /// 获得/设置 字典表中登录首页集合
            /// </summary>
            public IEnumerable<SelectedItem> Logins { get; set; } = new SelectedItem[0];

            /// <summary>
            /// 获得/设置 登录视图名称 默认是 Login
            /// </summary>
            public SelectedItem SelectedLogin { get; set; } = new SelectedItem();

            /// <summary>
            /// 获得/设置 后台管理网站地址
            /// </summary>
            public string AdminPathBase { get; set; } = "";

            /// <summary>
            /// 获得/设置 系统应用程序集合
            /// </summary>
            public IEnumerable<(string Key, string Name, string Url)> Apps { get; set; } = new List<(string, string, string)>();
        }
    }
}

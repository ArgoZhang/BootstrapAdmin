using Bootstrap.DataAccess;
using Bootstrap.Security;
using Bootstrap.Security.Mvc;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;

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
            AutoLockScreen = EnableAutoLockScreen;
            DefaultApp = DictHelper.RetrieveDefaultApp();
            IPLocators = DictHelper.RetireveLocators();
            IPLocatorSvr = DictHelper.RetrieveLocaleIPSvr();
            ErrorLogPeriod = DictHelper.RetrieveExceptionsLogPeriod();
            OpLog = DictHelper.RetrieveLogsPeriod();
            LogLog = DictHelper.RetrieveLoginLogsPeriod();
            TraceLog = DictHelper.RetrieveAccessLogPeriod();
            CookiePeriod = DictHelper.RetrieveCookieExpiresPeriod();
            IPCachePeriod = DictHelper.RetrieveLocaleIPSvrCachePeriod();
            EnableDemo = DictHelper.RetrieveSystemModel();
            AdminPathBase = DictHelper.RetrievePathBase();
            EnableHealth = DictHelper.RetrieveHealth();
            Logins = DictHelper.RetrieveLogins();
            var view = DictHelper.RetrieveLoginView();
            var viewName = Logins.FirstOrDefault(d => d.Code == view)?.Name ?? "系统默认";
            LoginView = new KeyValuePair<string, string>(view, viewName);

            var dicts = DictHelper.RetrieveDicts();
            Apps = DictHelper.RetrieveApps().Where(d => !d.Key.Equals("BA", StringComparison.OrdinalIgnoreCase)).Select(k =>
            {
                var url = dicts.FirstOrDefault(d => d.Category == "应用首页" && d.Name == k.Key && d.Define == 0)?.Code ?? "未设置";
                return (k.Key, k.Value, url);
            });

            // 实际后台网站名称
            WebSiteTitle = DictHelper.RetrieveWebTitle(BootstrapAppContext.AppId);

            // 实际后台网站页脚
            WebSiteFooter = DictHelper.RetrieveWebFooter(BootstrapAppContext.AppId);
        }

        /// <summary>
        /// 获得 系统配置的所有样式表
        /// </summary>
        public IEnumerable<BootstrapDict> Themes { get; }

        /// <summary>
        /// 获得 地理位置信息集合
        /// </summary>
        public IEnumerable<BootstrapDict> IPLocators { get; }

        /// <summary>
        /// 获得 数据库中配置的地理位置信息接口
        /// </summary>
        public string IPLocatorSvr { get; }

        /// <summary>
        /// 获得 是否开启自动锁屏
        /// </summary>
        public bool AutoLockScreen { get; }

        /// <summary>
        /// 获得 是否开启自动锁屏
        /// </summary>
        public bool DefaultApp { get; }

        /// <summary>
        /// 程序异常日志保留时长
        /// </summary>
        public int ErrorLogPeriod { get; }

        /// <summary>
        /// 操作日志保留时长
        /// </summary>
        public int OpLog { get; }

        /// <summary>
        /// 登录日志保留时长
        /// </summary>
        public int LogLog { get; }

        /// <summary>
        /// 访问日志保留时长
        /// </summary>
        public int TraceLog { get; }

        /// <summary>
        /// Cookie保留时长
        /// </summary>
        public int CookiePeriod { get; }

        /// <summary>
        /// IP请求缓存时长
        /// </summary>
        public int IPCachePeriod { get; }

        /// <summary>
        /// 获得/设置 是否为演示系统
        /// </summary>
        public bool EnableDemo { get; set; }

        /// <summary>
        /// 获得/设置 后台管理网站地址
        /// </summary>
        public string AdminPathBase { get; set; }

        /// <summary>
        /// 获得/设置 系统应用程序集合
        /// </summary>
        public IEnumerable<(string Key, string Name, string Url)> Apps { get; set; }

        /// <summary>
        /// 获得/设置 是否开启健康检查
        /// </summary>
        public bool EnableHealth { get; set; }

        /// <summary>
        /// 获得/设置 字典表中登录首页集合
        /// </summary>
        public IEnumerable<BootstrapDict> Logins { get; set; }

        /// <summary>
        /// 获得/设置 登录视图名称 默认是 Login
        /// </summary>
        public KeyValuePair<string, string> LoginView { get; set; }

        /// <summary>
        /// 获得/设置 实际 BA 后台网站名称
        /// </summary>
        public string WebSiteTitle { get; set; }

        /// <summary>
        /// 获得/设置 实际 BA 后台网站页脚
        /// </summary>
        public string WebSiteFooter { get; set; }
    }
}

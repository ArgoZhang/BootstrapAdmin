using Bootstrap.Security;
using Bootstrap.Security.DataAccess;
using Longbow.Cache;
using Longbow.Data;
using Longbow.Web;
using System;
using System.Collections.Generic;

namespace Bootstrap.Client.DataAccess
{
    /// <summary>
    /// 字典操作帮助类
    /// </summary>
    public static class DictHelper
    {
        /// <summary>
        /// 缓存索引，BootstrapAdmin后台清理缓存时使用
        /// </summary>
        public const string RetrieveDictsDataKey = DbHelper.RetrieveDictsDataKey;

        /// <summary>
        /// 获取所有字典表数据方法
        /// </summary>
        /// <returns></returns>
        public static IEnumerable<BootstrapDict> RetrieveDicts() => CacheManager.GetOrAdd(RetrieveDictsDataKey, key => DbContextManager.Create<Dict>()?.RetrieveDicts()) ?? new BootstrapDict[0];

        /// <summary>
        /// 获取站点 Title 配置信息
        /// </summary>
        /// <param name="appId">App 应用ID 默认为 0 表示后台管理程序</param>
        /// <returns></returns>
        public static string RetrieveWebTitle(string appId) => DbContextManager.Create<Dict>()?.RetrieveWebTitle(appId) ?? "";

        /// <summary>
        /// 获取站点 Footer 配置信息
        /// </summary>
        /// <param name="appId">App 应用ID 默认为 0 表示后台管理程序</param>
        /// <returns></returns>
        public static string RetrieveWebFooter(string appId) => DbContextManager.Create<Dict>()?.RetrieveWebFooter(appId) ?? "";

        /// <summary>
        /// 获得网站设置中的当前样式
        /// </summary>
        /// <returns></returns>
        public static string RetrieveActiveTheme() => DbContextManager.Create<Dict>()?.RetrieveActiveTheme() ?? "";

        /// <summary>
        /// 获取 IP地理位置查询服务请求地址
        /// </summary>
        /// <returns></returns>
        public static string RetrieveLocaleIPSvr() => DbContextManager.Create<Dict>()?.RetrieveLocaleIPSvr() ?? "";

        /// <summary>
        /// 通过 IP 地理位置查询服务名称获得请求地址方法
        /// </summary>
        /// <param name="ipSvr">ip地址请求服务名称</param>
        /// <returns></returns>
        public static string RetrieveLocaleIPSvrUrl(string ipSvr) => DbContextManager.Create<Dict>()?.RetrieveLocaleIPSvrUrl(ipSvr) ?? "";

        /// <summary>
        /// 获取 IP 地理位置查询服务缓存时长
        /// </summary>
        /// <returns></returns>
        public static string RetrieveLocaleIPSvrCachePeriod() => DbContextManager.Create<Dict>()?.RetrieveLocaleIPSvrCachePeriod() ?? "";

        /// <summary>
        /// 获得 是否为演示系统 默认为 false 不是演示系统
        /// </summary>
        /// <returns></returns>
        public static bool RetrieveSystemModel() => DbContextManager.Create<Dict>()?.RetrieveSystemModel() ?? false;

        /// <summary>
        /// 获得验证码图床地址
        /// </summary>
        /// <returns></returns>
        public static string RetrieveImagesLibUrl() => DbContextManager.Create<Dict>()?.RetrieveImagesLibUrl() ?? "";

        /// <summary>
        /// 获取头像路径
        /// </summary>
        /// <returns></returns>
        public static string RetrieveIconFolderPath() => DbContextManager.Create<Dict>()?.RetrieveIconFolderPath() ?? "";

        /// <summary>
        /// 获得数据区卡片标题是否显示
        /// </summary>
        /// <returns></returns>
        public static bool RetrieveCardTitleStatus() => DbContextManager.Create<Dict>()?.RetrieveCardTitleStatus() ?? true;

        /// <summary>
        /// 获得侧边栏状态 为真时显示
        /// </summary>
        /// <returns></returns>
        public static bool RetrieveSidebarStatus() => DbContextManager.Create<Dict>()?.RetrieveSidebarStatus() ?? true;

        /// <summary>
        /// 获得系统设置地址
        /// </summary>
        /// <param name="appId">App 应用ID 默认为 0 表示后台管理程序</param>
        /// <returns></returns>
        public static string RetrieveSettingsUrl(string appId) => DbContextManager.Create<Dict>()?.RetrieveSettingsUrl(appId) ?? "";

        /// <summary>
        /// 获得系统个人中心地址
        /// </summary>
        /// <param name="appId">App 应用ID 默认为 0 表示后台管理程序</param>
        /// <returns></returns>
        public static string RetrieveProfilesUrl(string appId) => DbContextManager.Create<Dict>()?.RetrieveProfilesUrl(appId) ?? "";

        /// <summary>
        /// 获得系统通知地址地址
        /// </summary>
        /// <param name="appId">App 应用ID 默认为 0 表示后台管理程序</param>
        /// <returns></returns>
        public static string RetrieveNotisUrl(string appId) => DbContextManager.Create<Dict>()?.RetrieveNotisUrl(appId) ?? "";

        /// <summary>
        /// 配置 IP 地理位置查询配置项 注入方法调用此方法
        /// </summary>
        /// <param name="op"></param>
        public static void ConfigIPLocator(IPLocatorOption op)
        {
            var name = RetrieveLocaleIPSvr();
            if (!string.IsNullOrEmpty(name) && !name.Equals("None", StringComparison.OrdinalIgnoreCase))
            {
                var url = RetrieveLocaleIPSvrUrl(name);
                op.LocatorName = name;
                op.Url = string.IsNullOrEmpty(url) ? string.Empty : $"{url}{op.IP}";
                if (int.TryParse(RetrieveLocaleIPSvrCachePeriod(), out var period) && period > 0) op.Period = period * 60 * 1000;
            }
        }
    }
}

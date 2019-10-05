using Bootstrap.Security;
using Bootstrap.Security.DataAccess;
using Longbow.Cache;
using Longbow.Web;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Bootstrap.DataAccess
{
    /// <summary>
    /// 字典配置数据操作帮助类
    /// </summary>
    public static class DictHelper
    {
        /// <summary>
        /// 缓存索引，BootstrapAdmin后台清理缓存时使用
        /// </summary>
        public const string RetrieveDictsDataKey = DbHelper.RetrieveDictsDataKey;

        /// <summary>
        /// 缓存索引，字典分类数据缓存键值 值为 DictHelper-RetrieveDictsCategory
        /// </summary>
        public const string RetrieveCategoryDataKey = "DictHelper-RetrieveDictsCategory";

        /// <summary>
        /// 获得所有字典项配置数据集合方法 内部使用了缓存，缓存值 BootstrapMenu-RetrieveMenus
        /// </summary>
        /// <returns></returns>
        public static IEnumerable<BootstrapDict> RetrieveDicts() => CacheManager.GetOrAdd(RetrieveDictsDataKey, key => DbContextManager.Create<Dict>()?.RetrieveDicts());

        private static IEnumerable<BootstrapDict> RetrieveProtectedDicts() => RetrieveDicts().Where(d => d.Define == 0 || d.Category == "测试平台");

        /// <summary>
        /// 获取网站 logo 小图标
        /// </summary>
        /// <param name="appId"></param>
        /// <returns></returns>
        public static string RetrieveWebLogo(string appId)
        {
            // 获取应用程序 logo
            var ditcs = RetrieveDicts();
            var platName = ditcs.FirstOrDefault(d => d.Category == "应用程序" && d.Code == appId)?.Name;
            return ditcs.FirstOrDefault(d => d.Category == platName && d.Name == "网站图标")?.Code ?? $"~/favicon.ico";
        }

        /// <summary>
        /// 删除字典中的数据
        /// </summary>
        /// <param name="value">需要删除的IDs</param>
        /// <returns></returns>
        public static bool Delete(IEnumerable<string> value)
        {
            if (!value.Any()) return true;

            // 禁止删除系统数据与测试平台数据
            if (RetrieveSystemModel() && RetrieveProtectedDicts().Any(d => value.Any(v => v == d.Id))) return true;
            var ret = DbContextManager.Create<Dict>().Delete(value);
            CacheCleanUtility.ClearCache(dictIds: value);
            return ret;
        }

        /// <summary>
        /// 保存新建/更新的字典信息
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        public static bool Save(BootstrapDict p)
        {
            if (RetrieveSystemModel() && !string.IsNullOrEmpty(p.Id) && RetrieveProtectedDicts().Any(m => m.Id == p.Id)) return true;

            var ret = DbContextManager.Create<Dict>().Save(p);
            if (ret) CacheCleanUtility.ClearCache(dictIds: new List<string>());
            return ret;
        }

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
                op.Locator = string.IsNullOrEmpty(url) ? null : DefaultIPLocatorProvider.CreateLocator(name);
                op.Url = string.IsNullOrEmpty(url) ? string.Empty : $"{url}{op.IP}";
                if (int.TryParse(RetrieveLocaleIPSvrCachePeriod(), out var period) && period > 0) op.Period = period * 60 * 1000;
            }
        }

        /// <summary>
        /// 保存网站个性化设置
        /// </summary>
        /// <param name="dict"></param>
        /// <returns></returns>
        public static bool SaveSettings(BootstrapDict dict)
        {
            var ret = DbContextManager.Create<Dict>().SaveSettings(dict);
            if (ret) CacheCleanUtility.ClearCache(dictIds: new List<string>());
            return ret;
        }

        /// <summary>
        /// 获取字典分类名称
        /// </summary>
        /// <returns></returns>
        public static IEnumerable<string> RetrieveCategories() => CacheManager.GetOrAdd(RetrieveCategoryDataKey, key => DbContextManager.Create<Dict>().RetrieveCategories());

        /// <summary>
        /// 获取站点 Title 配置信息
        /// </summary>
        /// <param name="appId"></param>
        /// <returns></returns>
        public static string RetrieveWebTitle(string appId = "0") => DbContextManager.Create<Dict>().RetrieveWebTitle(appId);

        /// <summary>
        /// 获取站点 Footer 配置信息
        /// </summary>
        /// <param name="appId"></param>
        /// <returns></returns>
        public static string RetrieveWebFooter(string appId = "0") => DbContextManager.Create<Dict>().RetrieveWebFooter(appId);

        /// <summary>
        /// 获得系统中配置的可以使用的网站样式
        /// </summary>
        /// <returns></returns>
        public static IEnumerable<BootstrapDict> RetrieveThemes() => DbContextManager.Create<Dict>().RetrieveThemes();

        /// <summary>
        /// 获得网站设置中的当前样式
        /// </summary>
        /// <returns></returns>
        public static string RetrieveActiveTheme() => DbContextManager.Create<Dict>().RetrieveActiveTheme();

        /// <summary>
        /// 获取头像路径
        /// </summary>
        /// <returns></returns>
        public static string RetrieveIconFolderPath() => DbContextManager.Create<Dict>().RetrieveIconFolderPath();

        /// <summary>
        /// 获得默认的前台首页地址，默认为 ~/Home/Index
        /// </summary>
        /// <param name="appCode">应用程序编码</param>
        /// <returns></returns>
        public static string RetrieveHomeUrl(string appCode) => DbContextManager.Create<Dict>().RetrieveHomeUrl(appCode);

        /// <summary>
        /// 获取所有应用程序数据方法
        /// </summary>
        /// <returns></returns>
        public static IEnumerable<KeyValuePair<string, string>> RetrieveApps() => DbContextManager.Create<Dict>().RetrieveApps();

        /// <summary>
        /// 程序异常时长 默认 1 个月
        /// </summary>
        /// <returns></returns>
        public static int RetrieveExceptionsLogPeriod() => DbContextManager.Create<Dict>().RetrieveExceptionsLogPeriod();

        /// <summary>
        /// 获得操作日志保留时长 默认 12 个月
        /// </summary>
        /// <returns></returns>
        public static int RetrieveLogsPeriod() => DbContextManager.Create<Dict>().RetrieveLogsPeriod();

        /// <summary>
        /// 获得登录日志保留时长 默认 12 个月
        /// </summary>
        /// <returns></returns>
        public static int RetrieveLoginLogsPeriod() => DbContextManager.Create<Dict>().RetrieveLoginLogsPeriod();

        /// <summary>
        /// 获取登录认证Cookie保留时长 默认 7 天
        /// </summary>
        /// <returns></returns>
        public static int RetrieveCookieExpiresPeriod() => DbContextManager.Create<Dict>().RetrieveCookieExpiresPeriod();

        /// <summary>
        /// 获取 IP 地址位置查询服务名称
        /// </summary>
        /// <returns></returns>
        public static string RetrieveLocaleIPSvr() => DbContextManager.Create<Dict>().RetrieveLocaleIPSvr();

        /// <summary>
        /// 通过 IP 地理位置查询服务名称获得请求地址方法
        /// </summary>
        /// <param name="ipSvr">ip地址请求服务名称</param>
        /// <returns></returns>
        public static string RetrieveLocaleIPSvrUrl(string ipSvr) => DbContextManager.Create<Dict>().RetrieveLocaleIPSvrUrl(ipSvr);

        /// <summary>
        /// 获取 IP 地理位置查询服务缓存时长
        /// </summary>
        /// <returns></returns>
        public static string RetrieveLocaleIPSvrCachePeriod() => DbContextManager.Create<Dict>().RetrieveLocaleIPSvrCachePeriod();

        /// <summary>
        /// 访问日志保留时长 默认 1 个月
        /// </summary>
        /// <returns></returns>
        public static int RetrieveAccessLogPeriod() => DbContextManager.Create<Dict>().RetrieveAccessLogPeriod();

        /// <summary>
        /// 获得 是否为演示系统 默认为 false 不是演示系统
        /// </summary>
        /// <returns></returns>
        public static bool RetrieveSystemModel() => DbContextManager.Create<Dict>()?.RetrieveSystemModel() ?? true;

        /// <summary>
        /// 获得验证码图床地址
        /// </summary>
        /// <returns></returns>
        public static string RetrieveImagesLibUrl() => DbContextManager.Create<Dict>().RetrieveImagesLibUrl();

        /// <summary>
        /// 获得数据区卡片标题是否显示
        /// </summary>
        /// <returns></returns>
        public static bool RetrieveCardTitleStatus() => DbContextManager.Create<Dict>().RetrieveCardTitleStatus();

        /// <summary>
        /// 获得侧边栏状态 未真时显示
        /// </summary>
        /// <returns></returns>
        public static bool RetrieveSidebarStatus() => DbContextManager.Create<Dict>().RetrieveSidebarStatus();

        /// <summary>
        /// 获得是否允许短信验证码登录
        /// </summary>
        /// <returns></returns>
        public static bool RetrieveMobileLogin() => DbContextManager.Create<Dict>()?.RetrieveMobileLogin() ?? false;

        /// <summary>
        /// 获得是否允许 OAuth 认证登录
        /// </summary>
        /// <returns></returns>
        public static bool RetrieveOAuthLogin() => DbContextManager.Create<Dict>()?.RetrieveOAuthLogin() ?? false;

        /// <summary>
        /// 获得自动锁屏时长 默认 30 秒
        /// </summary>
        /// <returns></returns>
        public static int RetrieveAutoLockScreenPeriod() => DbContextManager.Create<Dict>()?.RetrieveAutoLockScreenPeriod() ?? 30;

        /// <summary>
        /// 获得自动锁屏 默认关闭
        /// </summary>
        /// <returns></returns>
        public static bool RetrieveAutoLockScreen() => DbContextManager.Create<Dict>()?.RetrieveAutoLockScreen() ?? false;
    }
}

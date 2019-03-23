using Bootstrap.Security;
using Longbow.Cache;
using Longbow.Data;
using System.Collections.Generic;

namespace Bootstrap.DataAccess
{
    /// <summary>
    /// 
    /// </summary>
    public static class DictHelper
    {
        /// <summary>
        /// 
        /// </summary>
        /// <summary>
        /// 缓存索引，BootstrapAdmin后台清理缓存时使用
        /// </summary>
        public const string RetrieveDictsDataKey = "BootstrapDict-RetrieveDicts";

        /// <summary>
        /// 
        /// </summary>
        public const string RetrieveCategoryDataKey = "DictHelper-RetrieveDictsCategory";

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static IEnumerable<BootstrapDict> RetrieveDicts() => CacheManager.GetOrAdd(RetrieveDictsDataKey, key => DbContextManager.Create<Dict>().RetrieveDicts());

        /// <summary>
        /// 删除字典中的数据
        /// </summary>
        /// <param name="value">需要删除的IDs</param>
        /// <returns></returns>
        public static bool Delete(IEnumerable<string> value)
        {
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
            var ret = DbContextManager.Create<Dict>().Save(p);
            if (ret) CacheCleanUtility.ClearCache(dictIds: new List<string>());
            return ret;
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
        /// 
        /// </summary>
        /// <returns></returns>
        public static string RetrieveWebTitle() => DbContextManager.Create<Dict>().RetrieveWebTitle();

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static string RetrieveWebFooter() => DbContextManager.Create<Dict>().RetrieveWebFooter();

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
        /// 获得默认的前台首页地址，默认为~/Home/Index
        /// </summary>
        /// <param name="appCode">应用程序编码</param>
        /// <returns></returns>
        public static string RetrieveHomeUrl(string appCode) => DbContextManager.Create<Dict>().RetrieveHomeUrl(appCode);

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static IEnumerable<KeyValuePair<string, string>> RetrieveApps() => DbContextManager.Create<Dict>().RetrieveApps();

        /// <summary>
        /// 程序异常时长 默认1月
        /// </summary>
        /// <returns></returns>
        public static int RetrieveExceptionsLogPeriod() => DbContextManager.Create<Dict>().RetrieveExceptionsLogPeriod();

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static int RetrieveLogsPeriod() => DbContextManager.Create<Dict>().RetrieveLogsPeriod();

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static int RetrieveLoginLogsPeriod() => DbContextManager.Create<Dict>().RetrieveLoginLogsPeriod();

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static int RetrieveCookieExpiresPeriod() => DbContextManager.Create<Dict>().RetrieveCookieExpiresPeriod();

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static int RetrieveLocaleIP() => DbContextManager.Create<Dict>().RetrieveLocaleIP();

        /// <summary>
        /// 访问日志保留时长 默认一个月
        /// </summary>
        /// <returns></returns>
        public static int RetrieveAccessLogPeriod() => DbContextManager.Create<Dict>().RetrieveAccessLogPeriod();
    }
}

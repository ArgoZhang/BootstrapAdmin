using Bootstrap.Security;
using Bootstrap.Security.DataAccess;
using Longbow.Cache;
using Longbow.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Bootstrap.Client.DataAccess
{
    /// <summary>
    /// 
    /// </summary>
    public static class DictHelper
    {
        /// <summary>
        /// 缓存索引，BootstrapAdmin后台清理缓存时使用
        /// </summary>
        public const string RetrieveDictsDataKey = "BootstrapDict-RetrieveDicts";

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static string RetrieveProfilesUrl()
        {
            return RetrieveAppName("个人中心地址");
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static string RetrieveSettingsUrl()
        {
            return RetrieveAppName("系统设置地址");
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static string RetrieveTitle()
        {
            return RetrieveAppName("网站标题");
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static string RetrieveFooter()
        {
            return RetrieveAppName("网站页脚");
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        private static IEnumerable<BootstrapDict> RetrieveDicts() => CacheManager.GetOrAdd(RetrieveDictsDataKey, key => DbHelper.RetrieveDicts());

        private static string RetrieveAppName(string name, string defaultValue = "未设置")
        {
            var dicts = RetrieveDicts();
            var platName = dicts.FirstOrDefault(d => d.Category == "应用程序" && d.Code == ConfigurationManager.AppSettings["AppId"])?.Name;
            return dicts.FirstOrDefault(d => d.Category == platName && d.Name == name)?.Code ?? $"{name}{defaultValue}";
        }

        /// <summary>
        /// 获得网站设置中的当前样式
        /// </summary>
        /// <returns></returns>
        public static string RetrieveActiveTheme()
        {
            var theme = RetrieveDicts().Where(d => d.Name == "使用样式" && d.Category == "当前样式" && d.Define == 0).FirstOrDefault()?.Code;
            return theme == null ? string.Empty : theme.Equals("site.css", StringComparison.OrdinalIgnoreCase) ? string.Empty : theme;
        }

        /// <summary>
        /// 获取头像路径
        /// </summary>
        /// <returns></returns>
        public static string RetrieveIconFolderPath() => (RetrieveDicts().FirstOrDefault(d => d.Name == "头像路径" && d.Category == "头像地址" && d.Define == 0) ?? new BootstrapDict() { Code = "~/images/uploader/" }).Code;

        /// <summary>
        /// 获取验证码图床
        /// </summary>
        /// <returns></returns>
        public static string RetrieveImagesLibUrl() => RetrieveDicts().FirstOrDefault(d => d.Name == "验证码图床" && d.Category == "系统设置" && d.Define == 0)?.Code ?? "http://images.sdgxgz.com/";
    }
}

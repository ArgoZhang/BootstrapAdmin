using Bootstrap.Security;
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
        private static IEnumerable<BootstrapDict> RetrieveDicts() => BAHelper.RetrieveDicts();

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
    }
}

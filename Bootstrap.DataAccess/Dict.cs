using Bootstrap.Security;
using Bootstrap.Security.DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Bootstrap.DataAccess
{
    /// <summary>
    /// 
    /// </summary>
    public class Dict : BootstrapDict
    {
        /// <summary>
        /// 删除字典中的数据
        /// </summary>
        /// <param name="value">需要删除的IDs</param>
        /// <returns></returns>
        public virtual bool Delete(IEnumerable<string> value)
        {
            if (!value.Any()) return true;
            var ids = string.Join(",", value);
            string sql = $"where ID in ({ids})";
            DbManager.Create().Delete<BootstrapDict>(sql);
            return true;
        }

        /// <summary>
        /// 保存新建/更新的字典信息
        /// </summary>
        /// <param name="dict"></param>
        /// <returns></returns>
        public virtual bool Save(BootstrapDict dict)
        {
            if (dict.Category.Length > 50) dict.Category = dict.Category.Substring(0, 50);
            if (dict.Name.Length > 50) dict.Name = dict.Name.Substring(0, 50);
            if (dict.Code.Length > 50) dict.Code = dict.Code.Substring(0, 50);

            DbManager.Create().Save(dict);
            return true;
        }

        /// <summary>
        /// 保存网站个性化设置
        /// </summary>
        /// <param name="dict"></param>
        /// <returns></returns>
        public virtual bool SaveSettings(BootstrapDict dict)
        {
            DbManager.Create().Update<BootstrapDict>("set Code = @Code where Category = @Category and Name = @Name", dict);
            return true;
        }

        /// <summary>
        /// 获取字典分类名称
        /// </summary>
        /// <returns></returns>
        public virtual IEnumerable<string> RetrieveCategories() => DictHelper.RetrieveDicts().Select(d => d.Category).Distinct();

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public virtual string RetrieveWebTitle() => (DictHelper.RetrieveDicts().FirstOrDefault(d => d.Name == "网站标题" && d.Category == "网站设置" && d.Define == 0) ?? new BootstrapDict() { Code = "后台管理系统" }).Code;

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public virtual string RetrieveWebFooter() => (DictHelper.RetrieveDicts().FirstOrDefault(d => d.Name == "网站页脚" && d.Category == "网站设置" && d.Define == 0) ?? new BootstrapDict() { Code = "2016 © 通用后台管理系统" }).Code;

        /// <summary>
        /// 获得系统中配置的可以使用的网站样式
        /// </summary>
        /// <returns></returns>
        public virtual IEnumerable<BootstrapDict> RetrieveThemes() => DictHelper.RetrieveDicts().Where(d => d.Category == "网站样式");

        /// <summary>
        /// 获得网站设置中的当前样式
        /// </summary>
        /// <returns></returns>
        public virtual string RetrieveActiveTheme()
        {
            var theme = DictHelper.RetrieveDicts().FirstOrDefault(d => d.Name == "使用样式" && d.Category == "当前样式" && d.Define == 0);
            return theme == null ? string.Empty : (theme.Code.Equals("site.css", StringComparison.OrdinalIgnoreCase) ? string.Empty : theme.Code);
        }

        /// <summary>
        /// 获取头像路径
        /// </summary>
        /// <returns></returns>
        public virtual string RetrieveIconFolderPath() => (DictHelper.RetrieveDicts().FirstOrDefault(d => d.Name == "头像路径" && d.Category == "头像地址" && d.Define == 0) ?? new BootstrapDict() { Code = "~/images/uploader/" }).Code;

        /// <summary>
        /// 获得默认的前台首页地址，默认为~/Home/Index
        /// </summary>
        /// <param name="appCode"></param>
        /// <returns></returns>
        public virtual string RetrieveHomeUrl(string appCode)
        {
            // https://gitee.com/LongbowEnterprise/dashboard/issues?id=IS0WK
            var url = "~/Home/Index";
            var dicts = DictHelper.RetrieveDicts();
            if (appCode != "0")
            {
                var appUrl = dicts.FirstOrDefault(d => d.Name.Equals(appCode, StringComparison.OrdinalIgnoreCase) && d.Category == "应用首页" && d.Define == 0)?.Code;
                if (!string.IsNullOrEmpty(appUrl))
                {
                    url = appUrl;
                    return url;
                }
            }
            var defaultUrl = dicts.FirstOrDefault(d => d.Name == "前台首页" && d.Category == "网站设置" && d.Define == 0)?.Code;
            if (!string.IsNullOrEmpty(defaultUrl)) url = defaultUrl;
            return url;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public virtual IEnumerable<KeyValuePair<string, string>> RetrieveApps() => DictHelper.RetrieveDicts().Where(d => d.Category == "应用程序" && d.Define == 0).Select(d => new KeyValuePair<string, string>(d.Code, d.Name)).OrderBy(d => d.Key);

        /// <summary>
        /// 通过数据库获得所有字典表配置信息，缓存Key=DictHelper-RetrieveDicts
        /// </summary>
        /// <returns></returns>
        public virtual IEnumerable<BootstrapDict> RetrieveDicts() => DbHelper.RetrieveDicts();
    }
}

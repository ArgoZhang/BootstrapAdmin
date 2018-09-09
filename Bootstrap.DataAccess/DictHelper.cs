using Bootstrap.Security;
using Longbow.Cache;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Globalization;
using System.Linq;

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
        private const string RetrieveCategoryDataKey = "DictHelper-RetrieveDictsCategory";
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static IEnumerable<BootstrapDict> RetrieveDicts()
        {
            return BootstrapDict.RetrieveDicts();
        }
        /// <summary>
        /// 删除字典中的数据
        /// </summary>
        /// <param name="value">需要删除的IDs</param>
        /// <returns></returns>
        public static bool DeleteDict(IEnumerable<int> value)
        {
            var ret = false;
            var ids = string.Join(",", value);
            string sql = string.Format(CultureInfo.InvariantCulture, "Delete from Dicts where ID in ({0})", ids);
            using (DbCommand cmd = DBAccessManager.SqlDBAccess.CreateCommand(CommandType.Text, sql))
            {
                ret = DBAccessManager.SqlDBAccess.ExecuteNonQuery(cmd) == value.Count();
                CacheCleanUtility.ClearCache(dictIds: ids);
            }
            return ret;
        }

        /// <summary>
        /// 保存新建/更新的字典信息
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        public static bool SaveDict(BootstrapDict p)
        {
            bool ret = false;
            if (p.Category.Length > 50) p.Category = p.Category.Substring(0, 50);
            if (p.Name.Length > 50) p.Name = p.Name.Substring(0, 50);
            if (p.Code.Length > 50) p.Code = p.Code.Substring(0, 50);
            string sql = p.Id == 0 ?
                "Insert Into Dicts (Category, Name, Code ,Define) Values (@Category, @Name, @Code, @Define)" :
                "Update Dicts set Category = @Category, Name = @Name, Code = @Code, Define = @Define where ID = @ID";
            using (DbCommand cmd = DBAccessManager.SqlDBAccess.CreateCommand(CommandType.Text, sql))
            {
                cmd.Parameters.Add(DBAccessManager.SqlDBAccess.CreateParameter("@ID", p.Id));
                cmd.Parameters.Add(DBAccessManager.SqlDBAccess.CreateParameter("@Category", p.Category));
                cmd.Parameters.Add(DBAccessManager.SqlDBAccess.CreateParameter("@Name", p.Name));
                cmd.Parameters.Add(DBAccessManager.SqlDBAccess.CreateParameter("@Code", p.Code));
                cmd.Parameters.Add(DBAccessManager.SqlDBAccess.CreateParameter("@Define", p.Define));
                ret = DBAccessManager.SqlDBAccess.ExecuteNonQuery(cmd) == 1;
            }
            CacheCleanUtility.ClearCache(dictIds: p.Id == 0 ? string.Empty : p.Id.ToString());
            return ret;
        }
        /// <summary>
        /// 保存网站个性化设置
        /// </summary>
        /// <param name="name"></param>
        /// <param name="code"></param>
        /// <param name="category"></param>
        /// <returns></returns>
        public static bool SaveSettings(BootstrapDict dict)
        {
            var ret = false;
            string sql = "Update Dicts set Code = @Code where Category = @Category and Name = @Name";
            using (DbCommand cmd = DBAccessManager.SqlDBAccess.CreateCommand(CommandType.Text, sql))
            {
                cmd.Parameters.Add(DBAccessManager.SqlDBAccess.CreateParameter("@Name", dict.Name));
                cmd.Parameters.Add(DBAccessManager.SqlDBAccess.CreateParameter("@Code", dict.Code));
                cmd.Parameters.Add(DBAccessManager.SqlDBAccess.CreateParameter("@Category", dict.Category));
                ret = DBAccessManager.SqlDBAccess.ExecuteNonQuery(cmd) == 1;
            }
            CacheCleanUtility.ClearCache(dictIds: string.Empty);
            return ret;
        }
        /// <summary>
        /// 获取字典分类名称
        /// </summary>
        /// <returns></returns>
        public static IEnumerable<string> RetrieveCategories()
        {
            return CacheManager.GetOrAdd(RetrieveCategoryDataKey, key =>
            {
                var ret = new List<string>();
                string sql = "select distinct Category from Dicts";
                DbCommand cmd = DBAccessManager.SqlDBAccess.CreateCommand(CommandType.Text, sql);
                using (DbDataReader reader = DBAccessManager.SqlDBAccess.ExecuteReader(cmd))
                {
                    while (reader.Read())
                    {
                        ret.Add((string)reader[0]);
                    }
                }
                return ret;
            });
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static string RetrieveWebTitle()
        {
            var settings = RetrieveDicts();
            return (settings.FirstOrDefault(d => d.Name == "网站标题" && d.Category == "网站设置" && d.Define == 0) ?? new BootstrapDict() { Code = "后台管理系统" }).Code;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static string RetrieveWebFooter()
        {
            var settings = RetrieveDicts();
            return (settings.FirstOrDefault(d => d.Name == "网站页脚" && d.Category == "网站设置" && d.Define == 0) ?? new BootstrapDict() { Code = "2016 © 通用后台管理系统" }).Code;
        }
        /// <summary>
        /// 获得系统中配置的可以使用的网站样式
        /// </summary>
        /// <returns></returns>
        public static IEnumerable<BootstrapDict> RetrieveThemes()
        {
            var data = RetrieveDicts();
            return data.Where(d => d.Category == "网站样式");
        }
        /// <summary>
        /// 获得网站设置中的当前样式
        /// </summary>
        /// <returns></returns>
        public static string RetrieveActiveTheme()
        {
            var data = RetrieveDicts();
            var theme = data.Where(d => d.Name == "使用样式" && d.Category == "当前样式" && d.Define == 0).FirstOrDefault();
            return theme == null ? string.Empty : (theme.Code.Equals("site.css", StringComparison.OrdinalIgnoreCase) ? string.Empty : theme.Code);
        }
        /// <summary>
        /// 获取头像路径
        /// </summary>
        /// <returns></returns>
        public static BootstrapDict RetrieveIconFolderPath()
        {
            var data = RetrieveDicts();
            return data.FirstOrDefault(d => d.Name == "头像路径" && d.Category == "头像地址" && d.Define == 0) ?? new BootstrapDict() { Code = "~/images/uploader/" };
        }
        /// <summary>
        /// 获得默认的前台首页地址，默认为~/Home/Index
        /// </summary>
        /// <returns></returns>
        public static string RetrieveHomeUrl()
        {
            var settings = RetrieveDicts();
            return (settings.FirstOrDefault(d => d.Name == "前台首页" && d.Category == "网站设置" && d.Define == 0) ?? new BootstrapDict() { Code = "~/Home/Index" }).Code;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static IEnumerable<KeyValuePair<string, string>> RetrieveApps()
        {
            var settings = RetrieveDicts();
            return settings.Where(d => d.Category == "应用程序" && d.Define == 0).Select(d => new KeyValuePair<string, string>(d.Code, d.Name)).OrderBy(d => d.Key);
        }
    }
}

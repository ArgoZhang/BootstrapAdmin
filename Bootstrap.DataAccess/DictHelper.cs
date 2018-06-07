using Bootstrap.Security;
using Longbow.Cache;
using Longbow.Logging;
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
        /// <param name="ids">需要删除的IDs</param>
        /// <returns></returns>
        public static bool DeleteDict(string ids)
        {
            if (string.IsNullOrEmpty(ids) || ids.Contains("'")) return false;
            var ret = false;
            try
            {
                string sql = string.Format(CultureInfo.InvariantCulture, "Delete from Dicts where ID in ({0})", ids);
                using (DbCommand cmd = DBAccessManager.SqlDBAccess.CreateCommand(CommandType.Text, sql))
                {
                    DBAccessManager.SqlDBAccess.ExecuteNonQuery(cmd);
                    CacheCleanUtility.ClearCache(dictIds: ids);
                    ret = true;
                }
            }
            catch (Exception ex)
            {
                ExceptionManager.Publish(ex);
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
            try
            {
                using (DbCommand cmd = DBAccessManager.SqlDBAccess.CreateCommand(CommandType.Text, sql))
                {
                    cmd.Parameters.Add(DBAccessManager.SqlDBAccess.CreateParameter("@ID", p.Id));
                    cmd.Parameters.Add(DBAccessManager.SqlDBAccess.CreateParameter("@Category", p.Category));
                    cmd.Parameters.Add(DBAccessManager.SqlDBAccess.CreateParameter("@Name", p.Name));
                    cmd.Parameters.Add(DBAccessManager.SqlDBAccess.CreateParameter("@Code", p.Code));
                    cmd.Parameters.Add(DBAccessManager.SqlDBAccess.CreateParameter("@Define", p.Define));
                    DBAccessManager.SqlDBAccess.ExecuteNonQuery(cmd);
                }
                ret = true;
                CacheCleanUtility.ClearCache(dictIds: p.Id == 0 ? string.Empty : p.Id.ToString());
            }
            catch (DbException ex)
            {
                ExceptionManager.Publish(ex);
            }
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
            try
            {
                using (DbCommand cmd = DBAccessManager.SqlDBAccess.CreateCommand(CommandType.Text, sql))
                {
                    cmd.Parameters.Add(DBAccessManager.SqlDBAccess.CreateParameter("@Name", dict.Name));
                    cmd.Parameters.Add(DBAccessManager.SqlDBAccess.CreateParameter("@Code", dict.Code));
                    cmd.Parameters.Add(DBAccessManager.SqlDBAccess.CreateParameter("@Category", dict.Category));
                    DBAccessManager.SqlDBAccess.ExecuteNonQuery(cmd);
                }
                CacheCleanUtility.ClearCache(dictIds: string.Empty);
                ret = true;
            }
            catch (DbException ex)
            {
                ExceptionManager.Publish(ex);
            }
            return ret;
        }
        /// <summary>
        /// 获取字典分类名称
        /// </summary>
        /// <returns></returns>
        public static IEnumerable<BootstrapDict> RetrieveCategories()
        {
            return CacheManager.GetOrAdd(RetrieveCategoryDataKey, key =>
            {
                var ret = new List<BootstrapDict>();
                string sql = "select distinct Category from Dicts";
                DbCommand cmd = DBAccessManager.SqlDBAccess.CreateCommand(CommandType.Text, sql);
                try
                {
                    using (DbDataReader reader = DBAccessManager.SqlDBAccess.ExecuteReader(cmd))
                    {
                        while (reader.Read())
                        {
                            ret.Add(new BootstrapDict() { Category = (string)reader[0] });
                        }
                    }
                }
                catch (Exception ex) { ExceptionManager.Publish(ex); }
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
        public static IEnumerable<BootstrapDict> RetrieveWebCss()
        {
            var data = RetrieveDicts();
            return data.Where(d => d.Category == "网站样式");
        }
        /// <summary>
        /// 获得网站设置中的当前样式
        /// </summary>
        /// <returns></returns>
        public static IEnumerable<BootstrapDict> RetrieveActiveCss()
        {
            var data = RetrieveDicts();
            return data.Where(d => d.Name == "使用样式" && d.Category == "当前样式" && d.Define == 0 && !d.Code.Equals("site.css", StringComparison.OrdinalIgnoreCase));
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

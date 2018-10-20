using Bootstrap.Security;
using Longbow.Cache;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Globalization;
using System.Linq;

namespace Bootstrap.DataAccess.SQLite
{
    /// <summary>
    /// 
    /// </summary>
    public class Dict : DataAccess.Dict
    {
        /// <summary>
        /// 删除字典中的数据
        /// </summary>
        /// <param name="value">需要删除的IDs</param>
        /// <returns></returns>
        public override bool DeleteDict(IEnumerable<int> value)
        {
            var ret = false;
            var ids = string.Join(",", value);
            string sql = string.Format(CultureInfo.InvariantCulture, "Delete from Dicts where ID in ({0})", ids);
            using (DbCommand cmd = DbAccessManager.DBAccess.CreateCommand(CommandType.Text, sql))
            {
                ret = DbAccessManager.DBAccess.ExecuteNonQuery(cmd) == value.Count();
                CacheCleanUtility.ClearCache(dictIds: ids);
            }
            return ret;
        }

        /// <summary>
        /// 保存新建/更新的字典信息
        /// </summary>
        /// <param name="dict"></param>
        /// <returns></returns>
        public override bool SaveDict(BootstrapDict dict)
        {
            bool ret = false;
            if (dict.Category.Length > 50) dict.Category = dict.Category.Substring(0, 50);
            if (dict.Name.Length > 50) dict.Name = dict.Name.Substring(0, 50);
            if (dict.Code.Length > 50) dict.Code = dict.Code.Substring(0, 50);
            string sql = dict.Id == 0 ?
                "Insert Into Dicts (Category, Name, Code ,Define) Values (@Category, @Name, @Code, @Define)" :
                "Update Dicts set Category = @Category, Name = @Name, Code = @Code, Define = @Define where ID = @ID";
            using (DbCommand cmd = DbAccessManager.DBAccess.CreateCommand(CommandType.Text, sql))
            {
                cmd.Parameters.Add(DbAccessManager.DBAccess.CreateParameter("@ID", dict.Id));
                cmd.Parameters.Add(DbAccessManager.DBAccess.CreateParameter("@Category", dict.Category));
                cmd.Parameters.Add(DbAccessManager.DBAccess.CreateParameter("@Name", dict.Name));
                cmd.Parameters.Add(DbAccessManager.DBAccess.CreateParameter("@Code", dict.Code));
                cmd.Parameters.Add(DbAccessManager.DBAccess.CreateParameter("@Define", dict.Define));
                ret = DbAccessManager.DBAccess.ExecuteNonQuery(cmd) == 1;
            }
            CacheCleanUtility.ClearCache(dictIds: dict.Id == 0 ? string.Empty : dict.Id.ToString());
            return ret;
        }
        /// <summary>
        /// 保存网站个性化设置
        /// </summary>
        /// <param name="name"></param>
        /// <param name="code"></param>
        /// <param name="category"></param>
        /// <returns></returns>
        public override bool SaveSettings(BootstrapDict dict)
        {
            var ret = false;
            string sql = "Update Dicts set Code = @Code where Category = @Category and Name = @Name";
            using (DbCommand cmd = DbAccessManager.DBAccess.CreateCommand(CommandType.Text, sql))
            {
                cmd.Parameters.Add(DbAccessManager.DBAccess.CreateParameter("@Name", dict.Name));
                cmd.Parameters.Add(DbAccessManager.DBAccess.CreateParameter("@Code", dict.Code));
                cmd.Parameters.Add(DbAccessManager.DBAccess.CreateParameter("@Category", dict.Category));
                ret = DbAccessManager.DBAccess.ExecuteNonQuery(cmd) == 1;
            }
            CacheCleanUtility.ClearCache(dictIds: string.Empty);
            return ret;
        }
        /// <summary>
        /// 获取字典分类名称
        /// </summary>
        /// <returns></returns>
        public override IEnumerable<string> RetrieveCategories()
        {
            return CacheManager.GetOrAdd(RetrieveCategoryDataKey, key =>
            {
                var ret = new List<string>();
                string sql = "select distinct Category from Dicts";
                DbCommand cmd = DbAccessManager.DBAccess.CreateCommand(CommandType.Text, sql);
                using (DbDataReader reader = DbAccessManager.DBAccess.ExecuteReader(cmd))
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
        public override string RetrieveWebTitle()
        {
            var settings = RetrieveDicts();
            return (settings.FirstOrDefault(d => d.Name == "网站标题" && d.Category == "网站设置" && d.Define == 0) ?? new BootstrapDict() { Code = "后台管理系统" }).Code;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override string RetrieveWebFooter()
        {
            var settings = RetrieveDicts();
            return (settings.FirstOrDefault(d => d.Name == "网站页脚" && d.Category == "网站设置" && d.Define == 0) ?? new BootstrapDict() { Code = "2016 © 通用后台管理系统" }).Code;
        }
        /// <summary>
        /// 获得系统中配置的可以使用的网站样式
        /// </summary>
        /// <returns></returns>
        public override IEnumerable<BootstrapDict> RetrieveThemes()
        {
            var data = RetrieveDicts();
            return data.Where(d => d.Category == "网站样式");
        }
        /// <summary>
        /// 获得网站设置中的当前样式
        /// </summary>
        /// <returns></returns>
        public override string RetrieveActiveTheme()
        {
            var data = RetrieveDicts();
            var theme = data.Where(d => d.Name == "使用样式" && d.Category == "当前样式" && d.Define == 0).FirstOrDefault();
            return theme == null ? string.Empty : (theme.Code.Equals("site.css", StringComparison.OrdinalIgnoreCase) ? string.Empty : theme.Code);
        }
        /// <summary>
        /// 获取头像路径
        /// </summary>
        /// <returns></returns>
        public override BootstrapDict RetrieveIconFolderPath()
        {
            var data = RetrieveDicts();
            return data.FirstOrDefault(d => d.Name == "头像路径" && d.Category == "头像地址" && d.Define == 0) ?? new BootstrapDict() { Code = "~/images/uploader/" };
        }
        /// <summary>
        /// 获得默认的前台首页地址，默认为~/Home/Index
        /// </summary>
        /// <returns></returns>
        public override string RetrieveHomeUrl()
        {
            var settings = RetrieveDicts();
            return (settings.FirstOrDefault(d => d.Name == "前台首页" && d.Category == "网站设置" && d.Define == 0) ?? new BootstrapDict() { Code = "~/Home/Index" }).Code;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override IEnumerable<KeyValuePair<string, string>> RetrieveApps()
        {
            var settings = RetrieveDicts();
            return settings.Where(d => d.Category == "应用程序" && d.Define == 0).Select(d => new KeyValuePair<string, string>(d.Code, d.Name)).OrderBy(d => d.Key);
        }
    }
}

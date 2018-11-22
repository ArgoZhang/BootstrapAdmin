using Bootstrap.Security;
using Bootstrap.Security.DataAccess;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
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
        public virtual bool DeleteDict(IEnumerable<string> value)
        {
            var ret = false;
            var ids = string.Join(",", value);
            string sql = $"Delete from Dicts where ID in ({ids})";
            using (DbCommand cmd = DbAccessManager.DBAccess.CreateCommand(CommandType.Text, sql))
            {
                ret = DbAccessManager.DBAccess.ExecuteNonQuery(cmd) == value.Count();
            }
            return ret;
        }
        /// <summary>
        /// 保存新建/更新的字典信息
        /// </summary>
        /// <param name="dict"></param>
        /// <returns></returns>
        public virtual bool SaveDict(BootstrapDict dict)
        {
            bool ret = false;
            if (dict.Category.Length > 50) dict.Category = dict.Category.Substring(0, 50);
            if (dict.Name.Length > 50) dict.Name = dict.Name.Substring(0, 50);
            if (dict.Code.Length > 50) dict.Code = dict.Code.Substring(0, 50);
            string sql = string.IsNullOrEmpty(dict.Id) ?
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
            return ret;
        }
        /// <summary>
        /// 保存网站个性化设置
        /// </summary>
        /// <param name="name"></param>
        /// <param name="code"></param>
        /// <param name="category"></param>
        /// <returns></returns>
        public virtual bool SaveSettings(BootstrapDict dict)
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
            return ret;
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
            var theme = DictHelper.RetrieveDicts().Where(d => d.Name == "使用样式" && d.Category == "当前样式" && d.Define == 0).FirstOrDefault();
            return theme == null ? string.Empty : (theme.Code.Equals("site.css", StringComparison.OrdinalIgnoreCase) ? string.Empty : theme.Code);
        }
        /// <summary>
        /// 获取头像路径
        /// </summary>
        /// <returns></returns>
        public virtual BootstrapDict RetrieveIconFolderPath() => DictHelper.RetrieveDicts().FirstOrDefault(d => d.Name == "头像路径" && d.Category == "头像地址" && d.Define == 0) ?? new BootstrapDict() { Code = "~/images/uploader/" };
        /// <summary>
        /// 获得默认的前台首页地址，默认为~/Home/Index
        /// </summary>
        /// <returns></returns>
        public virtual string RetrieveHomeUrl() => (DictHelper.RetrieveDicts().FirstOrDefault(d => d.Name == "前台首页" && d.Category == "网站设置" && d.Define == 0) ?? new BootstrapDict() { Code = "~/Home/Index" }).Code;
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public virtual IEnumerable<KeyValuePair<string, string>> RetrieveApps() => DictHelper.RetrieveDicts().Where(d => d.Category == "应用程序" && d.Define == 0).Select(d => new KeyValuePair<string, string>(d.Code, d.Name)).OrderBy(d => d.Key);
        /// <summary>
        /// 通过数据库获得所有字典表配置信息，缓存Key=DictHelper-RetrieveDicts
        /// </summary>
        /// <param name="db">数据库连接实例</param>
        /// <returns></returns>
        public virtual IEnumerable<BootstrapDict> RetrieveDicts() => DbHelper.RetrieveDicts();
    }
}

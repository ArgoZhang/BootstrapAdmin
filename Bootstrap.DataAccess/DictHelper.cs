using Longbow.Caching;
using Longbow.Caching.Configuration;
using Longbow.ExceptionManagement;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Globalization;
using System.Linq;

namespace Bootstrap.DataAccess
{
    public static class DictHelper
    {
        internal const string RetrieveDictsDataKey = "DictHelper-RetrieveDicts";
        internal const string RetrieveWebSettingsDataKey = "DictHelper-RetrieveDictsWebSettings";
        /// <summary>
        /// 查询所有字典信息
        /// </summary>
        /// <param name="tId"></param>
        /// <returns></returns>
        public static IEnumerable<Dict> RetrieveDicts(int id = 0)
        {
            var ret = CacheManager.GetOrAdd(RetrieveDictsDataKey, CacheSection.RetrieveIntervalByKey(RetrieveDictsDataKey), key =>
            {
                string sql = "select ID, Category, Name, Code, Define, case Define when 0 then '系统使用' else '自定义' end DefineName from Dicts";
                List<Dict> Dicts = new List<Dict>();
                DbCommand cmd = DBAccessManager.SqlDBAccess.CreateCommand(CommandType.Text, sql);
                try
                {
                    using (DbDataReader reader = DBAccessManager.SqlDBAccess.ExecuteReader(cmd))
                    {
                        while (reader.Read())
                        {
                            Dicts.Add(new Dict()
                            {
                                ID = (int)reader[0],
                                Category = (string)reader[1],
                                Name = (string)reader[2],
                                Code = (string)reader[3],
                                Define = (int)reader[4],
                                DefineName = (string)reader[5]
                            });
                        }
                    }
                }
                catch (Exception ex) { ExceptionManager.Publish(ex); }
                return Dicts;
            }, CacheSection.RetrieveDescByKey(RetrieveDictsDataKey));
            return id == 0 ? ret : ret.Where(t => id == t.ID);
        }
        /// <summary>
        /// 删除字典中的数据
        /// </summary>
        /// <param name="ids">需要删除的IDs</param>
        /// <returns></returns>
        public static bool DeleteDict(string ids)
        {
            var ret = false;
            if (string.IsNullOrEmpty(ids) || ids.Contains("'")) return ret;
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
        public static bool SaveDict(Dict p)
        {
            if (p == null) throw new ArgumentNullException("p");
            bool ret = false;
            if (p.Category.Length > 50) p.Category.Substring(0, 50);
            if (p.Name.Length > 50) p.Name.Substring(0, 50);
            if (p.Code.Length > 50) p.Code.Substring(0, 50);
            string sql = p.ID == 0 ?
                "Insert Into Dicts (Category, Name, Code ,Define) Values (@Category, @Name, @Code, @Define)" :
                "Update Dicts set Category = @Category, Name = @Name, Code = @Code, Define = @Define where ID = @ID";
            try
            {
                using (DbCommand cmd = DBAccessManager.SqlDBAccess.CreateCommand(CommandType.Text, sql))
                {
                    cmd.Parameters.Add(DBAccessManager.SqlDBAccess.CreateParameter("@ID", p.ID, ParameterDirection.Input));
                    cmd.Parameters.Add(DBAccessManager.SqlDBAccess.CreateParameter("@Category", p.Category, ParameterDirection.Input));
                    cmd.Parameters.Add(DBAccessManager.SqlDBAccess.CreateParameter("@Name", p.Name, ParameterDirection.Input));
                    cmd.Parameters.Add(DBAccessManager.SqlDBAccess.CreateParameter("@Code", p.Code, ParameterDirection.Input));
                    cmd.Parameters.Add(DBAccessManager.SqlDBAccess.CreateParameter("@Define", p.Define, ParameterDirection.Input));
                    DBAccessManager.SqlDBAccess.ExecuteNonQuery(cmd);
                }
                ret = true;
                CacheCleanUtility.ClearCache(dictIds: p.ID == 0 ? "" : p.ID.ToString());
            }
            catch (DbException ex)
            {
                ExceptionManager.Publish(ex);
            }
            return ret;
        }
        public static IEnumerable<Dict> RetrieveWebSettings()
        {
            return CacheManager.GetOrAdd(RetrieveWebSettingsDataKey, CacheSection.RetrieveIntervalByKey(RetrieveWebSettingsDataKey), key =>
            {
                string sql = "select ID, Category, Name, Code, Define, case Define when 0 then '系统使用' else '用户自定义' end DefineName from Dicts where Category = N'网站设置' and Define = 0";
                List<Dict> Dicts = new List<Dict>();
                DbCommand cmd = DBAccessManager.SqlDBAccess.CreateCommand(CommandType.Text, sql);
                try
                {
                    using (DbDataReader reader = DBAccessManager.SqlDBAccess.ExecuteReader(cmd))
                    {
                        while (reader.Read())
                        {
                            Dicts.Add(new Dict()
                            {
                                ID = (int)reader[0],
                                Category = (string)reader[1],
                                Name = (string)reader[2],
                                Code = (string)reader[3],
                                Define = (int)reader[4],
                                DefineName = (string)reader[5]
                            });
                        }
                    }
                }
                catch (Exception ex) { ExceptionManager.Publish(ex); }
                return Dicts;
            }, CacheSection.RetrieveDescByKey(RetrieveWebSettingsDataKey));
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static string RetrieveWebTitle()
        {
            var settings = DictHelper.RetrieveWebSettings();
            return (settings.FirstOrDefault(d => d.Name == "网站标题") ?? new Dict() { Code = "后台管理系统" }).Code;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static string RetrieveWebFooter()
        {
            var settings = DictHelper.RetrieveWebSettings();
            return (settings.FirstOrDefault(d => d.Name == "网站页脚") ?? new Dict() { Code = "2016 © 通用后台管理系统" }).Code;
        }
        /// <summary>
        /// 保存网站个性化设置
        /// 2016-11-8
        /// </summary>
        /// <returns></returns>
        public static bool SaveProfiles(string type, string value)
        {
            string name="";
            string category="网站设置";
            switch(type)
            {
                case "sysName": name = "网站标题"; break;
                case "foot": name = "网站页脚"; break;
            }
            var settings = DictHelper.RetrieveWebSettings();
            var p = from ps in settings
                    where ps.Name.Equals(name)
                     select new
                     {
                         ID=ps.ID
                     };
                   
            var ret = false;
            string sql = "Update Dicts set Code = @Code where Category =@Category and Name=@Name";
            try
            {
                using (DbCommand cmd = DBAccessManager.SqlDBAccess.CreateCommand(CommandType.Text, sql))
                {
                    cmd.Parameters.Add(DBAccessManager.SqlDBAccess.CreateParameter("@Code", value, ParameterDirection.Input));
                    cmd.Parameters.Add(DBAccessManager.SqlDBAccess.CreateParameter("@Category", category, ParameterDirection.Input));
                    cmd.Parameters.Add(DBAccessManager.SqlDBAccess.CreateParameter("@Name", name, ParameterDirection.Input));     
                    DBAccessManager.SqlDBAccess.ExecuteNonQuery(cmd);
                }
                ret = true;

                foreach(var px in p)
                CacheCleanUtility.ClearCache(dictIds:px.ID.ToString());
            }
            catch (DbException ex)
            {
                ExceptionManager.Publish(ex);
            }
            return ret;
        }
       
   
    }
}

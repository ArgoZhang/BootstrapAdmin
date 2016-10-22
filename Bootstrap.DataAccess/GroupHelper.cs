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
    /// <summary>
    /// author:liuchun
    /// date:2016.10.22
    /// </summary>
    public static class GroupHelper
    {
        private const string GroupDataKey = "GroupData-CodeGroupHelper";
        /// <summary>
        /// 查询所有群组信息
        /// </summary>
        /// <param name="tId"></param>
        /// <returns></returns>
        public static IEnumerable<Group> RetrieveGroups(string tId = null)
        {
            string sql = "select * from Groups";
            var ret = CacheManager.GetOrAdd(GroupDataKey, CacheSection.RetrieveIntervalByKey(GroupDataKey), key =>
            {
                List<Group> Groups = new List<Group>();
                DbCommand cmd = DBAccessManager.SqlDBAccess.CreateCommand(CommandType.Text, sql);
                try
                {
                    using (DbDataReader reader = DBAccessManager.SqlDBAccess.ExecuteReader(cmd))
                    {
                        while (reader.Read())
                        {
                            Groups.Add(new Group()
                            {
                                ID = (int)reader[0],
                                GroupName = (string)reader[1],
                                Description = (string)reader[2]
                            });
                        }
                    }
                }
                catch (Exception ex) { ExceptionManager.Publish(ex); }
                return Groups;
            }, CacheSection.RetrieveDescByKey(GroupDataKey));
            return string.IsNullOrEmpty(tId) ? ret : ret.Where(t => tId.Equals(t.ID.ToString(), StringComparison.OrdinalIgnoreCase));
        }
        /// <summary>
        /// 删除群组信息
        /// </summary>
        /// <param name="ids"></param>
        public static void DeleteGroup(string ids)
        {
            if (string.IsNullOrEmpty(ids) || ids.Contains("'")) return;
            string sql = string.Format(CultureInfo.InvariantCulture, "Delete from Groups where ID in ({0})", ids);
            using (DbCommand cmd = DBAccessManager.SqlDBAccess.CreateCommand(CommandType.Text, sql))
            {
                DBAccessManager.SqlDBAccess.ExecuteNonQuery(cmd);
                ClearCache();
            }
        }
        /// <summary>
        /// 保存新建/更新的群组信息
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        public static bool SaveGroup(Group p)
        {
            if (p == null) throw new ArgumentNullException("p");
            bool ret = false;
            if (p.GroupName.Length > 50) p.GroupName.Substring(0, 50);
            if (p.Description.Length > 500) p.Description.Substring(0, 500);
            string sql = p.ID == 0 ?
                "Insert Into Groups (GroupName, Description) Values (@GroupName, @Description)" :
                "Update Groups set GroupName = @GroupName, Description = @Description where ID = @ID";
            try
            {
                using (DbCommand cmd = DBAccessManager.SqlDBAccess.CreateCommand(CommandType.Text, sql))
                {
                    cmd.Parameters.Add(DBAccessManager.SqlDBAccess.CreateParameter("@ID", p.ID, ParameterDirection.Input));
                    cmd.Parameters.Add(DBAccessManager.SqlDBAccess.CreateParameter("@GroupName", p.GroupName, ParameterDirection.Input));
                    cmd.Parameters.Add(DBAccessManager.SqlDBAccess.CreateParameter("@Description", p.Description, ParameterDirection.Input));
                    DBAccessManager.SqlDBAccess.ExecuteNonQuery(cmd);
                }
                ret = true;
                ClearCache();
            }
            catch (DbException ex)
            {
                ExceptionManager.Publish(ex);
            }
            return ret;
        }
        // 更新缓存
        private static void ClearCache()
        {
            CacheManager.Clear(key => key == GroupDataKey);
        }
    }
}


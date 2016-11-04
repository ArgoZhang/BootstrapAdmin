using Longbow.Caching;
using Longbow.Caching.Configuration;
using Longbow.Data;
using Longbow.ExceptionManagement;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
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
        private const string RetrieveGroupsDataKey = "GroupHelper-RetrieveGroups";
        internal const string RetrieveGroupsByUserIDDataKey = "GroupHelper-RetrieveGroupsByUserId";
        internal const string RetrieveGroupsByRoleIDDataKey = "GroupHelper-RetrieveGroupsByRoleId";
        /// <summary>
        /// 查询所有群组信息
        /// </summary>
        /// <param name="tId"></param>
        /// <returns></returns>
        public static IEnumerable<Group> RetrieveGroups(string tId = null)
        {
            var ret = CacheManager.GetOrAdd(RetrieveGroupsDataKey, CacheSection.RetrieveIntervalByKey(RetrieveGroupsDataKey), key =>
            {
                string sql = "select * from Groups";
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
            }, CacheSection.RetrieveDescByKey(RetrieveGroupsDataKey));
            return string.IsNullOrEmpty(tId) ? ret : ret.Where(t => tId.Equals(t.ID.ToString(), StringComparison.OrdinalIgnoreCase));
        }
        /// <summary>
        /// 删除群组信息
        /// </summary>
        /// <param name="ids"></param>
        public static bool DeleteGroup(string ids)
        {
            var ret = false;
            if (string.IsNullOrEmpty(ids) || ids.Contains("'")) return ret;
            try
            {
                string sql = string.Format(CultureInfo.InvariantCulture, "Delete from Groups where ID in ({0})", ids);
                using (DbCommand cmd = DBAccessManager.SqlDBAccess.CreateCommand(CommandType.Text, sql))
                {
                    DBAccessManager.SqlDBAccess.ExecuteNonQuery(cmd);
                    CacheManager.Clear(key => key == RetrieveGroupsDataKey);
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
                CacheManager.Clear(key => key == RetrieveGroupsDataKey);
            }
            catch (DbException ex)
            {
                ExceptionManager.Publish(ex);
            }
            return ret;
        }
        /// <summary>
        /// 根据用户查询部门信息
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public static IEnumerable<Group> RetrieveGroupsByUserId(int userId)
        {
            string key = string.Format("{0}-{1}", RetrieveGroupsByUserIDDataKey, userId);
            var ret = CacheManager.GetOrAdd(key, CacheSection.RetrieveIntervalByKey(RetrieveGroupsByUserIDDataKey), k =>
            {
                string sql = "select g.ID,g.GroupName,g.[Description],case ug.GroupID when g.ID then 'checked' else '' end [status] from Groups g left join UserGroup ug on g.ID=ug.GroupID and UserID=@UserID";
                List<Group> Groups = new List<Group>();
                DbCommand cmd = DBAccessManager.SqlDBAccess.CreateCommand(CommandType.Text, sql);
                cmd.Parameters.Add(DBAccessManager.SqlDBAccess.CreateParameter("@UserID", userId, ParameterDirection.Input));
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
                                Description = (string)reader[2],
                                Checked = (string)reader[3]
                            });
                        }
                    }
                }
                catch (Exception ex) { ExceptionManager.Publish(ex); }
                return Groups;
            }, CacheSection.RetrieveDescByKey(RetrieveGroupsByUserIDDataKey));
            return ret;
        }
        /// <summary>
        /// 保存用户部门关系
        /// </summary>
        /// <param name="id"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public static bool SaveGroupsByUserId(int id, string groupIds)
        {
            var ret = false;
            DataTable dt = new DataTable();
            dt.Columns.Add("UserID", typeof(int));
            dt.Columns.Add("GroupID", typeof(int));
            //判断用户是否选定角色
            if (!string.IsNullOrEmpty(groupIds)) groupIds.Split(',').ToList().ForEach(groupId => dt.Rows.Add(id, groupId));
            using (TransactionPackage transaction = DBAccessManager.SqlDBAccess.BeginTransaction())
            {
                try
                {
                    //删除用户部门表中该用户所有的部门关系
                    string sql = "delete from UserGroup where UserID=@UserID;";
                    using (DbCommand cmd = DBAccessManager.SqlDBAccess.CreateCommand(CommandType.Text, sql))
                    {
                        cmd.Parameters.Add(DBAccessManager.SqlDBAccess.CreateParameter("@UserID", id, ParameterDirection.Input));
                        DBAccessManager.SqlDBAccess.ExecuteNonQuery(cmd, transaction);

                        // insert batch data into config table
                        using (SqlBulkCopy bulk = new SqlBulkCopy((SqlConnection)transaction.Transaction.Connection, SqlBulkCopyOptions.Default, (SqlTransaction)transaction.Transaction))
                        {
                            bulk.BatchSize = 1000;
                            bulk.DestinationTableName = "UserGroup";
                            bulk.ColumnMappings.Add("UserID", "UserID");
                            bulk.ColumnMappings.Add("GroupID", "GroupID");
                            bulk.WriteToServer(dt);
                            transaction.CommitTransaction();
                        }
                    }
                    groupIds.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries).AsParallel()
.ForAll(g => CacheManager.Clear(key => key == string.Format("{0}-{1}", RetrieveGroupsByUserIDDataKey, id) || key == string.Format("{0}-{1}", UserHelper.RetrieveUsersByGroupIDDataKey, g)));
                    ret = true;
                }
                catch (Exception ex)
                {
                    ExceptionManager.Publish(ex);
                    transaction.RollbackTransaction();
                }
            }
            return ret;
        }
        /// <summary>
        /// 根据角色ID指派部门
        /// </summary>
        /// <param name="roleId"></param>
        /// <returns></returns>
        public static IEnumerable<Group> RetrieveGroupsByRoleId(int roleId)
        {
            string k = string.Format("{0}-{1}", RetrieveGroupsByRoleIDDataKey, roleId);
            return CacheManager.GetOrAdd(k, CacheSection.RetrieveIntervalByKey(RetrieveGroupsByRoleIDDataKey), key =>
            {
                List<Group> Groups = new List<Group>();
                string sql = "select g.ID,g.GroupName,g.[Description],case rg.GroupID when g.ID then 'checked' else '' end [status] from Groups g left join RoleGroup rg on g.ID=rg.GroupID and RoleID=@RoleID";
                DbCommand cmd = DBAccessManager.SqlDBAccess.CreateCommand(CommandType.Text, sql);
                cmd.Parameters.Add(DBAccessManager.SqlDBAccess.CreateParameter("@RoleID", roleId, ParameterDirection.Input));
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
                                Description = (string)reader[2],
                                Checked = (string)reader[3]
                            });
                        }
                    }
                }
                catch (Exception ex) { ExceptionManager.Publish(ex); }
                return Groups;
            }, CacheSection.RetrieveDescByKey(RetrieveGroupsByRoleIDDataKey));
        }
        /// <summary>
        /// 根据角色ID以及选定的部门ID，保到角色部门表
        /// </summary>
        /// <param name="roleId"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public static bool SaveGroupsByRoleId(int id, string groupIds)
        {
            bool ret = false;
            DataTable dt = new DataTable();
            dt.Columns.Add("GroupID", typeof(int));
            dt.Columns.Add("RoleID", typeof(int));
            if (!string.IsNullOrEmpty(groupIds)) groupIds.Split(',').ToList().ForEach(groupId => dt.Rows.Add(groupId, id));
            using (TransactionPackage transaction = DBAccessManager.SqlDBAccess.BeginTransaction())
            {
                try
                {
                    //删除角色部门表该角色所有的部门
                    string sql = "delete from RoleGroup where RoleID=@RoleID";
                    using (DbCommand cmd = DBAccessManager.SqlDBAccess.CreateCommand(CommandType.Text, sql))
                    {
                        cmd.Parameters.Add(DBAccessManager.SqlDBAccess.CreateParameter("@RoleID", id, ParameterDirection.Input));
                        DBAccessManager.SqlDBAccess.ExecuteNonQuery(cmd, transaction);
                        //批插入角色部门表
                        using (SqlBulkCopy bulk = new SqlBulkCopy((SqlConnection)transaction.Transaction.Connection, SqlBulkCopyOptions.Default, (SqlTransaction)transaction.Transaction))
                        {
                            bulk.BatchSize = 1000;
                            bulk.ColumnMappings.Add("GroupID", "GroupID");
                            bulk.ColumnMappings.Add("RoleID", "RoleID");
                            bulk.DestinationTableName = "RoleGroup";
                            bulk.WriteToServer(dt);
                            transaction.CommitTransaction();
                        }
                    }
                    groupIds.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries).AsParallel()
    .ForAll(g => CacheManager.Clear(key => key == string.Format("{0}-{1}", RetrieveGroupsByRoleIDDataKey, id) || key == string.Format("{0}-{1}", RoleHelper.RetrieveRolesByGroupIDDataKey, g)));
                    ret = true;
                }
                catch (Exception ex)
                {
                    ExceptionManager.Publish(ex);
                    transaction.RollbackTransaction();
                }
            }
            return ret;
        }
    }
}


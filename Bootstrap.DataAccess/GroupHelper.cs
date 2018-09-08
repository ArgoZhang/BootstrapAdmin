using Longbow.Cache;
using Longbow.Data;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;

namespace Bootstrap.DataAccess
{
    /// <summary>
    /// author:liuchun
    /// date:2016.10.22
    /// </summary>
    public static class GroupHelper
    {
        internal const string RetrieveGroupsDataKey = "GroupHelper-RetrieveGroups";
        internal const string RetrieveGroupsByUserIdDataKey = "GroupHelper-RetrieveGroupsByUserId";
        internal const string RetrieveGroupsByRoleIdDataKey = "GroupHelper-RetrieveGroupsByRoleId";
        /// <summary>
        /// 查询所有群组信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public static IEnumerable<Group> RetrieveGroups(int id = 0)
        {
            var ret = CacheManager.GetOrAdd(RetrieveGroupsDataKey, key =>
            {
                string sql = "select * from Groups";
                List<Group> groups = new List<Group>();
                DbCommand cmd = DBAccessManager.SqlDBAccess.CreateCommand(CommandType.Text, sql);
                using (DbDataReader reader = DBAccessManager.SqlDBAccess.ExecuteReader(cmd))
                {
                    while (reader.Read())
                    {
                        groups.Add(new Group()
                        {
                            Id = (int)reader[0],
                            GroupName = (string)reader[1],
                            Description = reader.IsDBNull(2) ? string.Empty : (string)reader[2]
                        });
                    }
                }
                return groups;
            });
            return id == 0 ? ret : ret.Where(t => id == t.Id);
        }
        /// <summary>
        /// 删除群组信息
        /// </summary>
        /// <param name="ids"></param>
        public static bool DeleteGroup(IEnumerable<int> value)
        {
            bool ret = false;
            var ids = string.Join(",", value);
            using (DbCommand cmd = DBAccessManager.SqlDBAccess.CreateCommand(CommandType.StoredProcedure, "Proc_DeleteGroups"))
            {
                cmd.Parameters.Add(DBAccessManager.SqlDBAccess.CreateParameter("@ids", ids));
                ret = DBAccessManager.SqlDBAccess.ExecuteNonQuery(cmd) == -1;
            }
            CacheCleanUtility.ClearCache(groupIds: ids);
            return ret;
        }
        /// <summary>
        /// 保存新建/更新的群组信息
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        public static bool SaveGroup(Group p)
        {
            bool ret = false;
            if (p.GroupName.Length > 50) p.GroupName = p.GroupName.Substring(0, 50);
            if (!string.IsNullOrEmpty(p.Description) && p.Description.Length > 500) p.Description = p.Description.Substring(0, 500);
            string sql = p.Id == 0 ?
                "Insert Into Groups (GroupName, Description) Values (@GroupName, @Description)" :
                "Update Groups set GroupName = @GroupName, Description = @Description where ID = @ID";
            using (DbCommand cmd = DBAccessManager.SqlDBAccess.CreateCommand(CommandType.Text, sql))
            {
                cmd.Parameters.Add(DBAccessManager.SqlDBAccess.CreateParameter("@ID", p.Id));
                cmd.Parameters.Add(DBAccessManager.SqlDBAccess.CreateParameter("@GroupName", p.GroupName));
                cmd.Parameters.Add(DBAccessManager.SqlDBAccess.CreateParameter("@Description", DBAccessFactory.ToDBValue(p.Description)));
                ret = DBAccessManager.SqlDBAccess.ExecuteNonQuery(cmd) == 1;
            }
            CacheCleanUtility.ClearCache(groupIds: p.Id == 0 ? string.Empty : p.Id.ToString());
            return ret;
        }
        /// <summary>
        /// 根据用户查询部门信息
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public static IEnumerable<Group> RetrieveGroupsByUserId(int userId)
        {
            string key = string.Format("{0}-{1}", RetrieveGroupsByUserIdDataKey, userId);
            var ret = CacheManager.GetOrAdd(key, k =>
            {
                string sql = "select g.ID,g.GroupName,g.[Description],case ug.GroupID when g.ID then 'checked' else '' end [status] from Groups g left join UserGroup ug on g.ID=ug.GroupID and UserID=@UserID";
                List<Group> groups = new List<Group>();
                DbCommand cmd = DBAccessManager.SqlDBAccess.CreateCommand(CommandType.Text, sql);
                cmd.Parameters.Add(DBAccessManager.SqlDBAccess.CreateParameter("@UserID", userId));
                using (DbDataReader reader = DBAccessManager.SqlDBAccess.ExecuteReader(cmd))
                {
                    while (reader.Read())
                    {
                        groups.Add(new Group()
                        {
                            Id = (int)reader[0],
                            GroupName = (string)reader[1],
                            Description = reader.IsDBNull(2) ? string.Empty : (string)reader[2],
                            Checked = (string)reader[3]
                        });
                    }
                }
                return groups;
            }, RetrieveGroupsByUserIdDataKey);
            return ret;
        }
        /// <summary>
        /// 保存用户部门关系
        /// </summary>
        /// <param name="id"></param>
        /// <param name="groupIds"></param>
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
                        cmd.Parameters.Add(DBAccessManager.SqlDBAccess.CreateParameter("@UserID", id));
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
                    CacheCleanUtility.ClearCache(groupIds: groupIds, userIds: id.ToString());
                    ret = true;
                }
                catch (Exception ex)
                {
                    transaction.RollbackTransaction();
                    throw ex;
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
            string k = string.Format("{0}-{1}", RetrieveGroupsByRoleIdDataKey, roleId);
            return CacheManager.GetOrAdd(k, key =>
            {
                List<Group> groups = new List<Group>();
                string sql = "select g.ID,g.GroupName,g.[Description],case rg.GroupID when g.ID then 'checked' else '' end [status] from Groups g left join RoleGroup rg on g.ID=rg.GroupID and RoleID=@RoleID";
                DbCommand cmd = DBAccessManager.SqlDBAccess.CreateCommand(CommandType.Text, sql);
                cmd.Parameters.Add(DBAccessManager.SqlDBAccess.CreateParameter("@RoleID", roleId));
                using (DbDataReader reader = DBAccessManager.SqlDBAccess.ExecuteReader(cmd))
                {
                    while (reader.Read())
                    {
                        groups.Add(new Group()
                        {
                            Id = (int)reader[0],
                            GroupName = (string)reader[1],
                            Description = reader.IsDBNull(2) ? string.Empty : (string)reader[2],
                            Checked = (string)reader[3]
                        });
                    }
                }
                return groups;
            }, RetrieveGroupsByRoleIdDataKey);
        }
        /// <summary>
        /// 根据角色ID以及选定的部门ID，保到角色部门表
        /// </summary>
        /// <param name="id"></param>
        /// <param name="groupIds"></param>
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
                        cmd.Parameters.Add(DBAccessManager.SqlDBAccess.CreateParameter("@RoleID", id));
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
                    CacheCleanUtility.ClearCache(groupIds: groupIds, roleIds: id.ToString());
                    ret = true;
                }
                catch (Exception ex)
                {
                    transaction.RollbackTransaction();
                    throw ex;
                }
            }
            return ret;
        }
    }
}

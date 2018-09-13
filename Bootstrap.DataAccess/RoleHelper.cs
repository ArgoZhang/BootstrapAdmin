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
    /// 
    /// </summary>
    public static class RoleHelper
    {
        internal const string RetrieveRolesDataKey = "RoleHelper-RetrieveRoles";
        internal const string RetrieveRolesByUserIdDataKey = "RoleHelper-RetrieveRolesByUserId";
        internal const string RetrieveRolesByMenuIdDataKey = "RoleHelper-RetrieveRolesByMenuId";
        internal const string RetrieveRolesByGroupIdDataKey = "RoleHelper-RetrieveRolesByGroupId";
        /// <summary>
        /// 查询所有角色
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public static IEnumerable<Role> RetrieveRoles(int id = 0)
        {
            var ret = CacheManager.GetOrAdd(RetrieveRolesDataKey, key =>
            {
                string sql = "select * from Roles";
                List<Role> roles = new List<Role>();
                DbCommand cmd = DBAccessManager.SqlDBAccess.CreateCommand(CommandType.Text, sql);
                using (DbDataReader reader = DBAccessManager.SqlDBAccess.ExecuteReader(cmd))
                {
                    while (reader.Read())
                    {
                        roles.Add(new Role()
                        {
                            Id = (int)reader[0],
                            RoleName = (string)reader[1],
                            Description = reader.IsDBNull(2) ? string.Empty : (string)reader[2]
                        });
                    }
                }
                return roles;
            });
            return id == 0 ? ret : ret.Where(t => id == t.Id);
        }
        /// <summary>
        /// 保存用户角色关系
        /// </summary>
        /// <param name="id"></param>
        /// <param name="roleIds"></param>
        /// <returns></returns>
        public static bool SaveRolesByUserId(int id, string roleIds)
        {
            var ret = false;
            DataTable dt = new DataTable();
            dt.Columns.Add("UserID", typeof(int));
            dt.Columns.Add("RoleID", typeof(int));
            //判断用户是否选定角色
            if (!string.IsNullOrEmpty(roleIds)) roleIds.Split(',').ToList().ForEach(roleId => dt.Rows.Add(id, roleId));
            using (TransactionPackage transaction = DBAccessManager.SqlDBAccess.BeginTransaction())
            {
                try
                {
                    // delete user from config table
                    string sql = "delete from UserRole where UserID = @UserID;";
                    using (DbCommand cmd = DBAccessManager.SqlDBAccess.CreateCommand(CommandType.Text, sql))
                    {
                        cmd.Parameters.Add(DBAccessManager.SqlDBAccess.CreateParameter("@UserID", id));
                        DBAccessManager.SqlDBAccess.ExecuteNonQuery(cmd, transaction);
                        if (dt.Rows.Count > 0)
                        {
                            // insert batch data into config table
                            using (SqlBulkCopy bulk = new SqlBulkCopy((SqlConnection)transaction.Transaction.Connection, SqlBulkCopyOptions.Default, (SqlTransaction)transaction.Transaction))
                            {
                                bulk.DestinationTableName = "UserRole";
                                bulk.ColumnMappings.Add("UserID", "UserID");
                                bulk.ColumnMappings.Add("RoleID", "RoleID");
                                bulk.WriteToServer(dt);
                            }
                        }
                        transaction.CommitTransaction();
                    }
                    CacheCleanUtility.ClearCache(userIds: id.ToString(), roleIds: roleIds);
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
        /// 查询某个用户所拥有的角色
        /// </summary>
        /// <returns></returns>
        public static IEnumerable<Role> RetrieveRolesByUserId(int userId)
        {
            string key = string.Format("{0}-{1}", RetrieveRolesByUserIdDataKey, userId);
            return CacheManager.GetOrAdd(key, k =>
            {
                List<Role> roles = new List<Role>();
                string sql = "select r.ID, r.RoleName, r.[Description], case ur.RoleID when r.ID then 'checked' else '' end [status] from Roles r left join UserRole ur on r.ID = ur.RoleID and UserID = @UserID";
                DbCommand cmd = DBAccessManager.SqlDBAccess.CreateCommand(CommandType.Text, sql);
                cmd.Parameters.Add(DBAccessManager.SqlDBAccess.CreateParameter("@UserID", userId));
                using (DbDataReader reader = DBAccessManager.SqlDBAccess.ExecuteReader(cmd))
                {
                    while (reader.Read())
                    {
                        roles.Add(new Role()
                        {
                            Id = (int)reader[0],
                            RoleName = (string)reader[1],
                            Description = reader.IsDBNull(2) ? string.Empty : (string)reader[2],
                            Checked = (string)reader[3]
                        });
                    }
                }
                return roles;
            }, RetrieveRolesByUserIdDataKey);
        }
        /// <summary>
        /// 删除角色表
        /// </summary>
        /// <param name="value"></param>
        public static bool DeleteRole(IEnumerable<int> value)
        {
            bool ret = false;
            var ids = string.Join(",", value);
            using (DbCommand cmd = DBAccessManager.SqlDBAccess.CreateCommand(CommandType.StoredProcedure, "Proc_DeleteRoles"))
            {
                cmd.Parameters.Add(DBAccessManager.SqlDBAccess.CreateParameter("@ids", ids));
                ret = DBAccessManager.SqlDBAccess.ExecuteNonQuery(cmd) == -1;
            }
            CacheCleanUtility.ClearCache(roleIds: ids);
            return ret;
        }
        /// <summary>
        /// 保存新建/更新的角色信息
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        public static bool SaveRole(Role p)
        {
            bool ret = false;
            if (!string.IsNullOrEmpty(p.RoleName) && p.RoleName.Length > 50) p.RoleName = p.RoleName.Substring(0, 50);
            if (!string.IsNullOrEmpty(p.Description) && p.Description.Length > 50) p.Description = p.Description.Substring(0, 500);
            string sql = p.Id == 0 ?
                "Insert Into Roles (RoleName, Description) Values (@RoleName, @Description)" :
                "Update Roles set RoleName = @RoleName, Description = @Description where ID = @ID";
            using (DbCommand cmd = DBAccessManager.SqlDBAccess.CreateCommand(CommandType.Text, sql))
            {
                cmd.Parameters.Add(DBAccessManager.SqlDBAccess.CreateParameter("@ID", p.Id));
                cmd.Parameters.Add(DBAccessManager.SqlDBAccess.CreateParameter("@RoleName", p.RoleName));
                cmd.Parameters.Add(DBAccessManager.SqlDBAccess.CreateParameter("@Description", DBAccessFactory.ToDBValue(p.Description)));
                ret = DBAccessManager.SqlDBAccess.ExecuteNonQuery(cmd) == 1;
            }
            CacheCleanUtility.ClearCache(roleIds: p.Id == 0 ? string.Empty : p.Id.ToString());
            return ret;
        }
        /// <summary>
        /// 查询某个菜单所拥有的角色
        /// </summary>
        /// <param name="menuId"></param>
        /// <returns></returns>
        public static IEnumerable<Role> RetrieveRolesByMenuId(int menuId)
        {
            string key = string.Format("{0}-{1}", RetrieveRolesByMenuIdDataKey, menuId);
            var ret = CacheManager.GetOrAdd(key, k =>
            {
                string sql = "select r.ID, r.RoleName, r.[Description], case ur.RoleID when r.ID then 'checked' else '' end [status] from Roles r left join NavigationRole ur on r.ID = ur.RoleID and NavigationID = @NavigationID";
                List<Role> roles = new List<Role>();
                DbCommand cmd = DBAccessManager.SqlDBAccess.CreateCommand(CommandType.Text, sql);
                cmd.Parameters.Add(DBAccessManager.SqlDBAccess.CreateParameter("@NavigationID", menuId));
                using (DbDataReader reader = DBAccessManager.SqlDBAccess.ExecuteReader(cmd))
                {
                    while (reader.Read())
                    {
                        roles.Add(new Role()
                        {
                            Id = (int)reader[0],
                            RoleName = (string)reader[1],
                            Description = reader.IsDBNull(2) ? string.Empty : (string)reader[2],
                            Checked = (string)reader[3]
                        });
                    }
                }
                return roles;
            }, RetrieveRolesByMenuIdDataKey);
            return ret;
        }
        public static bool SavaRolesByMenuId(int id, string roleIds)
        {
            var ret = false;
            DataTable dt = new DataTable();
            dt.Columns.Add("NavigationID", typeof(int));
            dt.Columns.Add("RoleID", typeof(int));
            //判断用户是否选定角色
            if (!string.IsNullOrEmpty(roleIds)) roleIds.Split(',').ToList().ForEach(roleId => dt.Rows.Add(id, roleId));
            using (TransactionPackage transaction = DBAccessManager.SqlDBAccess.BeginTransaction())
            {
                try
                {
                    // delete role from config table
                    string sql = "delete from NavigationRole where NavigationID=@NavigationID;";
                    using (DbCommand cmd = DBAccessManager.SqlDBAccess.CreateCommand(CommandType.Text, sql))
                    {
                        cmd.Parameters.Add(DBAccessManager.SqlDBAccess.CreateParameter("@NavigationID", id));
                        DBAccessManager.SqlDBAccess.ExecuteNonQuery(cmd, transaction);

                        // insert batch data into config table
                        using (SqlBulkCopy bulk = new SqlBulkCopy((SqlConnection)transaction.Transaction.Connection, SqlBulkCopyOptions.Default, (SqlTransaction)transaction.Transaction))
                        {
                            bulk.BatchSize = 1000;
                            bulk.DestinationTableName = "NavigationRole";
                            bulk.ColumnMappings.Add("NavigationID", "NavigationID");
                            bulk.ColumnMappings.Add("RoleID", "RoleID");
                            bulk.WriteToServer(dt);
                            transaction.CommitTransaction();
                        }
                    }
                    CacheCleanUtility.ClearCache(roleIds: roleIds, menuIds: new List<int>() { id });
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
        /// 根据GroupId查询和该Group有关的所有Roles
        /// </summary>
        /// <param name="groupId"></param>
        /// <returns></returns>
        public static IEnumerable<Role> RetrieveRolesByGroupId(int groupId)
        {
            string key = string.Format("{0}-{1}", RetrieveRolesByGroupIdDataKey, groupId);
            return CacheManager.GetOrAdd(key, k =>
            {
                List<Role> roles = new List<Role>();
                string sql = "select r.ID, r.RoleName, r.[Description], case ur.RoleID when r.ID then 'checked' else '' end [status] from Roles r left join RoleGroup ur on r.ID = ur.RoleID and GroupID = @GroupID";
                DbCommand cmd = DBAccessManager.SqlDBAccess.CreateCommand(CommandType.Text, sql);
                cmd.Parameters.Add(DBAccessManager.SqlDBAccess.CreateParameter("@GroupID", groupId));
                using (DbDataReader reader = DBAccessManager.SqlDBAccess.ExecuteReader(cmd))
                {
                    while (reader.Read())
                    {
                        roles.Add(new Role()
                        {
                            Id = (int)reader[0],
                            RoleName = (string)reader[1],
                            Description = reader.IsDBNull(2) ? string.Empty : (string)reader[2],
                            Checked = (string)reader[3]
                        });
                    }
                }
                return roles;
            }, RetrieveRolesByGroupIdDataKey);
        }

        /// <summary>
        /// 根据GroupId更新Roles信息，删除旧的Roles信息，插入新的Roles信息
        /// </summary>
        /// <param name="id"></param>
        /// <param name="roleIds"></param>
        /// <returns></returns>
        public static bool SaveRolesByGroupId(int id, string roleIds)
        {
            var ret = false;
            //构造表格
            DataTable dt = new DataTable();
            dt.Columns.Add("RoleID", typeof(int));
            dt.Columns.Add("GroupID", typeof(int));
            if (!string.IsNullOrEmpty(roleIds)) roleIds.Split(',').ToList().ForEach(roleId => dt.Rows.Add(roleId, id));
            using (TransactionPackage transaction = DBAccessManager.SqlDBAccess.BeginTransaction())
            {
                try
                {
                    // delete user from config table
                    string sql = "delete from RoleGroup where GroupID=@GroupID";
                    using (DbCommand cmd = DBAccessManager.SqlDBAccess.CreateCommand(CommandType.Text, sql))
                    {
                        cmd.Parameters.Add(DBAccessManager.SqlDBAccess.CreateParameter("@GroupID", id));
                        DBAccessManager.SqlDBAccess.ExecuteNonQuery(cmd, transaction);

                        // insert batch data into config table
                        using (SqlBulkCopy bulk = new SqlBulkCopy((SqlConnection)transaction.Transaction.Connection, SqlBulkCopyOptions.Default, (SqlTransaction)transaction.Transaction))
                        {
                            bulk.BatchSize = 1000;
                            bulk.DestinationTableName = "RoleGroup";
                            bulk.ColumnMappings.Add("RoleID", "RoleID");
                            bulk.ColumnMappings.Add("GroupID", "GroupID");
                            bulk.WriteToServer(dt);
                            transaction.CommitTransaction();
                        }
                    }
                    CacheCleanUtility.ClearCache(roleIds: roleIds, groupIds: id.ToString());
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
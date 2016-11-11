using Longbow;
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
    /// 
    /// </summary>
    public static class RoleHelper
    {
        internal const string RetrieveRolesDataKey = "RoleHelper-RetrieveRoles";
        private const string RetrieveRolesByUrlDataKey = "RoleHelper-RetrieveRolesByUrl";
        internal const string RetrieveRolesByUserNameDataKey = "RoleHelper-RetrieveRolesByUserName";
        internal const string RetrieveRolesByUserIDDataKey = "RoleHelper-RetrieveRolesByUserId";
        internal const string RetrieveRolesByMenuIDDataKey = "RoleHelper-RetrieveRolesByMenuId";
        internal const string RetrieveRolesByGroupIDDataKey = "RoleHelper-RetrieveRolesByGroupId";
        /// <summary>
        /// 查询所有角色
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public static IEnumerable<Role> RetrieveRoles(int id = 0)
        {
            var ret = CacheManager.GetOrAdd(RetrieveRolesDataKey, CacheSection.RetrieveIntervalByKey(RetrieveRolesDataKey), key =>
            {
                string sql = "select * from Roles";
                List<Role> roles = new List<Role>();
                DbCommand cmd = DBAccessManager.SqlDBAccess.CreateCommand(CommandType.Text, sql);
                try
                {
                    using (DbDataReader reader = DBAccessManager.SqlDBAccess.ExecuteReader(cmd))
                    {
                        while (reader.Read())
                        {
                            roles.Add(new Role()
                            {
                                ID = (int)reader[0],
                                RoleName = LgbConvert.ReadValue(reader[1], string.Empty),
                                Description = LgbConvert.ReadValue(reader[2], string.Empty)
                            });
                        }
                    }
                }
                catch (Exception ex) { ExceptionManager.Publish(ex); }
                return roles;
            }, CacheSection.RetrieveDescByKey(RetrieveRolesDataKey));
            return id == 0 ? ret : ret.Where(t => id == t.ID);
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
                        cmd.Parameters.Add(DBAccessManager.SqlDBAccess.CreateParameter("@UserID", id, ParameterDirection.Input));
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
                    ExceptionManager.Publish(ex);
                    transaction.RollbackTransaction();
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
            string key = string.Format("{0}-{1}", RetrieveRolesByUserIDDataKey, userId);
            return CacheManager.GetOrAdd(key, CacheSection.RetrieveIntervalByKey(RetrieveRolesByUserIDDataKey), k =>
            {
                List<Role> Roles = new List<Role>();
                string sql = "select r.ID, r.RoleName, r.[Description], case ur.RoleID when r.ID then 'checked' else '' end [status] from Roles r left join UserRole ur on r.ID = ur.RoleID and UserID = @UserID";
                try
                {
                    DbCommand cmd = DBAccessManager.SqlDBAccess.CreateCommand(CommandType.Text, sql);
                    cmd.Parameters.Add(DBAccessManager.SqlDBAccess.CreateParameter("@UserID", userId, ParameterDirection.Input));
                    using (DbDataReader reader = DBAccessManager.SqlDBAccess.ExecuteReader(cmd))
                    {
                        while (reader.Read())
                        {
                            Roles.Add(new Role()
                            {
                                ID = (int)reader[0],
                                RoleName = (string)reader[1],
                                Description = (string)reader[2],
                                Checked = (string)reader[3]
                            });
                        }
                    }
                }
                catch (Exception ex) { ExceptionManager.Publish(ex); }
                return Roles;
            }, CacheSection.RetrieveDescByKey(RetrieveRolesByUserIDDataKey));
        }
        /// <summary>
        /// 删除角色表
        /// </summary>
        /// <param name="IDs"></param>
        public static bool DeleteRole(string ids)
        {
            bool ret = false;
            if (string.IsNullOrEmpty(ids) || ids.Contains("'")) return ret;
            try
            {
                using (DbCommand cmd = DBAccessManager.SqlDBAccess.CreateCommand(CommandType.StoredProcedure, "Proc_DeleteRoles"))
                {
                    cmd.Parameters.Add(DBAccessManager.SqlDBAccess.CreateParameter("@ids", ids, ParameterDirection.Input));
                    DBAccessManager.SqlDBAccess.ExecuteNonQuery(cmd);
                }
                CacheCleanUtility.ClearCache(roleIds: ids);
                ret = true;
            }
            catch (Exception ex)
            {
                ExceptionManager.Publish(ex);
            }
            return ret;
        }
        /// <summary>
        /// 保存新建/更新的角色信息
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        public static bool SaveRole(Role p)
        {
            if (p == null) throw new ArgumentNullException("p");
            bool ret = false;
            if (!string.IsNullOrEmpty(p.RoleName) && p.RoleName.Length > 50) p.RoleName = p.RoleName.Substring(0, 50);
            if (!string.IsNullOrEmpty(p.Description) && p.Description.Length > 50) p.Description = p.Description.Substring(0, 500);
            string sql = p.ID == 0 ?
                "Insert Into Roles (RoleName, Description) Values (@RoleName, @Description)" :
                "Update Roles set RoleName = @RoleName, Description = @Description where ID = @ID";
            try
            {
                using (DbCommand cmd = DBAccessManager.SqlDBAccess.CreateCommand(CommandType.Text, sql))
                {
                    cmd.Parameters.Add(DBAccessManager.SqlDBAccess.CreateParameter("@ID", p.ID, ParameterDirection.Input));
                    cmd.Parameters.Add(DBAccessManager.SqlDBAccess.CreateParameter("@RoleName", p.RoleName, ParameterDirection.Input));
                    cmd.Parameters.Add(DBAccessManager.SqlDBAccess.CreateParameter("@Description", p.Description, ParameterDirection.Input));
                    DBAccessManager.SqlDBAccess.ExecuteNonQuery(cmd);
                }
                CacheCleanUtility.ClearCache(roleIds: p.ID == 0 ? string.Empty : p.ID.ToString());
                ret = true;
            }
            catch (DbException ex)
            {
                ExceptionManager.Publish(ex);
            }
            return ret;
        }
        /// <summary>
        /// 查询某个菜单所拥有的角色
        /// </summary>
        /// <param name="menuId"></param>
        /// <returns></returns>
        public static IEnumerable<Role> RetrieveRolesByMenuId(int menuId)
        {
            string key = string.Format("{0}-{1}", RetrieveRolesByMenuIDDataKey, menuId);
            var ret = CacheManager.GetOrAdd(key, CacheSection.RetrieveIntervalByKey(RetrieveRolesByMenuIDDataKey), k =>
            {
                string sql = "select r.ID, r.RoleName, r.[Description], case ur.RoleID when r.ID then 'checked' else '' end [status] from Roles r left join NavigationRole ur on r.ID = ur.RoleID and NavigationID = @NavigationID";
                List<Role> Roles = new List<Role>();
                DbCommand cmd = DBAccessManager.SqlDBAccess.CreateCommand(CommandType.Text, sql);
                cmd.Parameters.Add(DBAccessManager.SqlDBAccess.CreateParameter("@NavigationID", menuId, ParameterDirection.Input));
                try
                {
                    using (DbDataReader reader = DBAccessManager.SqlDBAccess.ExecuteReader(cmd))
                    {
                        while (reader.Read())
                        {
                            Roles.Add(new Role()
                            {
                                ID = (int)reader[0],
                                RoleName = (string)reader[1],
                                Description = (string)reader[2],
                                Checked = (string)reader[3]
                            });
                        }
                    }
                }
                catch (Exception ex) { ExceptionManager.Publish(ex); }
                return Roles;
            }, CacheSection.RetrieveDescByKey(RetrieveRolesByMenuIDDataKey));
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
                        cmd.Parameters.Add(DBAccessManager.SqlDBAccess.CreateParameter("@NavigationID", id, ParameterDirection.Input));
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
                    CacheCleanUtility.ClearCache(roleIds: roleIds, menuIds: id.ToString());
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
        /// 根据GroupId查询和该Group有关的所有Roles
        /// author:liuchun
        /// <param name="tId"></param>
        /// <returns></returns>
        public static IEnumerable<Role> RetrieveRolesByGroupId(int groupID)
        {
            string key = string.Format("{0}-{1}", RetrieveRolesByGroupIDDataKey, groupID);
            return CacheManager.GetOrAdd(key, CacheSection.RetrieveIntervalByKey(RetrieveRolesByGroupIDDataKey), k =>
            {
                List<Role> Roles = new List<Role>();
                string sql = "select r.ID, r.RoleName, r.[Description], case ur.RoleID when r.ID then 'checked' else '' end [status] from Roles r left join RoleGroup ur on r.ID = ur.RoleID and GroupID = @GroupID";
                DbCommand cmd = DBAccessManager.SqlDBAccess.CreateCommand(CommandType.Text, sql);
                try
                {
                    cmd.Parameters.Add(DBAccessManager.SqlDBAccess.CreateParameter("@GroupID", groupID, ParameterDirection.Input));
                    using (DbDataReader reader = DBAccessManager.SqlDBAccess.ExecuteReader(cmd))
                    {
                        while (reader.Read())
                        {
                            Roles.Add(new Role()
                            {
                                ID = (int)reader[0],
                                RoleName = (string)reader[1],
                                Description = (string)reader[2],
                                Checked = (string)reader[3]
                            });
                        }
                    }
                }
                catch (Exception ex) { ExceptionManager.Publish(ex); }
                return Roles;
            }, CacheSection.RetrieveDescByKey(RetrieveRolesByGroupIDDataKey));
        }

        /// <summary>
        /// 根据GroupId更新Roles信息，删除旧的Roles信息，插入新的Roles信息
        /// </summary>
        /// <param name="p"></param>
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
                        cmd.Parameters.Add(DBAccessManager.SqlDBAccess.CreateParameter("@GroupID", id, ParameterDirection.Input));
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
                    ExceptionManager.Publish(ex);
                    transaction.RollbackTransaction();
                }
            }
            return ret;
        }
        /// <summary>
        /// 根据用户名查询某个用户所拥有的角色
        /// 从UserRole表查
        /// 从User-〉Group-〉GroupRole查
        /// </summary>
        /// <returns></returns>
        public static IEnumerable<Role> RetrieveRolesByUserName(string username)
        {
            string key = string.Format("{0}-{1}", RetrieveRolesByUserNameDataKey, username);
            return CacheManager.GetOrAdd(key, CacheSection.RetrieveIntervalByKey(RetrieveRolesByUserNameDataKey), k =>
            {
                List<Role> Roles = new List<Role>();
                try
                {
                    string sql = "select r.ID, r.RoleName, r.[Description] from Roles r inner join UserRole ur on r.ID=ur.RoleID inner join Users u on ur.UserID=u.ID and u.UserName=@UserName union select r.ID, r.RoleName, r.[Description] from Roles r inner join RoleGroup rg on r.ID=rg.RoleID inner join Groups g on rg.GroupID=g.ID inner join UserGroup ug on ug.GroupID=g.ID inner join Users u on ug.UserID=u.ID and u.UserName=@UserName";
                    DbCommand cmd = DBAccessManager.SqlDBAccess.CreateCommand(CommandType.Text, sql);
                    cmd.Parameters.Add(DBAccessManager.SqlDBAccess.CreateParameter("@UserName", username, ParameterDirection.Input));
                    using (DbDataReader reader = DBAccessManager.SqlDBAccess.ExecuteReader(cmd))
                    {
                        while (reader.Read())
                        {
                            Roles.Add(new Role()
                            {
                                ID = (int)reader[0],
                                RoleName = (string)reader[1],
                                Description = (string)reader[2],
                            });
                        }
                    }
                }
                catch (Exception ex) { ExceptionManager.Publish(ex); }
                return Roles;
            }, CacheSection.RetrieveDescByKey(RetrieveRolesByUserNameDataKey));
        }
        /// <summary>
        /// 根据菜单url查询某个所拥有的角色
        /// 从NavigatorRole表查
        /// 从Navigators-〉GroupNavigatorRole-〉Role查查询某个用户所拥有的角色
        /// </summary>
        /// <returns></returns>
        public static IEnumerable<Role> RetrieveRolesByUrl(string url)
        {
            string key = string.Format("{0}-{1}", RetrieveRolesByUrlDataKey, url);
            return CacheManager.GetOrAdd(key, CacheSection.RetrieveIntervalByKey(RetrieveRolesByUrlDataKey), k =>
            {
                string sql = "select r.ID, r.RoleName, r.[Description] from Roles r inner join NavigationRole nr on r.ID = nr.RoleID inner join Navigations n on nr.NavigationID = n.ID and n.Url = @URl";
                List<Role> Roles = new List<Role>();
                try
                {
                    DbCommand cmd = DBAccessManager.SqlDBAccess.CreateCommand(CommandType.Text, sql);
                    cmd.Parameters.Add(DBAccessManager.SqlDBAccess.CreateParameter("@URl", url, ParameterDirection.Input));
                    using (DbDataReader reader = DBAccessManager.SqlDBAccess.ExecuteReader(cmd))
                    {
                        while (reader.Read())
                        {
                            Roles.Add(new Role()
                            {
                                ID = (int)reader[0],
                                RoleName = (string)reader[1],
                                Description = (string)reader[2],
                            });
                        }
                    }
                }
                catch (Exception ex) { ExceptionManager.Publish(ex); }
                return Roles;
            }, CacheSection.RetrieveDescByKey(RetrieveRolesByUrlDataKey));
        }
    }
}
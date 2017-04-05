using Bootstrap.Security;
using Longbow;
using Longbow.Caching;
using Longbow.Data;
using Longbow.ExceptionManagement;
using Longbow.Security;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;

namespace Bootstrap.DataAccess
{
    /// <summary>
    /// 用户表相关操作类
    /// </summary>
    public static class UserHelper
    {
        private const string RetrieveUsersByNameDataKey = "BootstrapUser-RetrieveUsersByName";
        internal const string RetrieveUsersByRoleIdDataKey = "BootstrapUser-RetrieveUsersByRoleId";
        internal const string RetrieveUsersByGroupIdDataKey = "BootstrapUser-RetrieveUsersByGroupId";
        internal const string RetrieveNewUsersDataKey = "UserHelper-RetrieveNewUsers";
        /// <summary>
        /// 查询所有用户
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public static IEnumerable<User> RetrieveUsers()
        {
            return CacheManager.GetOrAdd(BootstrapUser.RetrieveUsersDataKey, key =>
            {
                List<User> users = new List<User>();
                DbCommand cmd = DBAccessManager.SqlDBAccess.CreateCommand(CommandType.Text, "select ID, UserName, DisplayName, RegisterTime, ApprovedTime, ApprovedBy, Description from Users Where ApprovedTime is not null");
                try
                {
                    using (DbDataReader reader = DBAccessManager.SqlDBAccess.ExecuteReader(cmd))
                    {
                        while (reader.Read())
                        {
                            users.Add(new User()
                            {
                                Id = (int)reader[0],
                                UserName = (string)reader[1],
                                DisplayName = (string)reader[2],
                                RegisterTime = (DateTime)reader[3],
                                ApprovedTime = LgbConvert.ReadValue(reader[4], DateTime.MinValue),
                                ApprovedBy = reader.IsDBNull(5) ? string.Empty : (string)reader[5],
                                Description = (string)reader[6]
                            });
                        }
                    }
                }
                catch (Exception ex) { ExceptionManager.Publish(ex); }
                return users;
            });
        }
        /// <summary>
        /// 查询所有的新注册用户
        /// </summary>
        /// <returns></returns>
        public static IEnumerable<User> RetrieveNewUsers()
        {
            return CacheManager.GetOrAdd(RetrieveNewUsersDataKey, key =>
            {
                string sql = "select ID, UserName, DisplayName, RegisterTime, [Description] from Users Where ApprovedTime is null and RejectedTime is null order by RegisterTime desc";
                List<User> users = new List<User>();
                DbCommand cmd = DBAccessManager.SqlDBAccess.CreateCommand(CommandType.Text, sql);
                try
                {
                    using (DbDataReader reader = DBAccessManager.SqlDBAccess.ExecuteReader(cmd))
                    {
                        while (reader.Read())
                        {
                            users.Add(new User()
                            {
                                Id = (int)reader[0],
                                UserName = (string)reader[1],
                                DisplayName = (string)reader[2],
                                RegisterTime = (DateTime)reader[3],
                                Description = (string)reader[4]
                            });
                        }
                    }
                }
                catch (Exception ex) { ExceptionManager.Publish(ex); }
                return users;
            });
        }
        /// <summary>
        /// 删除用户
        /// </summary>
        /// <param name="ids"></param>
        public static bool DeleteUser(string ids)
        {
            if (string.IsNullOrEmpty(ids) || ids.Contains("'")) return false;
            bool ret = false;
            try
            {
                using (DbCommand cmd = DBAccessManager.SqlDBAccess.CreateCommand(CommandType.StoredProcedure, "Proc_DeleteUsers"))
                {
                    cmd.Parameters.Add(DBAccessManager.SqlDBAccess.CreateParameter("@ids", ids));
                    DBAccessManager.SqlDBAccess.ExecuteNonQuery(cmd);
                }
                CacheCleanUtility.ClearCache(userIds: ids);
                ret = true;
            }
            catch (Exception ex)
            {
                ExceptionManager.Publish(ex);
            }
            return ret;
        }
        /// <summary>
        /// 保存新建/更新的用户信息
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        public static bool SaveUser(User p)
        {
            if (p.Id == 0 && p.Description.Length > 500) p.Description = p.Description.Substring(0, 500);
            if (p.UserStatus != 2)
            {
                if (p.UserName.Length > 50) p.UserName = p.UserName.Substring(0, 50);
                p.PassSalt = LgbCryptography.GenerateSalt();
                p.Password = LgbCryptography.ComputeHash(p.Password, p.PassSalt);
            }
            bool ret = false;
            try
            {
                using (DbCommand cmd = DBAccessManager.SqlDBAccess.CreateCommand(CommandType.StoredProcedure, "Proc_SaveUsers"))
                {
                    cmd.Parameters.Add(DBAccessManager.SqlDBAccess.CreateParameter("@id", p.Id));
                    cmd.Parameters.Add(DBAccessManager.SqlDBAccess.CreateParameter("@userName", DBAccess.ToDBValue(p.UserName)));
                    cmd.Parameters.Add(DBAccessManager.SqlDBAccess.CreateParameter("@password", DBAccess.ToDBValue(p.Password)));
                    cmd.Parameters.Add(DBAccessManager.SqlDBAccess.CreateParameter("@passSalt", DBAccess.ToDBValue(p.PassSalt)));
                    cmd.Parameters.Add(DBAccessManager.SqlDBAccess.CreateParameter("@displayName", DBAccess.ToDBValue(p.DisplayName)));
                    cmd.Parameters.Add(DBAccessManager.SqlDBAccess.CreateParameter("@description", DBAccess.ToDBValue(p.Description)));
                    cmd.Parameters.Add(DBAccessManager.SqlDBAccess.CreateParameter("@approvedBy", DBAccess.ToDBValue(p.ApprovedBy)));
                    cmd.Parameters.Add(DBAccessManager.SqlDBAccess.CreateParameter("@rejectedBy", DBAccess.ToDBValue(p.RejectedBy)));
                    cmd.Parameters.Add(DBAccessManager.SqlDBAccess.CreateParameter("@rejectedReason", DBAccess.ToDBValue(p.RejectedReason)));
                    cmd.Parameters.Add(DBAccessManager.SqlDBAccess.CreateParameter("@userStatus", p.UserStatus));
                    DBAccessManager.SqlDBAccess.ExecuteNonQuery(cmd);
                }
                CacheCleanUtility.ClearCache(userIds: p.Id == 0 ? string.Empty : p.Id.ToString());
                ret = true;
            }
            catch (DbException ex)
            {
                ExceptionManager.Publish(ex);
            }
            return ret;
        }
        /// <summary>
        /// 通过roleId获取所有用户
        /// </summary>
        /// <param name="roleId"></param>
        /// <returns></returns>
        public static IEnumerable<User> RetrieveUsersByRoleId(int roleId)
        {
            string key = string.Format("{0}-{1}", RetrieveUsersByRoleIdDataKey, roleId);
            return CacheManager.GetOrAdd(key, k =>
            {
                List<User> users = new List<User>();
                string sql = "select u.ID, u.UserName, u.DisplayName, case ur.UserID when u.ID then 'checked' else '' end [status] from Users u left join UserRole ur on u.ID = ur.UserID and RoleID = @RoleID where u.ApprovedTime is not null";
                DbCommand cmd = DBAccessManager.SqlDBAccess.CreateCommand(CommandType.Text, sql);
                cmd.Parameters.Add(DBAccessManager.SqlDBAccess.CreateParameter("@RoleID", roleId));
                try
                {
                    using (DbDataReader reader = DBAccessManager.SqlDBAccess.ExecuteReader(cmd))
                    {
                        while (reader.Read())
                        {
                            users.Add(new User()
                            {
                                Id = (int)reader[0],
                                UserName = (string)reader[1],
                                DisplayName = (string)reader[2],
                                Checked = (string)reader[3]
                            });
                        }
                    }
                }
                catch (Exception ex) { ExceptionManager.Publish(ex); }
                return users;
            }, RetrieveUsersByRoleIdDataKey);
        }
        /// <summary>
        /// 通过角色ID保存当前授权用户（插入）
        /// </summary>
        /// <param name="id">角色ID</param>
        /// <param name="userIds">用户ID数组</param>
        /// <returns></returns>
        public static bool SaveUsersByRoleId(int id, string userIds)
        {
            bool ret = false;
            DataTable dt = new DataTable();
            dt.Columns.Add("RoleID", typeof(int));
            dt.Columns.Add("UserID", typeof(int));
            if (!string.IsNullOrEmpty(userIds)) userIds.Split(',').ToList().ForEach(userId => dt.Rows.Add(id, userId));
            using (TransactionPackage transaction = DBAccessManager.SqlDBAccess.BeginTransaction())
            {
                try
                {
                    //删除用户角色表该角色所有的用户
                    string sql = "delete from UserRole where RoleID=@RoleID";
                    using (DbCommand cmd = DBAccessManager.SqlDBAccess.CreateCommand(CommandType.Text, sql))
                    {
                        cmd.Parameters.Add(DBAccessManager.SqlDBAccess.CreateParameter("@RoleID", id));
                        DBAccessManager.SqlDBAccess.ExecuteNonQuery(cmd, transaction);
                        //批插入用户角色表
                        using (SqlBulkCopy bulk = new SqlBulkCopy((SqlConnection)transaction.Transaction.Connection, SqlBulkCopyOptions.Default, (SqlTransaction)transaction.Transaction))
                        {
                            bulk.DestinationTableName = "UserRole";
                            bulk.ColumnMappings.Add("RoleID", "RoleID");
                            bulk.ColumnMappings.Add("UserID", "UserID");
                            bulk.WriteToServer(dt);
                            transaction.CommitTransaction();
                        }
                    }
                    CacheCleanUtility.ClearCache(userIds: userIds, roleIds: id.ToString());
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
        /// 通过groupId获取所有用户
        /// </summary>
        /// <param name="groupId"></param>
        /// <returns></returns>
        public static IEnumerable<User> RetrieveUsersByGroupId(int groupId)
        {
            string key = string.Format("{0}-{1}", RetrieveUsersByGroupIdDataKey, groupId);
            return CacheManager.GetOrAdd(key, k =>
            {
                List<User> users = new List<User>();
                string sql = "select u.ID, u.UserName, u.DisplayName, case ur.UserID when u.ID then 'checked' else '' end [status] from Users u left join UserGroup ur on u.ID = ur.UserID and GroupID =@groupId where u.ApprovedTime is not null";
                DbCommand cmd = DBAccessManager.SqlDBAccess.CreateCommand(CommandType.Text, sql);
                cmd.Parameters.Add(DBAccessManager.SqlDBAccess.CreateParameter("@GroupID", groupId));
                try
                {
                    using (DbDataReader reader = DBAccessManager.SqlDBAccess.ExecuteReader(cmd))
                    {
                        while (reader.Read())
                        {
                            users.Add(new User()
                            {
                                Id = (int)reader[0],
                                UserName = (string)reader[1],
                                DisplayName = (string)reader[2],
                                Checked = (string)reader[3]
                            });
                        }
                    }
                }
                catch (Exception ex) { ExceptionManager.Publish(ex); }
                return users;
            }, RetrieveUsersByRoleIdDataKey);
        }
        /// <summary>
        /// 通过部门ID保存当前授权用户（插入）
        /// </summary>
        /// <param name="id">GroupID</param>
        /// <param name="userIds">用户ID数组</param>
        /// <returns></returns>
        public static bool SaveUsersByGroupId(int id, string userIds)
        {
            bool ret = false;
            DataTable dt = new DataTable();
            dt.Columns.Add("UserID", typeof(int));
            dt.Columns.Add("GroupID", typeof(int));
            if (!string.IsNullOrEmpty(userIds)) userIds.Split(',').ToList().ForEach(userId => dt.Rows.Add(userId, id));
            using (TransactionPackage transaction = DBAccessManager.SqlDBAccess.BeginTransaction())
            {
                try
                {
                    //删除用户角色表该角色所有的用户
                    string sql = "delete from UserGroup where GroupID = @GroupID";
                    using (DbCommand cmd = DBAccessManager.SqlDBAccess.CreateCommand(CommandType.Text, sql))
                    {
                        cmd.Parameters.Add(DBAccessManager.SqlDBAccess.CreateParameter("@GroupID", id));
                        DBAccessManager.SqlDBAccess.ExecuteNonQuery(cmd, transaction);
                        //批插入用户角色表
                        using (SqlBulkCopy bulk = new SqlBulkCopy((SqlConnection)transaction.Transaction.Connection, SqlBulkCopyOptions.Default, (SqlTransaction)transaction.Transaction))
                        {
                            bulk.DestinationTableName = "UserGroup";
                            bulk.ColumnMappings.Add("UserID", "UserID");
                            bulk.ColumnMappings.Add("GroupID", "GroupID");
                            bulk.WriteToServer(dt);
                            transaction.CommitTransaction();
                        }
                    }
                    CacheCleanUtility.ClearCache(userIds: userIds, groupIds: id.ToString());
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
        /// 根据用户名修改用户头像
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="iconName"></param>
        /// <returns></returns>
        public static bool SaveUserIconByName(string userName, string iconName)
        {
            bool ret = false;
            try
            {
                string sql = "Update Users set Icon = @iconName where UserName = @userName";
                using (DbCommand cmd = DBAccessManager.SqlDBAccess.CreateCommand(CommandType.Text, sql))
                {
                    cmd.Parameters.Add(DBAccessManager.SqlDBAccess.CreateParameter("@iconName", iconName));
                    cmd.Parameters.Add(DBAccessManager.SqlDBAccess.CreateParameter("@userName", userName));
                    DBAccessManager.SqlDBAccess.ExecuteNonQuery(cmd);
                    string key = string.Format("{0}-{1}", RetrieveUsersByNameDataKey, userName);
                    CacheManager.Clear(k => key == k);
                    ret = true;
                }
            }
            catch (Exception ex)
            {
                ExceptionManager.Publish(ex);
            }
            return ret;
        }
    }
}

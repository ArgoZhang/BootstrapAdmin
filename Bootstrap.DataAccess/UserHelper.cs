using Bootstrap.Security;
using Longbow;
using Longbow.Caching;
using Longbow.Caching.Configuration;
using Longbow.Data;
using Longbow.ExceptionManagement;
using Longbow.Security;
using Longbow.Security.Principal;
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
        internal const string RetrieveUsersDataKey = "UserHelper-RetrieveUsers";
        private const string RetrieveUsersByNameDataKey = "UserHelper-RetrieveUsersByName";
        internal const string RetrieveUsersByRoleIDDataKey = "UserHelper-RetrieveUsersByRoleId";
        internal const string RetrieveUsersByGroupIDDataKey = "UserHelper-RetrieveUsersByGroupId";
        internal const string RetrieveNewUsersDataKey = "UserHelper-RetrieveNewUsers";
        /// <summary>
        /// 查询所有用户
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public static IEnumerable<User> RetrieveUsers(int id = 0)
        {
            string sql = "select ID, UserName, DisplayName, RegisterTime, ApprovedTime from Users Where ApprovedTime is not null";
            var ret = CacheManager.GetOrAdd(RetrieveUsersDataKey, CacheSection.RetrieveIntervalByKey(RetrieveUsersDataKey), key =>
            {
                List<User> Users = new List<User>();
                DbCommand cmd = DBAccessManager.SqlDBAccess.CreateCommand(CommandType.Text, sql);
                try
                {
                    using (DbDataReader reader = DBAccessManager.SqlDBAccess.ExecuteReader(cmd))
                    {
                        while (reader.Read())
                        {
                            Users.Add(new User()
                            {
                                ID = (int)reader[0],
                                UserName = (string)reader[1],
                                DisplayName = (string)reader[2],
                                RegisterTime = (DateTime)reader[3],
                                ApprovedTime = LgbConvert.ReadValue(reader[4], DateTime.MinValue)
                            });
                        }
                    }
                }
                catch (Exception ex) { ExceptionManager.Publish(ex); }
                return Users;
            }, CacheSection.RetrieveDescByKey(RetrieveUsersDataKey));
            return id == 0 ? ret : ret.Where(t => id == t.ID);
        }
        /// <summary>
        /// 根据用户名查询用户
        /// </summary>
        /// <param name="userName"></param>
        /// <returns></returns>
        public static User RetrieveUsersByName(string userName)
        {
            if (LgbPrincipal.IsWebAdmin(userName)) return new User() { DisplayName = "网站管理员", UserName = userName, Icon = "~/Content/images/uploader/default.jpg" };
            string key = string.Format("{0}-{1}", RetrieveUsersByNameDataKey, userName);
            return CacheManager.GetOrAdd(key, CacheSection.RetrieveIntervalByKey(RetrieveUsersByNameDataKey), k =>
            {
                User user = null;
                string sql = "select u.ID, UserName, DisplayName, RegisterTime, ApprovedTime, case isnull(d.Code, '') when '' then '~/Content/images/uploader/' else d.Code end + Icon from Users u left join Dicts d on d.Define = '0' and d.Category = N'头像地址' and Name = N'头像路径' where ApprovedTime is not null and UserName = @UserName";
                DbCommand cmd = DBAccessManager.SqlDBAccess.CreateCommand(CommandType.Text, sql);
                try
                {
                    cmd.Parameters.Add(DBAccessManager.SqlDBAccess.CreateParameter("@UserName", userName, ParameterDirection.Input));
                    using (DbDataReader reader = DBAccessManager.SqlDBAccess.ExecuteReader(cmd))
                    {
                        if (reader.Read())
                        {
                            user = new User()
                            {
                                ID = (int)reader[0],
                                UserName = (string)reader[1],
                                DisplayName = (string)reader[2],
                                RegisterTime = (DateTime)reader[3],
                                ApprovedTime = (DateTime)reader[4],
                                Icon = (string)reader[5]
                            };
                        }
                    }
                }
                catch (Exception ex) { ExceptionManager.Publish(ex); }
                return user;
            }, CacheSection.RetrieveDescByKey(RetrieveUsersByNameDataKey));
        }
        /// <summary>
        /// 查询所有的新注册用户
        /// </summary>
        /// <returns></returns>
        public static IEnumerable<User> RetrieveNewUsers()
        {
            return CacheManager.GetOrAdd(RetrieveNewUsersDataKey, CacheSection.RetrieveIntervalByKey(RetrieveNewUsersDataKey), key =>
            {
                string sql = "select ID, UserName, DisplayName, RegisterTime, [Description] from Users Where ApprovedTime is null and RejectedTime is null order by RegisterTime desc";
                List<User> Users = new List<User>();
                DbCommand cmd = DBAccessManager.SqlDBAccess.CreateCommand(CommandType.Text, sql);
                try
                {
                    using (DbDataReader reader = DBAccessManager.SqlDBAccess.ExecuteReader(cmd))
                    {
                        while (reader.Read())
                        {
                            Users.Add(new User()
                            {
                                ID = (int)reader[0],
                                UserName = (string)reader[1],
                                DisplayName = (string)reader[2],
                                RegisterTime = (DateTime)reader[3],
                                Description = (string)reader[4]
                            });
                        }
                    }
                }
                catch (Exception ex) { ExceptionManager.Publish(ex); }
                return Users;
            }, CacheSection.RetrieveDescByKey(RetrieveNewUsersDataKey));
        }
        /// <summary>
        /// 删除用户
        /// </summary>
        /// <param name="ids"></param>
        public static bool DeleteUser(string ids)
        {
            bool ret = false;
            if (string.IsNullOrEmpty(ids) || ids.Contains("'")) return ret;
            try
            {
                using (DbCommand cmd = DBAccessManager.SqlDBAccess.CreateCommand(CommandType.StoredProcedure, "Proc_DeleteUsers"))
                {
                    cmd.Parameters.Add(DBAccessManager.SqlDBAccess.CreateParameter("@ids", ids, ParameterDirection.Input));
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
            if (p.ID == 0 && p.Description.Length > 500) p.Description.Substring(0, 500);
            if (p.UserStatus != 2)
            {
                if (p.UserName.Length > 50) p.UserName.Substring(0, 50);
                p.PassSalt = LgbCryptography.GenerateSalt();
                p.Password = LgbCryptography.ComputeHash(p.Password, p.PassSalt);
            }
            bool ret = false;
            try
            {
                using (DbCommand cmd = DBAccessManager.SqlDBAccess.CreateCommand(CommandType.StoredProcedure, "Proc_SaveUsers"))
                {
                    cmd.Parameters.Add(DBAccessManager.SqlDBAccess.CreateParameter("@id", p.ID, ParameterDirection.Input));
                    cmd.Parameters.Add(DBAccessManager.SqlDBAccess.CreateParameter("@userName", DBAccess.ToDBValue(p.UserName), ParameterDirection.Input));
                    cmd.Parameters.Add(DBAccessManager.SqlDBAccess.CreateParameter("@password", DBAccess.ToDBValue(p.Password), ParameterDirection.Input));
                    cmd.Parameters.Add(DBAccessManager.SqlDBAccess.CreateParameter("@passSalt", DBAccess.ToDBValue(p.PassSalt), ParameterDirection.Input));
                    cmd.Parameters.Add(DBAccessManager.SqlDBAccess.CreateParameter("@displayName", DBAccess.ToDBValue(p.DisplayName), ParameterDirection.Input));
                    cmd.Parameters.Add(DBAccessManager.SqlDBAccess.CreateParameter("@description", DBAccess.ToDBValue(p.Description), ParameterDirection.Input));
                    cmd.Parameters.Add(DBAccessManager.SqlDBAccess.CreateParameter("@approvedBy", DBAccess.ToDBValue(p.ApprovedBy), ParameterDirection.Input));
                    cmd.Parameters.Add(DBAccessManager.SqlDBAccess.CreateParameter("@rejectedBy", DBAccess.ToDBValue(p.RejectedBy), ParameterDirection.Input));
                    cmd.Parameters.Add(DBAccessManager.SqlDBAccess.CreateParameter("@rejectedReason", DBAccess.ToDBValue(p.RejectedReason), ParameterDirection.Input));
                    cmd.Parameters.Add(DBAccessManager.SqlDBAccess.CreateParameter("@userStatus", p.UserStatus, ParameterDirection.Input));
                    DBAccessManager.SqlDBAccess.ExecuteNonQuery(cmd);
                }
                CacheCleanUtility.ClearCache(userIds: p.ID == 0 ? string.Empty : p.ID.ToString());
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
            string key = string.Format("{0}-{1}", RetrieveUsersByRoleIDDataKey, roleId);
            return CacheManager.GetOrAdd(key, CacheSection.RetrieveIntervalByKey(RetrieveUsersByNameDataKey), k =>
            {
                List<User> Users = new List<User>();
                string sql = "select u.ID, u.UserName, u.DisplayName, case ur.UserID when u.ID then 'checked' else '' end [status] from Users u left join UserRole ur on u.ID = ur.UserID and RoleID = @RoleID where u.ApprovedTime is not null";
                DbCommand cmd = DBAccessManager.SqlDBAccess.CreateCommand(CommandType.Text, sql);
                cmd.Parameters.Add(DBAccessManager.SqlDBAccess.CreateParameter("@RoleID", roleId, ParameterDirection.Input));
                try
                {
                    using (DbDataReader reader = DBAccessManager.SqlDBAccess.ExecuteReader(cmd))
                    {
                        while (reader.Read())
                        {
                            Users.Add(new User()
                            {
                                ID = (int)reader[0],
                                UserName = (string)reader[1],
                                DisplayName = (string)reader[2],
                                Checked = (string)reader[3]
                            });
                        }
                    }
                }
                catch (Exception ex) { ExceptionManager.Publish(ex); }
                return Users;
            }, CacheSection.RetrieveDescByKey(RetrieveUsersByRoleIDDataKey));
        }
        /// <summary>
        /// 通过角色ID保存当前授权用户（插入）
        /// </summary>
        /// <param name="id">角色ID</param>
        /// <param name="value">用户ID数组</param>
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
                        cmd.Parameters.Add(DBAccessManager.SqlDBAccess.CreateParameter("@RoleID", id, ParameterDirection.Input));
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
        /// <param name="roleId"></param>
        /// <returns></returns>
        public static IEnumerable<User> RetrieveUsersByGroupId(int groupId)
        {
            string key = string.Format("{0}-{1}", RetrieveUsersByGroupIDDataKey, groupId);
            return CacheManager.GetOrAdd(key, CacheSection.RetrieveIntervalByKey(RetrieveUsersByGroupIDDataKey), k =>
            {
                List<User> Users = new List<User>();
                string sql = "select u.ID, u.UserName, u.DisplayName, case ur.UserID when u.ID then 'checked' else '' end [status] from Users u left join UserGroup ur on u.ID = ur.UserID and GroupID =@groupId where u.ApprovedTime is not null";
                DbCommand cmd = DBAccessManager.SqlDBAccess.CreateCommand(CommandType.Text, sql);
                cmd.Parameters.Add(DBAccessManager.SqlDBAccess.CreateParameter("@GroupID", groupId, ParameterDirection.Input));
                try
                {
                    using (DbDataReader reader = DBAccessManager.SqlDBAccess.ExecuteReader(cmd))
                    {
                        while (reader.Read())
                        {
                            Users.Add(new User()
                            {
                                ID = (int)reader[0],
                                UserName = (string)reader[1],
                                DisplayName = (string)reader[2],
                                Checked = (string)reader[3]
                            });
                        }
                    }
                }
                catch (Exception ex) { ExceptionManager.Publish(ex); }
                return Users;
            }, CacheSection.RetrieveDescByKey(RetrieveUsersByRoleIDDataKey));
        }
        /// <summary>
        /// 通过部门ID保存当前授权用户（插入）
        /// </summary>
        /// <param name="id">GroupID</param>
        /// <param name="value">用户ID数组</param>
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
                        cmd.Parameters.Add(DBAccessManager.SqlDBAccess.CreateParameter("@GroupID", id, ParameterDirection.Input));
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
                    cmd.Parameters.Add(DBAccessManager.SqlDBAccess.CreateParameter("@iconName", iconName, ParameterDirection.Input));
                    cmd.Parameters.Add(DBAccessManager.SqlDBAccess.CreateParameter("@userName", userName, ParameterDirection.Input));
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
        /// <summary>
        /// 
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public static bool SaveUserInfoByName(User user)
        {
            bool ret = false;
            try
            {
                string sql = "Update Users set DisplayName = @DisplayName where UserName = @userName";
                using (DbCommand cmd = DBAccessManager.SqlDBAccess.CreateCommand(CommandType.Text, sql))
                {
                    cmd.Parameters.Add(DBAccessManager.SqlDBAccess.CreateParameter("@DisplayName", user.DisplayName, ParameterDirection.Input));
                    cmd.Parameters.Add(DBAccessManager.SqlDBAccess.CreateParameter("@userName", user.UserName, ParameterDirection.Input));
                    DBAccessManager.SqlDBAccess.ExecuteNonQuery(cmd);
                    CacheCleanUtility.ClearCache(userIds: string.Empty);
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
        /// 
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public static bool ChangePassword(User user)
        {
            bool ret = false;
            try
            {
                if (BootstrapUser.Authenticate(user.UserName, user.Password))
                {
                    string sql = "Update Users set Password = @Password, PassSalt = @PassSalt where UserName = @userName";
                    user.PassSalt = LgbCryptography.GenerateSalt();
                    user.NewPassword = LgbCryptography.ComputeHash(user.NewPassword, user.PassSalt);
                    using (DbCommand cmd = DBAccessManager.SqlDBAccess.CreateCommand(CommandType.Text, sql))
                    {
                        cmd.Parameters.Add(DBAccessManager.SqlDBAccess.CreateParameter("@Password", user.NewPassword, ParameterDirection.Input));
                        cmd.Parameters.Add(DBAccessManager.SqlDBAccess.CreateParameter("@PassSalt", user.PassSalt, ParameterDirection.Input));
                        cmd.Parameters.Add(DBAccessManager.SqlDBAccess.CreateParameter("@userName", user.UserName, ParameterDirection.Input));
                        DBAccessManager.SqlDBAccess.ExecuteNonQuery(cmd);
                        string key = string.Format("{0}-{1}", RetrieveUsersByNameDataKey, user.UserName);
                        CacheManager.Clear(k => k == key);
                        ret = true;
                    }
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
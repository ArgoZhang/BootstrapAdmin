using Bootstrap.Security;
using Longbow;
using Longbow.Cache;
using Longbow.Data;
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
        internal const string RetrieveUsersDataKey = "BootstrapUser-RetrieveUsers";
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
            return CacheManager.GetOrAdd(RetrieveUsersDataKey, key =>
            {
                List<User> users = new List<User>();
                DbCommand cmd = DBAccessManager.SqlDBAccess.CreateCommand(CommandType.Text, "select ID, UserName, DisplayName, RegisterTime, ApprovedTime, ApprovedBy, Description from Users Where ApprovedTime is not null");

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
                string sql = "select ID, UserName, DisplayName, RegisterTime, [Description] from Users Where ApprovedTime is null order by RegisterTime desc";
                List<User> users = new List<User>();
                DbCommand cmd = DBAccessManager.SqlDBAccess.CreateCommand(CommandType.Text, sql);
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
                return users;
            });
        }
        /// <summary>
        /// 删除用户
        /// </summary>
        /// <param name="value"></param>
        public static bool DeleteUser(IEnumerable<int> value)
        {
            bool ret = false;
            var ids = string.Join(",", value);
            using (DbCommand cmd = DBAccessManager.SqlDBAccess.CreateCommand(CommandType.StoredProcedure, "Proc_DeleteUsers"))
            {
                cmd.Parameters.Add(DBAccessManager.SqlDBAccess.CreateParameter("@ids", ids));
                ret = DBAccessManager.SqlDBAccess.ExecuteNonQuery(cmd) == -1;
                if (ret) CacheCleanUtility.ClearCache(userIds: value);
            }
            return ret;
        }
        /// <summary>
        /// 保存新建
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        public static bool SaveUser(User p)
        {
            var ret = false;
            if (p.Id == 0 && p.Description.Length > 500) p.Description = p.Description.Substring(0, 500);
            if (p.UserName.Length > 50) p.UserName = p.UserName.Substring(0, 50);
            p.PassSalt = LgbCryptography.GenerateSalt();
            p.Password = LgbCryptography.ComputeHash(p.Password, p.PassSalt);
            using (DbCommand cmd = DBAccessManager.SqlDBAccess.CreateCommand(CommandType.StoredProcedure, "Proc_SaveUsers"))
            {
                cmd.Parameters.Add(DBAccessManager.SqlDBAccess.CreateParameter("@userName", p.UserName));
                cmd.Parameters.Add(DBAccessManager.SqlDBAccess.CreateParameter("@password", p.Password));
                cmd.Parameters.Add(DBAccessManager.SqlDBAccess.CreateParameter("@passSalt", p.PassSalt));
                cmd.Parameters.Add(DBAccessManager.SqlDBAccess.CreateParameter("@displayName", p.DisplayName));
                cmd.Parameters.Add(DBAccessManager.SqlDBAccess.CreateParameter("@approvedBy", DBAccessFactory.ToDBValue(p.ApprovedBy)));
                object approvedTime = p.ApprovedTime;
                if (p.ApprovedTime == DateTime.MinValue) approvedTime = DBNull.Value;
                cmd.Parameters.Add(DBAccessManager.SqlDBAccess.CreateParameter("@approvedTime", approvedTime));
                cmd.Parameters.Add(DBAccessManager.SqlDBAccess.CreateParameter("@description", p.Description));
                ret = DBAccessManager.SqlDBAccess.ExecuteNonQuery(cmd) == -1;
                if (ret) CacheCleanUtility.ClearCache(userIds: p.Id == 0 ? new List<int>() : new List<int>() { p.Id });
            }
            return ret;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <param name="password"></param>
        /// <param name="displayName"></param>
        /// <returns></returns>
        public static bool UpdateUser(int id, string password, string displayName)
        {
            bool ret = false;
            string sql = "Update Users set Password = @Password, PassSalt = @PassSalt, DisplayName = @DisplayName where ID = @id";
            var passSalt = LgbCryptography.GenerateSalt();
            var newPassword = LgbCryptography.ComputeHash(password, passSalt);
            using (DbCommand cmd = DBAccessManager.SqlDBAccess.CreateCommand(CommandType.Text, sql))
            {
                cmd.Parameters.Add(DBAccessManager.SqlDBAccess.CreateParameter("@id", id));
                cmd.Parameters.Add(DBAccessManager.SqlDBAccess.CreateParameter("@DisplayName", displayName));
                cmd.Parameters.Add(DBAccessManager.SqlDBAccess.CreateParameter("@Password", newPassword));
                cmd.Parameters.Add(DBAccessManager.SqlDBAccess.CreateParameter("@PassSalt", passSalt));
                ret = DBAccessManager.SqlDBAccess.ExecuteNonQuery(cmd) == 1;
                if (ret) CacheCleanUtility.ClearCache(userIds: id == 0 ? new List<int>() : new List<int>() { id });
            }
            return ret;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <param name="approvedBy"></param>
        /// <returns></returns>
        public static bool ApproveUser(int id, string approvedBy)
        {
            var ret = false;
            var sql = "update Users set ApprovedTime = GETDATE(), ApprovedBy = @approvedBy where ID = @id";
            using (DbCommand cmd = DBAccessManager.SqlDBAccess.CreateCommand(CommandType.Text, sql))
            {
                cmd.Parameters.Add(DBAccessManager.SqlDBAccess.CreateParameter("@id", id));
                cmd.Parameters.Add(DBAccessManager.SqlDBAccess.CreateParameter("@approvedBy", approvedBy));
                ret = DBAccessManager.SqlDBAccess.ExecuteNonQuery(cmd) == 1;
                if (ret) CacheCleanUtility.ClearCache(userIds: new List<int>() { id });
            }
            return ret;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="password"></param>
        /// <param name="newPass"></param>
        /// <returns></returns>
        public static bool ChangePassword(string userName, string password, string newPass)
        {
            bool ret = false;
            if (BootstrapUser.Authenticate(userName, password))
            {
                string sql = "Update Users set Password = @Password, PassSalt = @PassSalt where UserName = @userName";
                var passSalt = LgbCryptography.GenerateSalt();
                var newPassword = LgbCryptography.ComputeHash(newPass, passSalt);
                using (DbCommand cmd = DBAccessManager.SqlDBAccess.CreateCommand(CommandType.Text, sql))
                {
                    cmd.Parameters.Add(DBAccessManager.SqlDBAccess.CreateParameter("@Password", newPassword));
                    cmd.Parameters.Add(DBAccessManager.SqlDBAccess.CreateParameter("@PassSalt", passSalt));
                    cmd.Parameters.Add(DBAccessManager.SqlDBAccess.CreateParameter("@userName", userName));
                    ret = DBAccessManager.SqlDBAccess.ExecuteNonQuery(cmd) == 1;
                }
            }
            return ret;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <param name="rejectBy"></param>
        /// <param name="reason"></param>
        /// <returns></returns>
        public static bool RejectUser(int id, string rejectBy)
        {
            var ret = false;
            using (DbCommand cmd = DBAccessManager.SqlDBAccess.CreateCommand(CommandType.StoredProcedure, "Proc_RejectUsers"))
            {
                cmd.Parameters.Add(DBAccessManager.SqlDBAccess.CreateParameter("@id", id));
                cmd.Parameters.Add(DBAccessManager.SqlDBAccess.CreateParameter("@rejectedBy", rejectBy));
                cmd.Parameters.Add(DBAccessManager.SqlDBAccess.CreateParameter("@rejectedReason", "未填写"));
                ret = DBAccessManager.SqlDBAccess.ExecuteNonQuery(cmd) == -1;
                if (ret) CacheCleanUtility.ClearCache(userIds: new List<int>() { id });
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
                return users;
            }, RetrieveUsersByRoleIdDataKey);
        }
        /// <summary>
        /// 通过角色ID保存当前授权用户（插入）
        /// </summary>
        /// <param name="id">角色ID</param>
        /// <param name="userIds">用户ID数组</param>
        /// <returns></returns>
        public static bool SaveUsersByRoleId(int id, IEnumerable<int> userIds)
        {
            bool ret = false;
            DataTable dt = new DataTable();
            dt.Columns.Add("RoleID", typeof(int));
            dt.Columns.Add("UserID", typeof(int));
            userIds.ToList().ForEach(userId => dt.Rows.Add(id, userId));
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
                    CacheCleanUtility.ClearCache(userIds: userIds, roleIds: new List<int>() { id });
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
                return users;
            }, RetrieveUsersByRoleIdDataKey);
        }
        /// <summary>
        /// 通过部门ID保存当前授权用户（插入）
        /// </summary>
        /// <param name="id">GroupID</param>
        /// <param name="userIds">用户ID数组</param>
        /// <returns></returns>
        public static bool SaveUsersByGroupId(int id, IEnumerable<int> userIds)
        {
            bool ret = false;
            DataTable dt = new DataTable();
            dt.Columns.Add("UserID", typeof(int));
            dt.Columns.Add("GroupID", typeof(int));
            userIds.ToList().ForEach(userId => dt.Rows.Add(userId, id));
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
                    CacheCleanUtility.ClearCache(userIds: userIds, groupIds: new List<int>() { id });
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
        /// 根据用户名修改用户头像
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="iconName"></param>
        /// <returns></returns>
        public static bool SaveUserIconByName(string userName, string iconName)
        {
            bool ret = false;
            string sql = "Update Users set Icon = @iconName where UserName = @userName";
            using (DbCommand cmd = DBAccessManager.SqlDBAccess.CreateCommand(CommandType.Text, sql))
            {
                cmd.Parameters.Add(DBAccessManager.SqlDBAccess.CreateParameter("@iconName", iconName));
                cmd.Parameters.Add(DBAccessManager.SqlDBAccess.CreateParameter("@userName", userName));
                ret = DBAccessManager.SqlDBAccess.ExecuteNonQuery(cmd) == 1;
                if (ret) CacheCleanUtility.ClearCache(cacheKey: $"{RetrieveUsersDataKey}*");
            }
            return ret;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="displayName"></param>
        /// <returns></returns>
        public static bool SaveDisplayName(string userName, string displayName)
        {
            bool ret = false;
            string sql = "Update Users set DisplayName = @DisplayName where UserName = @userName";
            using (DbCommand cmd = DBAccessManager.SqlDBAccess.CreateCommand(CommandType.Text, sql))
            {
                cmd.Parameters.Add(DBAccessManager.SqlDBAccess.CreateParameter("@DisplayName", displayName));
                cmd.Parameters.Add(DBAccessManager.SqlDBAccess.CreateParameter("@userName", userName));
                ret = DBAccessManager.SqlDBAccess.ExecuteNonQuery(cmd) == 1;
                if (ret) CacheCleanUtility.ClearCache(cacheKey: $"{RetrieveUsersDataKey}*");
            }
            return ret;
        }
        /// <summary>
        /// 根据用户名更改用户皮肤
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="cssName"></param>
        /// <returns></returns>
        public static bool SaveUserCssByName(string userName, string cssName)
        {
            bool ret = false;
            string sql = "Update Users set Css = @cssName where UserName = @userName";
            using (DbCommand cmd = DBAccessManager.SqlDBAccess.CreateCommand(CommandType.Text, sql))
            {
                cmd.Parameters.Add(DBAccessManager.SqlDBAccess.CreateParameter("@cssName", DBAccessFactory.ToDBValue(cssName)));
                cmd.Parameters.Add(DBAccessManager.SqlDBAccess.CreateParameter("@userName", userName));
                ret = DBAccessManager.SqlDBAccess.ExecuteNonQuery(cmd) == 1;
                if (ret) CacheCleanUtility.ClearCache(cacheKey: $"{RetrieveUsersDataKey}*");
            }
            return ret;
        }
    }
}

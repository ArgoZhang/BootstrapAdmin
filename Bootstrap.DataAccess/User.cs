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
    /// 用户表实体类
    /// </summary>
    public class User : BootstrapUser
    {
        public const string RetrieveUsersDataKey = "BootstrapUser-RetrieveUsers";
        public const string RetrieveUsersByRoleIdDataKey = "BootstrapUser-RetrieveUsersByRoleId";
        public const string RetrieveUsersByGroupIdDataKey = "BootstrapUser-RetrieveUsersByGroupId";
        public const string RetrieveNewUsersDataKey = "UserHelper-RetrieveNewUsers";
        protected const string RetrieveUsersByNameDataKey = "BootstrapUser-RetrieveUsersByName";
        /// <summary>
        /// 获得/设置 用户主键ID
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// 获取/设置 密码
        /// </summary>
        public string Password { get; set; }
        /// <summary>
        /// 获取/设置 密码盐
        /// </summary>
        public string PassSalt { get; set; }
        /// <summary>
        /// 获取/设置 角色用户关联状态 checked 标示已经关联 '' 标示未关联
        /// </summary>
        public string Checked { get; set; }
        /// <summary>
        /// 获得/设置 用户注册时间
        /// </summary>
        public DateTime RegisterTime { get; set; }
        /// <summary>
        /// 获得/设置 用户被批复时间
        /// </summary>
        public DateTime ApprovedTime { get; set; }
        /// <summary>
        /// 获得/设置 用户批复人
        /// </summary>
        public string ApprovedBy { get; set; }
        /// <summary>
        /// 获得/设置 用户的申请理由
        /// </summary>
        public string Description { get; set; }
        /// <summary>
        /// 获得/设置 用户当前状态 0 表示管理员注册用户 1 表示用户注册 2 表示更改密码 3 表示更改个人皮肤 4 表示更改显示名称 5 批复新用户注册操作
        /// </summary>
        public UserStates UserStatus { get; set; }
        /// <summary>
        /// 获得/设置 通知描述 2分钟内为刚刚
        /// </summary>
        public string Period { get; set; }
        /// <summary>
        /// 获得/设置 新密码
        /// </summary>
        public string NewPassword { get; set; }
        /// <summary>
        /// 验证用户登陆账号与密码正确
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        public virtual bool Authenticate(string userName, string password)
        {
            if (string.IsNullOrEmpty(userName) && string.IsNullOrEmpty(password)) return false;
            string oldPassword = null;
            string passwordSalt = null;
            string sql = "select [Password], PassSalt from Users where ApprovedTime is not null and UserName = @UserName";
            var db = DbAccessManager.DBAccess;
            using (DbCommand cmd = db.CreateCommand(CommandType.Text, sql))
            {
                cmd.Parameters.Add(db.CreateParameter("@UserName", userName));
                using (DbDataReader reader = db.ExecuteReader(cmd))
                {
                    if (reader.Read())
                    {
                        oldPassword = (string)reader[0];
                        passwordSalt = (string)reader[1];
                    }
                }
            }
            return !string.IsNullOrEmpty(passwordSalt) && oldPassword == LgbCryptography.ComputeHash(password, passwordSalt);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="password"></param>
        /// <param name="newPass"></param>
        /// <returns></returns>
        public virtual bool ChangePassword(string userName, string password, string newPass)
        {
            bool ret = false;
            if (Authenticate(userName, password))
            {
                string sql = "Update Users set Password = @Password, PassSalt = @PassSalt where UserName = @userName";
                var passSalt = LgbCryptography.GenerateSalt();
                var newPassword = LgbCryptography.ComputeHash(newPass, passSalt);
                using (DbCommand cmd = DbAccessManager.DBAccess.CreateCommand(CommandType.Text, sql))
                {
                    cmd.Parameters.Add(DbAccessManager.DBAccess.CreateParameter("@Password", newPassword));
                    cmd.Parameters.Add(DbAccessManager.DBAccess.CreateParameter("@PassSalt", passSalt));
                    cmd.Parameters.Add(DbAccessManager.DBAccess.CreateParameter("@userName", userName));
                    ret = DbAccessManager.DBAccess.ExecuteNonQuery(cmd) == 1;
                }
            }
            return ret;
        }
        /// <summary>
        /// 查询所有用户
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public virtual IEnumerable<User> RetrieveUsers()
        {
            return CacheManager.GetOrAdd(RetrieveUsersDataKey, key =>
            {
                List<User> users = new List<User>();
                DbCommand cmd = DbAccessManager.DBAccess.CreateCommand(CommandType.Text, "select ID, UserName, DisplayName, RegisterTime, ApprovedTime, ApprovedBy, Description from Users Where ApprovedTime is not null");

                using (DbDataReader reader = DbAccessManager.DBAccess.ExecuteReader(cmd))
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
        public virtual IEnumerable<User> RetrieveNewUsers()
        {
            return CacheManager.GetOrAdd(RetrieveNewUsersDataKey, key =>
            {
                string sql = "select ID, UserName, DisplayName, RegisterTime, [Description] from Users Where ApprovedTime is null order by RegisterTime desc";
                List<User> users = new List<User>();
                DbCommand cmd = DbAccessManager.DBAccess.CreateCommand(CommandType.Text, sql);
                using (DbDataReader reader = DbAccessManager.DBAccess.ExecuteReader(cmd))
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
        public virtual bool DeleteUser(IEnumerable<int> value)
        {
            bool ret = false;
            var ids = string.Join(",", value);
            using (DbCommand cmd = DbAccessManager.DBAccess.CreateCommand(CommandType.StoredProcedure, "Proc_DeleteUsers"))
            {
                cmd.Parameters.Add(DbAccessManager.DBAccess.CreateParameter("@ids", ids));
                ret = DbAccessManager.DBAccess.ExecuteNonQuery(cmd) == -1;
                if (ret) CacheCleanUtility.ClearCache(userIds: value);
            }
            return ret;
        }
        /// <summary>
        /// 保存新建
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        public virtual bool SaveUser(User p)
        {
            var ret = false;
            if (p.Id == 0 && p.Description.Length > 500) p.Description = p.Description.Substring(0, 500);
            if (p.UserName.Length > 50) p.UserName = p.UserName.Substring(0, 50);
            p.PassSalt = LgbCryptography.GenerateSalt();
            p.Password = LgbCryptography.ComputeHash(p.Password, p.PassSalt);
            using (DbCommand cmd = DbAccessManager.DBAccess.CreateCommand(CommandType.StoredProcedure, "Proc_SaveUsers"))
            {
                cmd.Parameters.Add(DbAccessManager.DBAccess.CreateParameter("@userName", p.UserName));
                cmd.Parameters.Add(DbAccessManager.DBAccess.CreateParameter("@password", p.Password));
                cmd.Parameters.Add(DbAccessManager.DBAccess.CreateParameter("@passSalt", p.PassSalt));
                cmd.Parameters.Add(DbAccessManager.DBAccess.CreateParameter("@displayName", p.DisplayName));
                cmd.Parameters.Add(DbAccessManager.DBAccess.CreateParameter("@approvedBy", DbAccessFactory.ToDBValue(p.ApprovedBy)));
                cmd.Parameters.Add(DbAccessManager.DBAccess.CreateParameter("@description", p.Description));
                ret = DbAccessManager.DBAccess.ExecuteNonQuery(cmd) == -1;
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
        public virtual bool UpdateUser(int id, string password, string displayName)
        {
            bool ret = false;
            string sql = "Update Users set Password = @Password, PassSalt = @PassSalt, DisplayName = @DisplayName where ID = @id";
            var passSalt = LgbCryptography.GenerateSalt();
            var newPassword = LgbCryptography.ComputeHash(password, passSalt);
            using (DbCommand cmd = DbAccessManager.DBAccess.CreateCommand(CommandType.Text, sql))
            {
                cmd.Parameters.Add(DbAccessManager.DBAccess.CreateParameter("@id", id));
                cmd.Parameters.Add(DbAccessManager.DBAccess.CreateParameter("@DisplayName", displayName));
                cmd.Parameters.Add(DbAccessManager.DBAccess.CreateParameter("@Password", newPassword));
                cmd.Parameters.Add(DbAccessManager.DBAccess.CreateParameter("@PassSalt", passSalt));
                ret = DbAccessManager.DBAccess.ExecuteNonQuery(cmd) == 1;
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
        public virtual bool ApproveUser(int id, string approvedBy)
        {
            var ret = false;
            var sql = "update Users set ApprovedTime = GETDATE(), ApprovedBy = @approvedBy where ID = @id";
            using (DbCommand cmd = DbAccessManager.DBAccess.CreateCommand(CommandType.Text, sql))
            {
                cmd.Parameters.Add(DbAccessManager.DBAccess.CreateParameter("@id", id));
                cmd.Parameters.Add(DbAccessManager.DBAccess.CreateParameter("@approvedBy", approvedBy));
                ret = DbAccessManager.DBAccess.ExecuteNonQuery(cmd) == 1;
                if (ret) CacheCleanUtility.ClearCache(userIds: new List<int>() { id });
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
        public virtual bool RejectUser(int id, string rejectBy)
        {
            var ret = false;
            using (DbCommand cmd = DbAccessManager.DBAccess.CreateCommand(CommandType.StoredProcedure, "Proc_RejectUsers"))
            {
                cmd.Parameters.Add(DbAccessManager.DBAccess.CreateParameter("@id", id));
                cmd.Parameters.Add(DbAccessManager.DBAccess.CreateParameter("@rejectedBy", rejectBy));
                cmd.Parameters.Add(DbAccessManager.DBAccess.CreateParameter("@rejectedReason", "未填写"));
                ret = DbAccessManager.DBAccess.ExecuteNonQuery(cmd) == -1;
                if (ret) CacheCleanUtility.ClearCache(userIds: new List<int>() { id });
            }
            return ret;
        }
        /// <summary>
        /// 通过roleId获取所有用户
        /// </summary>
        /// <param name="roleId"></param>
        /// <returns></returns>
        public virtual IEnumerable<User> RetrieveUsersByRoleId(int roleId)
        {
            string key = string.Format("{0}-{1}", RetrieveUsersByRoleIdDataKey, roleId);
            return CacheManager.GetOrAdd(key, k =>
            {
                List<User> users = new List<User>();
                string sql = "select u.ID, u.UserName, u.DisplayName, case ur.UserID when u.ID then 'checked' else '' end [status] from Users u left join UserRole ur on u.ID = ur.UserID and RoleID = @RoleID where u.ApprovedTime is not null";
                DbCommand cmd = DbAccessManager.DBAccess.CreateCommand(CommandType.Text, sql);
                cmd.Parameters.Add(DbAccessManager.DBAccess.CreateParameter("@RoleID", roleId));
                using (DbDataReader reader = DbAccessManager.DBAccess.ExecuteReader(cmd))
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
        public virtual bool SaveUsersByRoleId(int id, IEnumerable<int> userIds)
        {
            bool ret = false;
            DataTable dt = new DataTable();
            dt.Columns.Add("RoleID", typeof(int));
            dt.Columns.Add("UserID", typeof(int));
            userIds.ToList().ForEach(userId => dt.Rows.Add(id, userId));
            using (TransactionPackage transaction = DbAccessManager.DBAccess.BeginTransaction())
            {
                try
                {
                    //删除用户角色表该角色所有的用户
                    string sql = "delete from UserRole where RoleID=@RoleID";
                    using (DbCommand cmd = DbAccessManager.DBAccess.CreateCommand(CommandType.Text, sql))
                    {
                        cmd.Parameters.Add(DbAccessManager.DBAccess.CreateParameter("@RoleID", id));
                        DbAccessManager.DBAccess.ExecuteNonQuery(cmd, transaction);
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
        public virtual IEnumerable<User> RetrieveUsersByGroupId(int groupId)
        {
            string key = string.Format("{0}-{1}", RetrieveUsersByGroupIdDataKey, groupId);
            return CacheManager.GetOrAdd(key, k =>
            {
                List<User> users = new List<User>();
                string sql = "select u.ID, u.UserName, u.DisplayName, case ur.UserID when u.ID then 'checked' else '' end [status] from Users u left join UserGroup ur on u.ID = ur.UserID and GroupID =@groupId where u.ApprovedTime is not null";
                DbCommand cmd = DbAccessManager.DBAccess.CreateCommand(CommandType.Text, sql);
                cmd.Parameters.Add(DbAccessManager.DBAccess.CreateParameter("@GroupID", groupId));
                using (DbDataReader reader = DbAccessManager.DBAccess.ExecuteReader(cmd))
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
        public virtual bool SaveUsersByGroupId(int id, IEnumerable<int> userIds)
        {
            bool ret = false;
            DataTable dt = new DataTable();
            dt.Columns.Add("UserID", typeof(int));
            dt.Columns.Add("GroupID", typeof(int));
            userIds.ToList().ForEach(userId => dt.Rows.Add(userId, id));
            using (TransactionPackage transaction = DbAccessManager.DBAccess.BeginTransaction())
            {
                try
                {
                    //删除用户角色表该角色所有的用户
                    string sql = "delete from UserGroup where GroupID = @GroupID";
                    using (DbCommand cmd = DbAccessManager.DBAccess.CreateCommand(CommandType.Text, sql))
                    {
                        cmd.Parameters.Add(DbAccessManager.DBAccess.CreateParameter("@GroupID", id));
                        DbAccessManager.DBAccess.ExecuteNonQuery(cmd, transaction);
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
        public virtual bool SaveUserIconByName(string userName, string iconName)
        {
            bool ret = false;
            string sql = "Update Users set Icon = @iconName where UserName = @userName";
            using (DbCommand cmd = DbAccessManager.DBAccess.CreateCommand(CommandType.Text, sql))
            {
                cmd.Parameters.Add(DbAccessManager.DBAccess.CreateParameter("@iconName", iconName));
                cmd.Parameters.Add(DbAccessManager.DBAccess.CreateParameter("@userName", userName));
                ret = DbAccessManager.DBAccess.ExecuteNonQuery(cmd) == 1;
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
        public virtual bool SaveDisplayName(string userName, string displayName)
        {
            bool ret = false;
            string sql = "Update Users set DisplayName = @DisplayName where UserName = @userName";
            using (DbCommand cmd = DbAccessManager.DBAccess.CreateCommand(CommandType.Text, sql))
            {
                cmd.Parameters.Add(DbAccessManager.DBAccess.CreateParameter("@DisplayName", displayName));
                cmd.Parameters.Add(DbAccessManager.DBAccess.CreateParameter("@userName", userName));
                ret = DbAccessManager.DBAccess.ExecuteNonQuery(cmd) == 1;
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
        public virtual bool SaveUserCssByName(string userName, string cssName)
        {
            bool ret = false;
            string sql = "Update Users set Css = @cssName where UserName = @userName";
            using (DbCommand cmd = DbAccessManager.DBAccess.CreateCommand(CommandType.Text, sql))
            {
                cmd.Parameters.Add(DbAccessManager.DBAccess.CreateParameter("@cssName", DbAccessFactory.ToDBValue(cssName)));
                cmd.Parameters.Add(DbAccessManager.DBAccess.CreateParameter("@userName", userName));
                ret = DbAccessManager.DBAccess.ExecuteNonQuery(cmd) == 1;
                if (ret) CacheCleanUtility.ClearCache(cacheKey: $"{RetrieveUsersDataKey}*");
            }
            return ret;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="userName"></param>
        /// <returns></returns>
        public virtual BootstrapUser RetrieveUserByUserName(string userName)
        {
            var key = string.Format("{0}-{1}", RetrieveUsersByNameDataKey, userName);
            return CacheManager.GetOrAdd(key, k =>
            {
                BootstrapUser user = null;
                var sql = "select UserName, DisplayName, case isnull(d.Code, '') when '' then '~/images/uploader/' else d.Code end + Icon Icon, u.Css from Users u left join Dicts d on d.Define = '0' and d.Category = N'头像地址' and Name = N'头像路径' where ApprovedTime is not null and UserName = @UserName";
                var db = DbAccessManager.DBAccess;
                var cmd = db.CreateCommand(CommandType.Text, sql);
                cmd.Parameters.Add(db.CreateParameter("@UserName", userName));
                using (DbDataReader reader = db.ExecuteReader(cmd))
                {
                    if (reader.Read())
                    {
                        user = new BootstrapUser
                        {
                            UserName = (string)reader[0],
                            DisplayName = (string)reader[1],
                            Icon = (string)reader[2],
                            Css = reader.IsDBNull(3) ? string.Empty : (string)reader[3]
                        };
                    }
                }
                return user;
            }, RetrieveUsersByNameDataKey);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return string.Format("{0} ({1})", UserName, DisplayName);
        }
    }
    /// <summary>
    /// 
    /// </summary>
    public enum UserStates
    {
        /// <summary>
        /// 
        /// </summary>
        ChangePassword,
        /// <summary>
        /// 
        /// </summary>
        ChangeTheme,
        /// <summary>
        /// 
        /// </summary>
        ChangeDisplayName,
        /// <summary>
        /// 
        /// </summary>
        ApproveUser,
        /// <summary>
        /// 
        /// </summary>
        RejectUser
    }
}

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


namespace Bootstrap.DataAccess.SQLite
{
    /// <summary>
    /// 用户表实体类
    /// </summary>
    public class User : DataAccess.User
    {
        /// <summary>
        /// 查询所有用户
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public override IEnumerable<DataAccess.User> RetrieveUsers()
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
                            Id = LgbConvert.ReadValue(reader[0], 0),
                            UserName = (string)reader[1],
                            DisplayName = (string)reader[2],
                            RegisterTime = LgbConvert.ReadValue(reader[3], DateTime.MinValue),
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
        public override IEnumerable<DataAccess.User> RetrieveNewUsers()
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
                            Id = LgbConvert.ReadValue(reader[0], 0),
                            UserName = (string)reader[1],
                            DisplayName = (string)reader[2],
                            RegisterTime = LgbConvert.ReadValue(reader[3], DateTime.MinValue),
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
        public override bool DeleteUser(IEnumerable<int> value)
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
        public override bool SaveUser(DataAccess.User p)
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
        public override bool UpdateUser(int id, string password, string displayName)
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
        public override bool ApproveUser(int id, string approvedBy)
        {
            var ret = false;
            var sql = "update Users set ApprovedTime = datetime('now', 'localtime'), ApprovedBy = @approvedBy where ID = @id";
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
        public override bool RejectUser(int id, string rejectBy)
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
        public override IEnumerable<DataAccess.User> RetrieveUsersByRoleId(int roleId)
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
                            Id = LgbConvert.ReadValue(reader[0], 0),
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
        public override bool SaveUsersByRoleId(int id, IEnumerable<int> userIds)
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
        public override IEnumerable<DataAccess.User> RetrieveUsersByGroupId(int groupId)
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
                            Id = LgbConvert.ReadValue(reader[0], 0),
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
        public override bool SaveUsersByGroupId(int id, IEnumerable<int> userIds)
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
        public override bool SaveUserIconByName(string userName, string iconName)
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
        public override bool SaveDisplayName(string userName, string displayName)
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
        public override bool SaveUserCssByName(string userName, string cssName)
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
        /// <param name="name"></param>
        /// <returns></returns>
        public override BootstrapUser RetrieveUserByUserName(string userName)
        {
            var key = string.Format("{0}-{1}", RetrieveUsersByNameDataKey, userName);
            return CacheManager.GetOrAdd(key, k =>
            {
                BootstrapUser user = null;
                var sql = "select UserName, DisplayName, case ifnull(d.Code, '') when '' then '~/images/uploader/' else d.Code end || ifnull(Icon, 'default.jpg') Icon, u.Css from Users u left join Dicts d on d.Define = '0' and d.Category = '头像地址' and Name = '头像路径' where ApprovedTime is not null and UserName = @UserName";
                var cmd = DbAccessManager.DBAccess.CreateCommand(CommandType.Text, sql);
                cmd.Parameters.Add(DbAccessManager.DBAccess.CreateParameter("@UserName", userName));
                using (DbDataReader reader = DbAccessManager.DBAccess.ExecuteReader(cmd))
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
    }
}

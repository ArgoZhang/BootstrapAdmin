using Bootstrap.Security;
using Longbow.Data;
using Longbow.Security.Cryptography;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;

namespace Bootstrap.DataAccess.SQLite
{
    /// <summary>
    /// 用户表实体类
    /// </summary>
    public class User : DataAccess.User
    {
        /// <summary>
        /// <summary>
        /// 删除用户
        /// </summary>
        /// <param name="value"></param>
        public override bool DeleteUser(IEnumerable<int> value)
        {
            bool ret = false;
            var ids = string.Join(",", value);
            using (TransactionPackage transaction = DbAccessManager.DBAccess.BeginTransaction())
            {
                try
                {
                    using (DbCommand cmd = DbAccessManager.DBAccess.CreateCommand(CommandType.Text, $"Delete from UserRole where UserID in ({ids})"))
                    {
                        DbAccessManager.DBAccess.ExecuteNonQuery(cmd, transaction);

                        cmd.CommandText = $"delete from UserGroup where UserID in ({ids})";
                        DbAccessManager.DBAccess.ExecuteNonQuery(cmd, transaction);

                        cmd.CommandText = $"delete from Users where ID in ({ids})";
                        DbAccessManager.DBAccess.ExecuteNonQuery(cmd, transaction);

                        transaction.CommitTransaction();
                        CacheCleanUtility.ClearCache(userIds: value);
                        ret = true;
                    }
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

            using (TransactionPackage transaction = DbAccessManager.DBAccess.BeginTransaction())
            {
                try
                {
                    using (DbCommand cmd = DbAccessManager.DBAccess.CreateCommand(CommandType.Text, "select UserName from Users Where UserName = @userName"))
                    {
                        cmd.Parameters.Add(DbAccessManager.DBAccess.CreateParameter("@userName", p.UserName));
                        var un = DbAccessManager.DBAccess.ExecuteScalar(cmd, transaction);
                        if (DbAdapterManager.ToObjectValue(un) == null)
                        {
                            cmd.CommandText = "Insert Into Users (UserName, [Password], PassSalt, DisplayName, RegisterTime, ApprovedBy, ApprovedTime, [Description]) values (@userName, @password, @passSalt, @displayName, datetime('now', 'localtime'), @approvedBy, datetime('now', 'localtime'), @description)";
                            cmd.Parameters.Add(DbAccessManager.DBAccess.CreateParameter("@password", p.Password));
                            cmd.Parameters.Add(DbAccessManager.DBAccess.CreateParameter("@passSalt", p.PassSalt));
                            cmd.Parameters.Add(DbAccessManager.DBAccess.CreateParameter("@displayName", p.DisplayName));
                            cmd.Parameters.Add(DbAccessManager.DBAccess.CreateParameter("@approvedBy", DbAdapterManager.ToDBValue(p.ApprovedBy)));
                            cmd.Parameters.Add(DbAccessManager.DBAccess.CreateParameter("@description", p.Description));
                            DbAccessManager.DBAccess.ExecuteNonQuery(cmd, transaction);

                            cmd.CommandText = $"insert into UserRole (UserID, RoleID) select ID, (select ID from Roles where RoleName = 'Default') RoleId from Users where UserName = '{p.UserName}'";
                            cmd.Parameters.Clear();
                            DbAccessManager.DBAccess.ExecuteNonQuery(cmd, transaction);

                            transaction.CommitTransaction();
                            CacheCleanUtility.ClearCache(userIds: p.Id == 0 ? new List<int>() : new List<int>() { p.Id });
                            ret = true;
                        }
                    }
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
            using (TransactionPackage transaction = DbAccessManager.DBAccess.BeginTransaction())
            {
                try
                {
                    using (DbCommand cmd = DbAccessManager.DBAccess.CreateCommand(CommandType.Text, $"insert into RejectUsers (UserName, DisplayName, RegisterTime, RejectedBy, RejectedTime, RejectedReason) select UserName, DisplayName, Registertime, '{rejectBy}', datetime('now', 'localtime'), '未填写' from Users where ID = {id}"))
                    {
                        DbAccessManager.DBAccess.ExecuteNonQuery(cmd, transaction);

                        cmd.CommandText = $"delete from UserRole where UserId = {id}";
                        DbAccessManager.DBAccess.ExecuteNonQuery(cmd, transaction);

                        cmd.CommandText = $"delete from UserGroup where UserId = {id}";
                        DbAccessManager.DBAccess.ExecuteNonQuery(cmd, transaction);

                        cmd.CommandText = $"delete from users where ID = {id}";
                        DbAccessManager.DBAccess.ExecuteNonQuery(cmd, transaction);

                        transaction.CommitTransaction();
                        CacheCleanUtility.ClearCache(userIds: new List<int>() { id });
                        ret = true;
                    }
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
        /// 通过角色ID保存当前授权用户（插入）
        /// </summary>
        /// <param name="roleId">角色ID</param>
        /// <param name="userIds">用户ID数组</param>
        /// <returns></returns>
        public override bool SaveUsersByRoleId(int roleId, IEnumerable<int> userIds)
        {
            bool ret = false;
            DataTable dt = new DataTable();
            dt.Columns.Add("RoleID", typeof(int));
            dt.Columns.Add("UserID", typeof(int));
            userIds.ToList().ForEach(userId => dt.Rows.Add(roleId, userId));
            using (TransactionPackage transaction = DbAccessManager.DBAccess.BeginTransaction())
            {
                try
                {
                    //删除用户角色表该角色所有的用户
                    string sql = $"delete from UserRole where RoleID = {roleId}";
                    using (DbCommand cmd = DbAccessManager.DBAccess.CreateCommand(CommandType.Text, sql))
                    {
                        DbAccessManager.DBAccess.ExecuteNonQuery(cmd, transaction);
                        //批插入用户角色表
                        userIds.ToList().ForEach(uId =>
                        {
                            cmd.CommandText = $"Insert Into UserRole (UserID, RoleID) Values ( {uId}, {roleId})";
                            DbAccessManager.DBAccess.ExecuteNonQuery(cmd, transaction);
                        });
                        transaction.CommitTransaction();
                    }
                    CacheCleanUtility.ClearCache(userIds: userIds, roleIds: new List<int>() { roleId });
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
        /// 通过部门ID保存当前授权用户（插入）
        /// </summary>
        /// <param name="groupId">GroupID</param>
        /// <param name="userIds">用户ID数组</param>
        /// <returns></returns>
        public override bool SaveUsersByGroupId(int groupId, IEnumerable<int> userIds)
        {
            bool ret = false;
            using (TransactionPackage transaction = DbAccessManager.DBAccess.BeginTransaction())
            {
                try
                {
                    //删除用户角色表该角色所有的用户
                    string sql = $"delete from UserGroup where GroupID = {groupId}";
                    using (DbCommand cmd = DbAccessManager.DBAccess.CreateCommand(CommandType.Text, sql))
                    {
                        DbAccessManager.DBAccess.ExecuteNonQuery(cmd, transaction);
                        //批插入用户角色表
                        userIds.ToList().ForEach(uId =>
                        {
                            cmd.CommandText = $"Insert Into UserGroup (UserID, GroupID) Values ( {uId}, {groupId})";
                            DbAccessManager.DBAccess.ExecuteNonQuery(cmd, transaction);
                        });
                        transaction.CommitTransaction();
                    }
                    CacheCleanUtility.ClearCache(userIds: userIds, groupIds: new List<int>() { groupId });
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
        /// 
        /// </summary>
        /// <param name="userName"></param>
        /// <returns></returns>
        public override BootstrapUser RetrieveUserByUserName(string userName)
        {
            BootstrapUser user = null;
            var sql = "select UserName, DisplayName, case ifnull(d.Code, '') when '' then '~/images/uploader/' else d.Code end || ifnull(Icon, 'default.jpg') Icon, u.Css from Users u left join Dicts d on d.Define = '0' and d.Category = '头像地址' and Name = '头像路径' where ApprovedTime is not null and UserName = @UserName";
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
        }
    }
}

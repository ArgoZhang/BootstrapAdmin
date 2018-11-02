using Bootstrap.Security;
using Longbow.Data;
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
        /// 通过角色ID保存当前授权用户（插入）
        /// </summary>
        /// <param name="roleId">角色ID</param>
        /// <param name="userIds">用户ID数组</param>
        /// <returns></returns>
        public override bool SaveUsersByRoleId(string roleId, IEnumerable<string> userIds)
        {
            bool ret = false;
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
        public override bool SaveUsersByGroupId(string groupId, IEnumerable<string> userIds)
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

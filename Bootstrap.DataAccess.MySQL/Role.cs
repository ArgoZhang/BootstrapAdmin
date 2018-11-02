using Longbow.Data;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;

namespace Bootstrap.DataAccess.MySQL
{
    /// <summary>
    /// 
    /// </summary>
    public class Role : DataAccess.Role
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="userName"></param>
        /// <returns></returns>
        public override IEnumerable<string> RetrieveRolesByUserName(string userName)
        {
            var entities = new List<string>();
            using (DbCommand cmd = DbAccessManager.DBAccess.CreateCommand(CommandType.Text, "select r.RoleName from Roles r inner join UserRole ur on r.ID=ur.RoleID inner join Users u on ur.UserID = u.ID and u.UserName = @UserName union select r.RoleName from Roles r inner join RoleGroup rg on r.ID = rg.RoleID inner join `Groups` g on rg.GroupID = g.ID inner join UserGroup ug on ug.GroupID = g.ID inner join Users u on ug.UserID = u.ID and u.UserName=@UserName"))
            {
                cmd.Parameters.Add(DbAccessManager.DBAccess.CreateParameter("@UserName", userName));
                using (DbDataReader reader = DbAccessManager.DBAccess.ExecuteReader(cmd))
                {
                    while (reader.Read())
                    {
                        entities.Add((string)reader[0]);
                    }
                }
            }
            return entities;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public override IEnumerable<string> RetrieveRolesByUrl(string url)
        {
            string sql = "select distinct r.RoleName, r.Description from Roles r inner join NavigationRole nr on r.ID = nr.RoleID inner join Navigations n on nr.NavigationID = n.ID and n.Application = '0' and n.Url like @Url";
            var ret = new List<string>();
            var cmd = DbAccessManager.DBAccess.CreateCommand(CommandType.Text, sql);
            cmd.Parameters.Add(DbAccessManager.DBAccess.CreateParameter("@Url", $"{url}%"));
            using (DbDataReader reader = DbAccessManager.DBAccess.ExecuteReader(cmd))
            {
                while (reader.Read())
                {
                    ret.Add((string)reader[0]);
                }
            }
            if (ret.Count == 0) ret.Add("Administrators");
            return ret;
        }

        /// <summary>
        /// <summary>
        /// 保存用户角色关系
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="roleIds"></param>
        /// <returns></returns>
        public override bool SaveRolesByUserId(string userId, IEnumerable<string> roleIds)
        {
            var ret = false;
            //判断用户是否选定角色
            using (TransactionPackage transaction = DbAccessManager.DBAccess.BeginTransaction())
            {
                try
                {
                    // delete user from config table
                    string sql = $"delete from UserRole where UserID = {userId}";
                    using (DbCommand cmd = DbAccessManager.DBAccess.CreateCommand(CommandType.Text, sql))
                    {
                        DbAccessManager.DBAccess.ExecuteNonQuery(cmd, transaction);
                        roleIds.ToList().ForEach(rId =>
                        {
                            cmd.CommandText = $"Insert Into UserRole (UserID, RoleID) Values ( {userId}, {rId})";
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
        /// 删除角色表
        /// </summary>
        /// <param name="value"></param>
        public override bool DeleteRole(IEnumerable<string> value)
        {
            bool ret = false;
            var ids = string.Join(",", value);
            using (TransactionPackage transaction = DbAccessManager.DBAccess.BeginTransaction())
            {
                using (DbCommand cmd = DbAccessManager.DBAccess.CreateCommand(CommandType.Text, $"delete from UserRole where RoleID in ({ids})"))
                {
                    try
                    {
                        DbAccessManager.DBAccess.ExecuteNonQuery(cmd, transaction);

                        cmd.CommandText = $"delete from RoleGroup where RoleID in ({ids})";
                        DbAccessManager.DBAccess.ExecuteNonQuery(cmd, transaction);

                        cmd.CommandText = $"delete from NavigationRole where RoleID in ({ids})";
                        DbAccessManager.DBAccess.ExecuteNonQuery(cmd, transaction);

                        cmd.CommandText = $"delete from Roles where ID in ({ids})";
                        DbAccessManager.DBAccess.ExecuteNonQuery(cmd, transaction);

                        transaction.CommitTransaction();
                        ret = true;
                    }
                    catch (Exception ex)
                    {
                        transaction.RollbackTransaction();
                        throw ex;
                    }
                }
            }
            return ret;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="menuId"></param>
        /// <param name="roleIds"></param>
        /// <returns></returns>
        public override bool SavaRolesByMenuId(string menuId, IEnumerable<string> roleIds)
        {
            var ret = false;
            //判断用户是否选定角色
            using (TransactionPackage transaction = DbAccessManager.DBAccess.BeginTransaction())
            {
                try
                {
                    // delete role from config table
                    string sql = $"delete from NavigationRole where NavigationID = {menuId}";
                    using (DbCommand cmd = DbAccessManager.DBAccess.CreateCommand(CommandType.Text, sql))
                    {
                        DbAccessManager.DBAccess.ExecuteNonQuery(cmd, transaction);
                        // insert batch data into config table
                        roleIds.ToList().ForEach(rId =>
                        {
                            cmd.CommandText = $"Insert Into NavigationRole (NavigationID, RoleID) Values ( {menuId}, {rId})";
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
        /// 根据GroupId更新Roles信息，删除旧的Roles信息，插入新的Roles信息
        /// </summary>
        /// <param name="groupId"></param>
        /// <param name="roleIds"></param>
        /// <returns></returns>
        public override bool SaveRolesByGroupId(string groupId, IEnumerable<string> roleIds)
        {
            var ret = false;
            //构造表格
            using (TransactionPackage transaction = DbAccessManager.DBAccess.BeginTransaction())
            {
                try
                {
                    // delete user from config table
                    string sql = $"delete from RoleGroup where GroupID = {groupId}";
                    using (DbCommand cmd = DbAccessManager.DBAccess.CreateCommand(CommandType.Text, sql))
                    {
                        DbAccessManager.DBAccess.ExecuteNonQuery(cmd, transaction);

                        // insert batch data into config table
                        roleIds.ToList().ForEach(rId =>
                        {
                            cmd.CommandText = $"Insert Into RoleGroup (GroupID, RoleID) Values ( {groupId}, {rId})";
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
    }
}
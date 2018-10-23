using Longbow.Data;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;

namespace Bootstrap.DataAccess.SQLite
{
    /// <summary>
    /// 
    /// </summary>
    public class Role : DataAccess.Role
    {
        /// <summary>
        /// <summary>
        /// 保存用户角色关系
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="roleIds"></param>
        /// <returns></returns>
        public override bool SaveRolesByUserId(int userId, IEnumerable<int> roleIds)
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
                    CacheCleanUtility.ClearCache(userIds: new List<int>() { userId }, roleIds: roleIds);
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
        public override bool DeleteRole(IEnumerable<int> value)
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
                        CacheCleanUtility.ClearCache(roleIds: value);
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
        public override bool SavaRolesByMenuId(int menuId, IEnumerable<int> roleIds)
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
                    CacheCleanUtility.ClearCache(roleIds: roleIds, menuIds: new List<int>() { menuId });
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
        public override bool SaveRolesByGroupId(int groupId, IEnumerable<int> roleIds)
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
                    CacheCleanUtility.ClearCache(roleIds: roleIds, groupIds: new List<int>() { groupId });
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
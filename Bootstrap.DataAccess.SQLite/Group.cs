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
    public class Group : DataAccess.Group
    {
        /// <summary>
        /// <summary>
        /// 删除群组信息
        /// </summary>
        /// <param name="ids"></param>
        public override bool DeleteGroup(IEnumerable<string> value)
        {
            bool ret = false;
            var ids = string.Join(",", value);
            using (TransactionPackage transaction = DbAccessManager.DBAccess.BeginTransaction())
            {
                using (DbCommand cmd = DbAccessManager.DBAccess.CreateCommand(CommandType.Text, $"delete from UserGroup where GroupID in ({ids})"))
                {
                    try
                    {
                        DbAccessManager.DBAccess.ExecuteNonQuery(cmd, transaction);

                        cmd.CommandText = $"delete from RoleGroup where GroupID in ({ids})";
                        DbAccessManager.DBAccess.ExecuteNonQuery(cmd, transaction);

                        cmd.CommandText = $"delete from Groups where ID in ({ids})";
                        DbAccessManager.DBAccess.ExecuteNonQuery(cmd, transaction);

                        transaction.CommitTransaction();
                        CacheCleanUtility.ClearCache(groupIds: value);
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
        /// 保存用户部门关系
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="groupIds"></param>
        /// <returns></returns>
        public override bool SaveGroupsByUserId(string userId, IEnumerable<string> groupIds)
        {
            var ret = false;

            //判断用户是否选定角色
            using (TransactionPackage transaction = DbAccessManager.DBAccess.BeginTransaction())
            {
                try
                {
                    //删除用户部门表中该用户所有的部门关系
                    string sql = $"delete from UserGroup where UserID = {userId}";
                    using (DbCommand cmd = DbAccessManager.DBAccess.CreateCommand(CommandType.Text, sql))
                    {
                        DbAccessManager.DBAccess.ExecuteNonQuery(cmd, transaction);

                        groupIds.ToList().ForEach(gId =>
                        {
                            cmd.CommandText = $"Insert Into UserGroup (UserID, GroupID) Values ({userId}, {gId})";
                            DbAccessManager.DBAccess.ExecuteNonQuery(cmd, transaction);
                        });
                        transaction.CommitTransaction();
                    }
                    CacheCleanUtility.ClearCache(groupIds: groupIds, userIds: new List<string>() { userId });
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
        /// 根据角色ID以及选定的部门ID，保到角色部门表
        /// </summary>
        /// <param name="roleId"></param>
        /// <param name="groupIds"></param>
        /// <returns></returns>
        public override bool SaveGroupsByRoleId(string roleId, IEnumerable<string> groupIds)
        {
            bool ret = false;
            using (TransactionPackage transaction = DbAccessManager.DBAccess.BeginTransaction())
            {
                try
                {
                    //删除角色部门表该角色所有的部门
                    string sql = $"delete from RoleGroup where RoleID = {roleId}";
                    using (DbCommand cmd = DbAccessManager.DBAccess.CreateCommand(CommandType.Text, sql))
                    {
                        DbAccessManager.DBAccess.ExecuteNonQuery(cmd, transaction);
                        //批插入角色部门表
                        groupIds.ToList().ForEach(gId =>
                        {
                            cmd.CommandText = $"Insert Into RoleGroup (RoleID, GroupID) Values ({roleId}, {gId})";
                            DbAccessManager.DBAccess.ExecuteNonQuery(cmd, transaction);
                        });
                        transaction.CommitTransaction();
                    }
                    CacheCleanUtility.ClearCache(groupIds: groupIds, roleIds: new List<string>() { roleId });
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

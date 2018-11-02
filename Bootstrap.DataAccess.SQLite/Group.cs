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

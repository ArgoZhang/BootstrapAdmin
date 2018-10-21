using Longbow.Data;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
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
        public override bool DeleteGroup(IEnumerable<int> value)
        {
            bool ret = false;
            var ids = string.Join(",", value);
            using (DbCommand cmd = DbAccessManager.DBAccess.CreateCommand(CommandType.StoredProcedure, "Proc_DeleteGroups"))
            {
                cmd.Parameters.Add(DbAccessManager.DBAccess.CreateParameter("@ids", ids));
                ret = DbAccessManager.DBAccess.ExecuteNonQuery(cmd) == -1;
            }
            CacheCleanUtility.ClearCache(groupIds: value);
            return ret;
        }
        /// <summary>
        /// 保存用户部门关系
        /// </summary>
        /// <param name="id"></param>
        /// <param name="groupIds"></param>
        /// <returns></returns>
        public override bool SaveGroupsByUserId(int id, IEnumerable<int> groupIds)
        {
            var ret = false;
            DataTable dt = new DataTable();
            dt.Columns.Add("UserID", typeof(int));
            dt.Columns.Add("GroupID", typeof(int));
            //判断用户是否选定角色
            groupIds.ToList().ForEach(groupId => dt.Rows.Add(id, groupId));
            using (TransactionPackage transaction = DbAccessManager.DBAccess.BeginTransaction())
            {
                try
                {
                    //删除用户部门表中该用户所有的部门关系
                    string sql = "delete from UserGroup where UserID=@UserID;";
                    using (DbCommand cmd = DbAccessManager.DBAccess.CreateCommand(CommandType.Text, sql))
                    {
                        cmd.Parameters.Add(DbAccessManager.DBAccess.CreateParameter("@UserID", id));
                        DbAccessManager.DBAccess.ExecuteNonQuery(cmd, transaction);

                        // insert batch data into config table
                        using (SqlBulkCopy bulk = new SqlBulkCopy((SqlConnection)transaction.Transaction.Connection, SqlBulkCopyOptions.Default, (SqlTransaction)transaction.Transaction))
                        {
                            bulk.BatchSize = 1000;
                            bulk.DestinationTableName = "UserGroup";
                            bulk.ColumnMappings.Add("UserID", "UserID");
                            bulk.ColumnMappings.Add("GroupID", "GroupID");
                            bulk.WriteToServer(dt);
                            transaction.CommitTransaction();
                        }
                    }
                    CacheCleanUtility.ClearCache(groupIds: groupIds, userIds: new List<int>() { id });
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
        /// <param name="id"></param>
        /// <param name="groupIds"></param>
        /// <returns></returns>
        public override bool SaveGroupsByRoleId(int id, IEnumerable<int> groupIds)
        {
            bool ret = false;
            DataTable dt = new DataTable();
            dt.Columns.Add("GroupID", typeof(int));
            dt.Columns.Add("RoleID", typeof(int));
            groupIds.ToList().ForEach(groupId => dt.Rows.Add(groupId, id));
            using (TransactionPackage transaction = DbAccessManager.DBAccess.BeginTransaction())
            {
                try
                {
                    //删除角色部门表该角色所有的部门
                    string sql = "delete from RoleGroup where RoleID=@RoleID";
                    using (DbCommand cmd = DbAccessManager.DBAccess.CreateCommand(CommandType.Text, sql))
                    {
                        cmd.Parameters.Add(DbAccessManager.DBAccess.CreateParameter("@RoleID", id));
                        DbAccessManager.DBAccess.ExecuteNonQuery(cmd, transaction);
                        //批插入角色部门表
                        using (SqlBulkCopy bulk = new SqlBulkCopy((SqlConnection)transaction.Transaction.Connection, SqlBulkCopyOptions.Default, (SqlTransaction)transaction.Transaction))
                        {
                            bulk.BatchSize = 1000;
                            bulk.ColumnMappings.Add("GroupID", "GroupID");
                            bulk.ColumnMappings.Add("RoleID", "RoleID");
                            bulk.DestinationTableName = "RoleGroup";
                            bulk.WriteToServer(dt);
                            transaction.CommitTransaction();
                        }
                    }
                    CacheCleanUtility.ClearCache(groupIds: groupIds, roleIds: new List<int>() { id });
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

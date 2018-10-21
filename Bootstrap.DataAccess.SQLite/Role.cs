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
    public class Role : DataAccess.Role
    {
        /// <summary>
        /// <summary>
        /// 保存用户角色关系
        /// </summary>
        /// <param name="id"></param>
        /// <param name="roleIds"></param>
        /// <returns></returns>
        public override bool SaveRolesByUserId(int id, IEnumerable<int> roleIds)
        {
            var ret = false;
            DataTable dt = new DataTable();
            dt.Columns.Add("UserID", typeof(int));
            dt.Columns.Add("RoleID", typeof(int));
            //判断用户是否选定角色
            roleIds.ToList().ForEach(roleId => dt.Rows.Add(id, roleId));
            using (TransactionPackage transaction = DbAccessManager.DBAccess.BeginTransaction())
            {
                try
                {
                    // delete user from config table
                    string sql = "delete from UserRole where UserID = @UserID;";
                    using (DbCommand cmd = DbAccessManager.DBAccess.CreateCommand(CommandType.Text, sql))
                    {
                        cmd.Parameters.Add(DbAccessManager.DBAccess.CreateParameter("@UserID", id));
                        DbAccessManager.DBAccess.ExecuteNonQuery(cmd, transaction);
                        if (dt.Rows.Count > 0)
                        {
                            // insert batch data into config table
                            using (SqlBulkCopy bulk = new SqlBulkCopy((SqlConnection)transaction.Transaction.Connection, SqlBulkCopyOptions.Default, (SqlTransaction)transaction.Transaction))
                            {
                                bulk.DestinationTableName = "UserRole";
                                bulk.ColumnMappings.Add("UserID", "UserID");
                                bulk.ColumnMappings.Add("RoleID", "RoleID");
                                bulk.WriteToServer(dt);
                            }
                        }
                        transaction.CommitTransaction();
                    }
                    CacheCleanUtility.ClearCache(userIds: new List<int>() { id }, roleIds: roleIds);
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
            using (DbCommand cmd = DbAccessManager.DBAccess.CreateCommand(CommandType.StoredProcedure, "Proc_DeleteRoles"))
            {
                cmd.Parameters.Add(DbAccessManager.DBAccess.CreateParameter("@ids", ids));
                ret = DbAccessManager.DBAccess.ExecuteNonQuery(cmd) == -1;
            }
            CacheCleanUtility.ClearCache(roleIds: value);
            return ret;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <param name="roleIds"></param>
        /// <returns></returns>
        public override bool SavaRolesByMenuId(int id, IEnumerable<int> roleIds)
        {
            var ret = false;
            DataTable dt = new DataTable();
            dt.Columns.Add("NavigationID", typeof(int));
            dt.Columns.Add("RoleID", typeof(int));
            //判断用户是否选定角色
            roleIds.ToList().ForEach(roleId => dt.Rows.Add(id, roleId));
            using (TransactionPackage transaction = DbAccessManager.DBAccess.BeginTransaction())
            {
                try
                {
                    // delete role from config table
                    string sql = "delete from NavigationRole where NavigationID=@NavigationID;";
                    using (DbCommand cmd = DbAccessManager.DBAccess.CreateCommand(CommandType.Text, sql))
                    {
                        cmd.Parameters.Add(DbAccessManager.DBAccess.CreateParameter("@NavigationID", id));
                        DbAccessManager.DBAccess.ExecuteNonQuery(cmd, transaction);

                        // insert batch data into config table
                        using (SqlBulkCopy bulk = new SqlBulkCopy((SqlConnection)transaction.Transaction.Connection, SqlBulkCopyOptions.Default, (SqlTransaction)transaction.Transaction))
                        {
                            bulk.BatchSize = 1000;
                            bulk.DestinationTableName = "NavigationRole";
                            bulk.ColumnMappings.Add("NavigationID", "NavigationID");
                            bulk.ColumnMappings.Add("RoleID", "RoleID");
                            bulk.WriteToServer(dt);
                            transaction.CommitTransaction();
                        }
                    }
                    CacheCleanUtility.ClearCache(roleIds: roleIds, menuIds: new List<int>() { id });
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
        /// <param name="id"></param>
        /// <param name="roleIds"></param>
        /// <returns></returns>
        public override bool SaveRolesByGroupId(int id, IEnumerable<int> roleIds)
        {
            var ret = false;
            //构造表格
            DataTable dt = new DataTable();
            dt.Columns.Add("RoleID", typeof(int));
            dt.Columns.Add("GroupID", typeof(int));
            roleIds.ToList().ForEach(roleId => dt.Rows.Add(roleId, id));
            using (TransactionPackage transaction = DbAccessManager.DBAccess.BeginTransaction())
            {
                try
                {
                    // delete user from config table
                    string sql = "delete from RoleGroup where GroupID=@GroupID";
                    using (DbCommand cmd = DbAccessManager.DBAccess.CreateCommand(CommandType.Text, sql))
                    {
                        cmd.Parameters.Add(DbAccessManager.DBAccess.CreateParameter("@GroupID", id));
                        DbAccessManager.DBAccess.ExecuteNonQuery(cmd, transaction);

                        // insert batch data into config table
                        using (SqlBulkCopy bulk = new SqlBulkCopy((SqlConnection)transaction.Transaction.Connection, SqlBulkCopyOptions.Default, (SqlTransaction)transaction.Transaction))
                        {
                            bulk.BatchSize = 1000;
                            bulk.DestinationTableName = "RoleGroup";
                            bulk.ColumnMappings.Add("RoleID", "RoleID");
                            bulk.ColumnMappings.Add("GroupID", "GroupID");
                            bulk.WriteToServer(dt);
                            transaction.CommitTransaction();
                        }
                    }
                    CacheCleanUtility.ClearCache(roleIds: roleIds, groupIds: new List<int>() { id });
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
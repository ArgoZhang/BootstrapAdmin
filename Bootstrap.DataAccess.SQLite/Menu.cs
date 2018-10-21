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
    public class Menu : DataAccess.Menu
    {
        /// <summary>
        /// 删除菜单信息
        /// </summary>
        /// <param name="value"></param>
        public override bool DeleteMenu(IEnumerable<int> value)
        {
            bool ret = false;
            var ids = string.Join(",", value);
            using (DbCommand cmd = DbAccessManager.DBAccess.CreateCommand(CommandType.StoredProcedure, "Proc_DeleteMenus"))
            {
                cmd.Parameters.Add(DbAccessManager.DBAccess.CreateParameter("@ids", ids));
                ret = DbAccessManager.DBAccess.ExecuteNonQuery(cmd) == -1;
            }
            CacheCleanUtility.ClearCache(menuIds: value);
            return ret;
        }
        /// <summary>
        /// <summary>
        /// 通过角色ID保存当前授权菜单
        /// </summary>
        /// <param name="id"></param>
        /// <param name="menuIds"></param>
        /// <returns></returns>
        public override bool SaveMenusByRoleId(int id, IEnumerable<int> menuIds)
        {
            bool ret = false;
            DataTable dt = new DataTable();
            dt.Columns.Add("RoleID", typeof(int));
            dt.Columns.Add("NavigationID", typeof(int));
            menuIds.ToList().ForEach(menuId => dt.Rows.Add(id, menuId));
            using (TransactionPackage transaction = DbAccessManager.DBAccess.BeginTransaction())
            {
                try
                {
                    //删除菜单角色表该角色所有的菜单
                    string sql = "delete from NavigationRole where RoleID=@RoleID";
                    using (DbCommand cmd = DbAccessManager.DBAccess.CreateCommand(CommandType.Text, sql))
                    {
                        cmd.Parameters.Add(DbAccessManager.DBAccess.CreateParameter("@RoleID", id));
                        DbAccessManager.DBAccess.ExecuteNonQuery(cmd, transaction);
                        //批插入菜单角色表
                        using (SqlBulkCopy bulk = new SqlBulkCopy((SqlConnection)transaction.Transaction.Connection, SqlBulkCopyOptions.Default, (SqlTransaction)transaction.Transaction))
                        {
                            bulk.DestinationTableName = "NavigationRole";
                            bulk.ColumnMappings.Add("RoleID", "RoleID");
                            bulk.ColumnMappings.Add("NavigationID", "NavigationID");
                            bulk.WriteToServer(dt);
                            transaction.CommitTransaction();
                        }
                    }
                    CacheCleanUtility.ClearCache(menuIds: menuIds, roleIds: new List<int>() { id });
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

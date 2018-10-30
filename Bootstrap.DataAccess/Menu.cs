using Bootstrap.Security;
using Bootstrap.Security.DataAccess;
using Longbow.Data;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;

namespace Bootstrap.DataAccess
{
    /// <summary>
    /// 
    /// </summary>
    public class Menu : BootstrapMenu
    {
        /// <summary>
        /// 删除菜单信息
        /// </summary>
        /// <param name="value"></param>
        public virtual bool DeleteMenu(IEnumerable<string> value)
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
        /// 保存新建/更新的菜单信息
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        public virtual bool SaveMenu(BootstrapMenu p)
        {
            if (string.IsNullOrEmpty(p.Name)) return false;
            bool ret = false;
            if (p.Name.Length > 50) p.Name = p.Name.Substring(0, 50);
            if (p.Icon != null && p.Icon.Length > 50) p.Icon = p.Icon.Substring(0, 50);
            if (p.Url != null && p.Url.Length > 4000) p.Url = p.Url.Substring(0, 4000);
            string sql = string.IsNullOrEmpty(p.Id) ?
                "Insert Into Navigations (ParentId, Name, [Order], Icon, Url, Category, Target, IsResource, [Application]) Values (@ParentId, @Name, @Order, @Icon, @Url, @Category, @Target, @IsResource, @ApplicationCode)" :
                "Update Navigations set ParentId = @ParentId, Name = @Name, [Order] = @Order, Icon = @Icon, Url = @Url, Category = @Category, Target = @Target, IsResource = @IsResource, Application = @ApplicationCode where ID = @ID";
            using (DbCommand cmd = DbAccessManager.DBAccess.CreateCommand(CommandType.Text, sql))
            {
                cmd.Parameters.Add(DbAccessManager.DBAccess.CreateParameter("@ID", p.Id));
                cmd.Parameters.Add(DbAccessManager.DBAccess.CreateParameter("@ParentId", p.ParentId));
                cmd.Parameters.Add(DbAccessManager.DBAccess.CreateParameter("@Name", p.Name));
                cmd.Parameters.Add(DbAccessManager.DBAccess.CreateParameter("@Order", p.Order));
                cmd.Parameters.Add(DbAccessManager.DBAccess.CreateParameter("@Icon", DbAdapterManager.ToDBValue(p.Icon)));
                cmd.Parameters.Add(DbAccessManager.DBAccess.CreateParameter("@Url", DbAdapterManager.ToDBValue(p.Url)));
                cmd.Parameters.Add(DbAccessManager.DBAccess.CreateParameter("@Category", p.Category));
                cmd.Parameters.Add(DbAccessManager.DBAccess.CreateParameter("@Target", p.Target));
                cmd.Parameters.Add(DbAccessManager.DBAccess.CreateParameter("@IsResource", p.IsResource));
                cmd.Parameters.Add(DbAccessManager.DBAccess.CreateParameter("@ApplicationCode", p.ApplicationCode));
                ret = DbAccessManager.DBAccess.ExecuteNonQuery(cmd) == 1;
            }
            CacheCleanUtility.ClearCache(menuIds: string.IsNullOrEmpty(p.Id) ? new List<string>() : new List<string>() { p.Id });
            return ret;
        }
        /// <summary>
        /// 查询某个角色所配置的菜单
        /// </summary>
        /// <param name="roleId"></param>
        /// <returns></returns>
        public virtual IEnumerable<BootstrapMenu> RetrieveMenusByRoleId(string roleId)
        {
            var menus = new List<BootstrapMenu>();
            string sql = "select NavigationID from NavigationRole where RoleID = @RoleID";
            using (DbCommand cmd = DbAccessManager.DBAccess.CreateCommand(CommandType.Text, sql))
            {
                cmd.Parameters.Add(DbAccessManager.DBAccess.CreateParameter("@RoleID", roleId));
                using (DbDataReader reader = DbAccessManager.DBAccess.ExecuteReader(cmd))
                {
                    while (reader.Read())
                    {
                        menus.Add(new BootstrapMenu()
                        {
                            Id = reader[0].ToString()
                        });
                    }
                }
            }
            return menus;
        }
        /// <summary>
        /// 通过角色ID保存当前授权菜单
        /// </summary>
        /// <param name="roleId"></param>
        /// <param name="menuIds"></param>
        /// <returns></returns>
        public virtual bool SaveMenusByRoleId(string roleId, IEnumerable<string> menuIds)
        {
            bool ret = false;
            DataTable dt = new DataTable();
            dt.Columns.Add("RoleID", typeof(int));
            dt.Columns.Add("NavigationID", typeof(int));
            menuIds.ToList().ForEach(menuId => dt.Rows.Add(roleId, menuId));
            using (TransactionPackage transaction = DbAccessManager.DBAccess.BeginTransaction())
            {
                try
                {
                    //删除菜单角色表该角色所有的菜单
                    string sql = $"delete from NavigationRole where RoleID = {roleId}";
                    using (DbCommand cmd = DbAccessManager.DBAccess.CreateCommand(CommandType.Text, sql))
                    {
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
                    CacheCleanUtility.ClearCache(menuIds: menuIds, roleIds: new List<string>() { roleId });
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
        /// 通过当前用户名获得所有菜单
        /// </summary>
        /// <param name="userName">当前登陆的用户名</param>
        /// <returns></returns>
        public virtual IEnumerable<BootstrapMenu> RetrieveAllMenus(string userName) => DbHelper.RetrieveAllMenus(userName);
    }
}

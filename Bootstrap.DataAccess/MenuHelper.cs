using Bootstrap.Security;
using Longbow.Cache;
using Longbow.Data;
using Longbow.Logging;
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
    public static class MenuHelper
    {
        /// <summary>
        /// 
        /// </summary>
        internal const string RetrieveMenusByRoleIdDataKey = "MenuHelper-RetrieveMenusByRoleId";
        /// <summary>
        /// 删除菜单信息
        /// </summary>
        /// <param name="ids"></param>
        public static bool DeleteMenu(string ids)
        {
            if (string.IsNullOrEmpty(ids) || ids.Contains("'")) return false;
            bool ret = false;
            try
            {
                using (DbCommand cmd = DBAccessManager.SqlDBAccess.CreateCommand(CommandType.StoredProcedure, "Proc_DeleteMenus"))
                {
                    cmd.Parameters.Add(DBAccessManager.SqlDBAccess.CreateParameter("@ids", ids));
                    DBAccessManager.SqlDBAccess.ExecuteNonQuery(cmd);
                }
                CacheCleanUtility.ClearCache(menuIds: ids);
                ret = true;
            }
            catch (Exception ex)
            {
                ExceptionManager.Publish(ex);
            }
            return ret;
        }
        /// <summary>
        /// 保存新建/更新的菜单信息
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        public static bool SaveMenu(BootstrapMenu p)
        {
            if (string.IsNullOrEmpty(p.Name)) return false;
            bool ret = false;
            if (p.Name.Length > 50) p.Name = p.Name.Substring(0, 50);
            if (p.Icon != null && p.Icon.Length > 50) p.Icon = p.Icon.Substring(0, 50);
            if (p.Url != null && p.Url.Length > 4000) p.Url = p.Url.Substring(0, 4000);
            string sql = p.Id == 0 ?
                "Insert Into Navigations (ParentId, Name, [Order], Icon, Url, Category, Target, IsResource, [Application]) Values (@ParentId, @Name, @Order, @Icon, @Url, @Category, @Target, @IsResource, @ApplicationCode)" :
                "Update Navigations set ParentId = @ParentId, Name = @Name, [Order] = @Order, Icon = @Icon, Url = @Url, Category = @Category, Target = @Target, IsResource = @IsResource, Application = @ApplicationCode where ID = @ID";
            try
            {
                using (DbCommand cmd = DBAccessManager.SqlDBAccess.CreateCommand(CommandType.Text, sql))
                {
                    cmd.Parameters.Add(DBAccessManager.SqlDBAccess.CreateParameter("@ID", p.Id));
                    cmd.Parameters.Add(DBAccessManager.SqlDBAccess.CreateParameter("@ParentId", p.ParentId));
                    cmd.Parameters.Add(DBAccessManager.SqlDBAccess.CreateParameter("@Name", p.Name));
                    cmd.Parameters.Add(DBAccessManager.SqlDBAccess.CreateParameter("@Order", p.Order));
                    cmd.Parameters.Add(DBAccessManager.SqlDBAccess.CreateParameter("@Icon", DBAccessFactory.ToDBValue(p.Icon)));
                    cmd.Parameters.Add(DBAccessManager.SqlDBAccess.CreateParameter("@Url", DBAccessFactory.ToDBValue(p.Url)));
                    cmd.Parameters.Add(DBAccessManager.SqlDBAccess.CreateParameter("@Category", p.Category));
                    cmd.Parameters.Add(DBAccessManager.SqlDBAccess.CreateParameter("@Target", p.Target));
                    cmd.Parameters.Add(DBAccessManager.SqlDBAccess.CreateParameter("@IsResource", p.IsResource));
                    cmd.Parameters.Add(DBAccessManager.SqlDBAccess.CreateParameter("@ApplicationCode", p.ApplicationCode));
                    DBAccessManager.SqlDBAccess.ExecuteNonQuery(cmd);
                }
                CacheCleanUtility.ClearCache(menuIds: p.Id == 0 ? string.Empty : p.Id.ToString());
                ret = true;
            }
            catch (DbException ex)
            {
                ExceptionManager.Publish(ex);
            }
            return ret;
        }

        /// <summary>
        /// 查询某个角色所配置的菜单
        /// </summary>
        /// <param name="roleId"></param>
        /// <returns></returns>
        public static IEnumerable<BootstrapMenu> RetrieveMenusByRoleId(int roleId)
        {
            string key = string.Format("{0}-{1}", RetrieveMenusByRoleIdDataKey, roleId);
            return CacheManager.GetOrAdd(key, k =>
            {
                var menus = new List<BootstrapMenu>();
                try
                {
                    string sql = "select NavigationID from NavigationRole where RoleID = @RoleID";
                    using (DbCommand cmd = DBAccessManager.SqlDBAccess.CreateCommand(CommandType.Text, sql))
                    {
                        cmd.Parameters.Add(DBAccessManager.SqlDBAccess.CreateParameter("@RoleID", roleId));
                        using (DbDataReader reader = DBAccessManager.SqlDBAccess.ExecuteReader(cmd))
                        {
                            while (reader.Read())
                            {
                                menus.Add(new BootstrapMenu()
                                {
                                    Id = (int)reader[0]
                                });
                            }
                        }
                    }
                }
                catch (Exception ex) { ExceptionManager.Publish(ex); }
                return menus;
            }, RetrieveMenusByRoleIdDataKey);
        }
        /// <summary>
        /// 通过角色ID保存当前授权菜单
        /// </summary>
        /// <param name="id"></param>
        /// <param name="menuIds"></param>
        /// <returns></returns>
        public static bool SaveMenusByRoleId(int id, string menuIds)
        {
            bool ret = false;
            DataTable dt = new DataTable();
            dt.Columns.Add("RoleID", typeof(int));
            dt.Columns.Add("NavigationID", typeof(int));
            if (!string.IsNullOrEmpty(menuIds)) menuIds.Split(',').ToList().ForEach(menuId => dt.Rows.Add(id, Convert.ToInt32(menuId)));
            using (TransactionPackage transaction = DBAccessManager.SqlDBAccess.BeginTransaction())
            {
                try
                {
                    //删除菜单角色表该角色所有的菜单
                    string sql = "delete from NavigationRole where RoleID=@RoleID";
                    using (DbCommand cmd = DBAccessManager.SqlDBAccess.CreateCommand(CommandType.Text, sql))
                    {
                        cmd.Parameters.Add(DBAccessManager.SqlDBAccess.CreateParameter("@RoleID", id));
                        DBAccessManager.SqlDBAccess.ExecuteNonQuery(cmd, transaction);
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
                    CacheCleanUtility.ClearCache(menuIds: menuIds, roleIds: id.ToString());
                    ret = true;
                }
                catch (Exception ex)
                {
                    ExceptionManager.Publish(ex);
                    transaction.RollbackTransaction();
                }
            }
            return ret;
        }
    }
}

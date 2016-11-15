using Longbow;
using Longbow.Caching;
using Longbow.Caching.Configuration;
using Longbow.Data;
using Longbow.ExceptionManagement;
using Longbow.Security.Principal;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;

namespace Bootstrap.DataAccess
{
    public static class MenuHelper
    {
        internal const string RetrieveMenusDataKey = "MenuHelper-RetrieveMenus";
        internal const string RetrieveMenusByRoleIDDataKey = "MenuHelper-RetrieveMenusByRoleId";
        /// <summary>
        /// 查询所有菜单信息
        /// </summary>
        /// <param name="userName"></param>
        /// <returns></returns>
        public static IEnumerable<Menu> RetrieveMenus(string userName = null)
        {
            userName = LgbPrincipal.IsAdmin(userName) ? string.Empty : userName;
            string key = string.Format("{0}-{1}", RetrieveMenusDataKey, userName);
            return CacheManager.GetOrAdd(key, CacheSection.RetrieveIntervalByKey(RetrieveMenusDataKey), k =>
            {
                List<Menu> Menus = new List<Menu>();
                try
                {
                    using (DbCommand cmd = DBAccessManager.SqlDBAccess.CreateCommand(CommandType.StoredProcedure, "Proc_RetrieveMenus"))
                    {
                        cmd.Parameters.Add(DBAccessManager.SqlDBAccess.CreateParameter("@userName", DBAccess.ToDBValue(userName), ParameterDirection.Input));
                        using (DbDataReader reader = DBAccessManager.SqlDBAccess.ExecuteReader(cmd))
                        {
                            while (reader.Read())
                            {
                                Menus.Add(new Menu()
                                {
                                    ID = (int)reader[0],
                                    ParentId = (int)reader[1],
                                    Name = (string)reader[2],
                                    Order = (int)reader[3],
                                    Icon = LgbConvert.ReadValue(reader[4], string.Empty),
                                    Url = LgbConvert.ReadValue(reader[5], string.Empty),
                                    Category = (string)reader[6],
                                    CategoryName = (string)reader[7],
                                    ParentName = LgbConvert.ReadValue(reader[8], string.Empty)
                                });
                            }
                        }
                    }
                }
                catch (Exception ex) { ExceptionManager.Publish(ex); }
                return Menus;
            }, CacheSection.RetrieveDescByKey(RetrieveMenusDataKey));
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static IEnumerable<Menu> RetrieveAllMenusByUserName(string userName)
        {
            var navs = RetrieveMenus(userName);
            var root = navs.Where(m => m.ParentId == 0).OrderBy(m => m.Order);
            CascadeMenu(navs, root);
            return root;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public static IEnumerable<Menu> RetrieveNavigationsByUserName(string userName)
        {
            var navs = RetrieveMenus(userName).Where(m => m.Category == "0");
            var root = navs.Where(m => m.ParentId == 0).OrderBy(m => m.Order);
            CascadeMenu(navs, root);
            return root;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public static IEnumerable<Menu> RetrieveLinksByUserName(string userName)
        {
            var navs = RetrieveMenus(userName).Where(m => m.Category == "1");
            var root = navs.Where(m => m.ParentId == 0).OrderBy(m => m.Order);
            CascadeMenu(navs, root);
            return root;
        }
        private static void CascadeMenu(IEnumerable<Menu> navs, IEnumerable<Menu> level)
        {
            level.ToList().ForEach(m =>
            {
                m.Menus = navs.Where(sub => sub.ParentId == m.ID).OrderBy(sub => sub.Order);
                CascadeMenu(navs, m.Menus);
            });
        }
        /// <summary>
        /// 删除菜单信息
        /// </summary>
        /// <param name="ids"></param>
        public static bool DeleteMenu(string ids)
        {
            bool ret = false;
            if (string.IsNullOrEmpty(ids) || ids.Contains("'")) return ret;
            try
            {
                using (DbCommand cmd = DBAccessManager.SqlDBAccess.CreateCommand(CommandType.StoredProcedure, "Proc_DeleteMenus"))
                {
                    cmd.Parameters.Add(DBAccessManager.SqlDBAccess.CreateParameter("@ids", ids, ParameterDirection.Input));
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
        public static bool SaveMenu(Menu p)
        {
            if (p == null) throw new ArgumentNullException("p");
            bool ret = false;
            if (string.IsNullOrEmpty(p.Name)) return ret;
            if (p.Name.Length > 50) p.Name.Substring(0, 50);
            if (p.Icon != null && p.Icon.Length > 50) p.Icon.Substring(0, 50);
            if (p.Url != null && p.Url.Length > 50) p.Url.Substring(0, 50);
            string sql = p.ID == 0 ?
                "Insert Into Navigations (ParentId, Name, [Order], Icon, Url, Category) Values (@ParentId, @Name, @Order, @Icon, @Url, @Category)" :
                "Update Navigations set ParentId = @ParentId, Name = @Name, [Order] = @Order, Icon = @Icon, Url = @Url, Category = @Category where ID = @ID";
            try
            {
                using (DbCommand cmd = DBAccessManager.SqlDBAccess.CreateCommand(CommandType.Text, sql))
                {
                    cmd.Parameters.Add(DBAccessManager.SqlDBAccess.CreateParameter("@ID", p.ID, ParameterDirection.Input));
                    cmd.Parameters.Add(DBAccessManager.SqlDBAccess.CreateParameter("@ParentId", p.ParentId, ParameterDirection.Input));
                    cmd.Parameters.Add(DBAccessManager.SqlDBAccess.CreateParameter("@Name", p.Name, ParameterDirection.Input));
                    cmd.Parameters.Add(DBAccessManager.SqlDBAccess.CreateParameter("@Order", p.Order, ParameterDirection.Input));
                    cmd.Parameters.Add(DBAccessManager.SqlDBAccess.CreateParameter("@Icon", DBAccess.ToDBValue(p.Icon), ParameterDirection.Input));
                    cmd.Parameters.Add(DBAccessManager.SqlDBAccess.CreateParameter("@Url", DBAccess.ToDBValue(p.Url), ParameterDirection.Input));
                    cmd.Parameters.Add(DBAccessManager.SqlDBAccess.CreateParameter("@Category", p.Category, ParameterDirection.Input));
                    DBAccessManager.SqlDBAccess.ExecuteNonQuery(cmd);
                }
                CacheCleanUtility.ClearCache(menuIds: p.ID == 0 ? string.Empty : p.ID.ToString());
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
        public static IEnumerable<Menu> RetrieveMenusByRoleId(int roleId)
        {
            string key = string.Format("{0}-{1}", RetrieveMenusByRoleIDDataKey, roleId);
            return CacheManager.GetOrAdd(key, CacheSection.RetrieveIntervalByKey(RetrieveMenusByRoleIDDataKey), k =>
            {
                List<Menu> Menus = new List<Menu>();
                try
                {
                    string sql = "select NavigationID from NavigationRole where RoleID = @RoleID";
                    using (DbCommand cmd = DBAccessManager.SqlDBAccess.CreateCommand(CommandType.Text, sql))
                    {
                        cmd.Parameters.Add(DBAccessManager.SqlDBAccess.CreateParameter("@RoleID", roleId, ParameterDirection.Input));
                        using (DbDataReader reader = DBAccessManager.SqlDBAccess.ExecuteReader(cmd))
                        {
                            while (reader.Read())
                            {
                                Menus.Add(new Menu()
                                {
                                    ID = (int)reader[0]
                                });
                            }
                        }
                    }
                }
                catch (Exception ex) { ExceptionManager.Publish(ex); }
                return Menus;
            }, CacheSection.RetrieveDescByKey(RetrieveMenusByRoleIDDataKey));
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
                        cmd.Parameters.Add(DBAccessManager.SqlDBAccess.CreateParameter("@RoleID", id, ParameterDirection.Input));
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

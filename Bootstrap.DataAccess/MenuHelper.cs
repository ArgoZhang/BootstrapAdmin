using Longbow;
using Longbow.Caching;
using Longbow.Caching.Configuration;
using Longbow.Data;
using Longbow.ExceptionManagement;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;

namespace Bootstrap.DataAccess
{
    public static class MenuHelper
    {
        internal const string RetrieveMenusDataKey = "MenuHelper-RetrieveMenus";
        internal const string RetrieveMenusByUserIDDataKey = "MenuHelper-RetrieveMenusByUserId";
        internal const string RetrieveMenusByRoleIDDataKey = "MenuHelper-RetrieveMenusByRoleId";
        /// <summary>
        /// 查询所有菜单信息
        /// </summary>
        /// <param name="tId"></param>
        /// <returns></returns>
        public static IEnumerable<Menu> RetrieveMenus()
        {
            return CacheManager.GetOrAdd(RetrieveMenusDataKey, CacheSection.RetrieveIntervalByKey(RetrieveMenusDataKey), key =>
            {
                string sql = "select n.*, d.Name as CategoryName, ln.Name as ParentName from Navigations n inner join Dicts d on n.Category = d.Code and d.Category = N'菜单' and d.Define = 0 left join Navigations ln on n.ParentId = ln.ID";
                List<Menu> Menus = new List<Menu>();
                DbCommand cmd = DBAccessManager.SqlDBAccess.CreateCommand(CommandType.Text, sql);
                try
                {
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
                catch (Exception ex) { ExceptionManager.Publish(ex); }
                return Menus;
            }, CacheSection.RetrieveDescByKey(RetrieveMenusDataKey));
        }
        /// <summary>
        /// 查询某个用户所配置的菜单
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public static IEnumerable<Menu> RetrieveMenusByUserId(int userId)
        {
            string key = string.Format("{0}-{1}", RetrieveMenusByUserIDDataKey, userId);
            return CacheManager.GetOrAdd(key, CacheSection.RetrieveIntervalByKey(RetrieveMenusByUserIDDataKey), k =>
            {
                string sql = "select n.* from Navigations n inner join NavigationRole nr on n.ID = nr.NavigationID inner join UserRole ur on nr.RoleID = ur.RoleID inner join Users u on ur.UserID = u.ID where u.ID = @UserID union select n.* from Navigations n inner join NavigationRole nr on n.ID = nr.NavigationID inner join RoleGroup rg on nr.RoleID = rg.RoleID inner join UserGroup ur on rg.GroupID = ur.GroupID inner join Users u on ur.UserID = u.ID where u.ID = @UserID";
                List<Menu> Menus = new List<Menu>();
                DbCommand cmd = DBAccessManager.SqlDBAccess.CreateCommand(CommandType.Text, sql);
                try
                {
                    cmd.Parameters.Add(DBAccessManager.SqlDBAccess.CreateParameter("@UserID", userId, ParameterDirection.Input));
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
                                Category = (string)reader[6]
                            });
                        }
                    }
                }
                catch (Exception ex) { ExceptionManager.Publish(ex); }
                return Menus;
            }, CacheSection.RetrieveDescByKey(RetrieveMenusByUserIDDataKey));
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public static IEnumerable<Menu> RetrieveNavigationsByUserId(int userId)
        {
            var navs = (userId == 0 ? RetrieveMenus() : RetrieveMenusByUserId(userId)).Where(m => m.Category == "0");
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
        /// 
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public static IEnumerable<Menu> RetrieveLinksByUserId(int userId)
        {
            var navs = (userId == 0 ? RetrieveMenus() : RetrieveMenusByUserId(userId)).Where(m => m.Category == "1");
            var root = navs.Where(m => m.ParentId == 0).OrderBy(m => m.Order);
            CascadeMenu(navs, root);
            return root;
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
                CacheCleanUtility.ClearCache(menuIds: p.ID == 0 ? "" : p.ID.ToString());
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
                string sql = "select n.ID,n.ParentId, n.Name,n.[Order],n.Icon,n.Url,n.Category, case nr.NavigationID when n.ID then 'active' else '' end [status] from Navigations n left join NavigationRole nr on n.ID = nr.NavigationID and RoleID = @RoleID";
                DbCommand cmd = DBAccessManager.SqlDBAccess.CreateCommand(CommandType.Text, sql);
                cmd.Parameters.Add(DBAccessManager.SqlDBAccess.CreateParameter("@RoleID", roleId, ParameterDirection.Input));
                try
                {
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
                                Active = (string)reader[7] == "" ? "" : "checked"
                            });
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

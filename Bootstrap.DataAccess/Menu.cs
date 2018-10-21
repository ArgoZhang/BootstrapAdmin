using Bootstrap.Security;
using Longbow;
using Longbow.Cache;
using Longbow.Configuration;
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
        /// 
        /// </summary>
        public const string RetrieveMenusByRoleIdDataKey = "MenuHelper-RetrieveMenusByRoleId";
        public const string RetrieveMenusDataKey = "BootstrapMenu-RetrieveMenusByUserName";
        public const string RetrieveMenusAll = "BootstrapMenu-RetrieveMenus";
        /// <summary>
        /// 删除菜单信息
        /// </summary>
        /// <param name="value"></param>
        public virtual bool DeleteMenu(IEnumerable<int> value)
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
            string sql = p.Id == 0 ?
                "Insert Into Navigations (ParentId, Name, [Order], Icon, Url, Category, Target, IsResource, [Application]) Values (@ParentId, @Name, @Order, @Icon, @Url, @Category, @Target, @IsResource, @ApplicationCode)" :
                "Update Navigations set ParentId = @ParentId, Name = @Name, [Order] = @Order, Icon = @Icon, Url = @Url, Category = @Category, Target = @Target, IsResource = @IsResource, Application = @ApplicationCode where ID = @ID";
            using (DbCommand cmd = DbAccessManager.DBAccess.CreateCommand(CommandType.Text, sql))
            {
                cmd.Parameters.Add(DbAccessManager.DBAccess.CreateParameter("@ID", p.Id));
                cmd.Parameters.Add(DbAccessManager.DBAccess.CreateParameter("@ParentId", p.ParentId));
                cmd.Parameters.Add(DbAccessManager.DBAccess.CreateParameter("@Name", p.Name));
                cmd.Parameters.Add(DbAccessManager.DBAccess.CreateParameter("@Order", p.Order));
                cmd.Parameters.Add(DbAccessManager.DBAccess.CreateParameter("@Icon", DbAccessFactory.ToDBValue(p.Icon)));
                cmd.Parameters.Add(DbAccessManager.DBAccess.CreateParameter("@Url", DbAccessFactory.ToDBValue(p.Url)));
                cmd.Parameters.Add(DbAccessManager.DBAccess.CreateParameter("@Category", p.Category));
                cmd.Parameters.Add(DbAccessManager.DBAccess.CreateParameter("@Target", p.Target));
                cmd.Parameters.Add(DbAccessManager.DBAccess.CreateParameter("@IsResource", p.IsResource));
                cmd.Parameters.Add(DbAccessManager.DBAccess.CreateParameter("@ApplicationCode", p.ApplicationCode));
                ret = DbAccessManager.DBAccess.ExecuteNonQuery(cmd) == 1;
            }
            CacheCleanUtility.ClearCache(menuIds: p.Id == 0 ? new List<int>() : new List<int>() { p.Id });
            return ret;
        }

        /// <summary>
        /// 查询某个角色所配置的菜单
        /// </summary>
        /// <param name="roleId"></param>
        /// <returns></returns>
        public virtual IEnumerable<BootstrapMenu> RetrieveMenusByRoleId(int roleId)
        {
            string key = string.Format("{0}-{1}", RetrieveMenusByRoleIdDataKey, roleId);
            return CacheManager.GetOrAdd(key, k =>
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
                                Id = (int)reader[0]
                            });
                        }
                    }
                }
                return menus;
            }, RetrieveMenusByRoleIdDataKey);
        }
        /// <summary>
        /// 通过角色ID保存当前授权菜单
        /// </summary>
        /// <param name="id"></param>
        /// <param name="menuIds"></param>
        /// <returns></returns>
        public virtual bool SaveMenusByRoleId(int id, IEnumerable<int> menuIds)
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
        /// <summary>
        /// 通过当前用户名获得所有菜单，层次化后集合
        /// </summary>
        /// <param name="userName">当前登陆的用户名</param>
        /// <param name="activeUrl">当前访问菜单</param>
        /// <param name="connName">连接字符串名称，默认为ba</param>
        /// <returns></returns>
        public virtual IEnumerable<BootstrapMenu> RetrieveAllMenus(string userName, string activeUrl = null)
        {
            var menus = RetrieveMenusByUserName(userName, activeUrl);
            var root = menus.Where(m => m.ParentId == 0).OrderBy(m => m.Category).ThenBy(m => m.ApplicationCode).ThenBy(m => m.Order);
            CascadeMenus(menus, root);
            return root;
        }
        /// <summary>
        /// 通过当前用户名获得前台菜单，层次化后集合
        /// </summary>
        /// <param name="userName">当前登陆的用户名</param>
        /// <param name="activeUrl">当前访问菜单</param>
        /// <param name="connName">连接字符串名称，默认为ba</param>
        /// <returns></returns>
        public virtual IEnumerable<BootstrapMenu> RetrieveAppMenus(string userName, string activeUrl = null)
        {
            var menus = RetrieveMenusByUserName(userName, activeUrl).Where(m => m.Category == "1" && m.IsResource == 0);
            var root = menus.Where(m => m.ParentId == 0).OrderBy(m => m.ApplicationCode).ThenBy(m => m.Order);
            CascadeMenus(menus, root);
            return root;
        }
        /// <summary>
        /// 通过当前用户名获得所有菜单
        /// </summary>
        /// <param name="userName">当前登陆的用户名</param>
        /// <param name="activeUrl">当前访问菜单</param>
        /// <returns></returns>
        public virtual IEnumerable<BootstrapMenu> RetrieveMenusByUserName(string userName, string activeUrl = null)
        {
            // TODO: 考虑第三方应用获取
            var appId = LgbConvert.ReadValue(ConfigurationManager.AppSettings["AppId"], "0");
            var key = string.Format("{0}-{1}-{2}", RetrieveMenusDataKey, userName, appId);
            var navs = CacheManager.GetOrAdd(key, k =>
            {
                var menus = RetrieveAllMenus(userName);
                return appId == "0" ? menus : menus.Where(m => m.ApplicationCode == appId);
            }, RetrieveMenusDataKey);
            if (!string.IsNullOrEmpty(activeUrl)) ActiveMenu(null, navs, activeUrl);
            return navs;
        }
        /// <summary>
        /// 通过当前用户名获得后台菜单，层次化后集合
        /// </summary>
        /// <param name="db"></param>
        /// <param name="userName">当前登陆的用户名</param>
        /// <param name="activeUrl">当前访问菜单</param>
        /// <param name="connName">连接字符串名称，默认为ba</param>
        /// <returns></returns>
        public virtual IEnumerable<BootstrapMenu> RetrieveSystemMenus(string userName, string activeUrl = null)
        {
            var menus = RetrieveMenusByUserName(userName, activeUrl).Where(m => m.Category == "0" && m.IsResource == 0);
            var root = menus.Where(m => m.ParentId == 0).OrderBy(m => m.ApplicationCode).ThenBy(m => m.Order);
            CascadeMenus(menus, root);
            return root;
        }

        private static IEnumerable<BootstrapMenu> RetrieveAllMenus(string userName)
        {
            return CacheManager.GetOrAdd(RetrieveMenusAll, k =>
            {
                var menus = new List<BootstrapMenu>();
                var db = DbAccessManager.DBAccess;
                using (DbCommand cmd = db.CreateCommand(CommandType.Text, "select n.ID, n.ParentId, n.Name, n.[Order], n.Icon, n.Url, n.Category, n.Target, n.IsResource, n.[Application], d.Name as CategoryName, ln.Name as ParentName from Navigations n inner join Dicts d on n.Category = d.Code and d.Category = @Category and d.Define = 0 left join Navigations ln on n.ParentId = ln.ID inner join (select nr.NavigationID from Users u inner join UserRole ur on ur.UserID = u.ID inner join NavigationRole nr on nr.RoleID = ur.RoleID where u.UserName = @UserName union select nr.NavigationID from Users u inner join UserGroup ug on u.ID = ug.UserID inner join RoleGroup rg on rg.GroupID = ug.GroupID inner join NavigationRole nr on nr.RoleID = rg.RoleID where u.UserName = @UserName union select n.ID from Navigations n where EXISTS (select UserName from Users u inner join UserRole ur on u.ID = ur.UserID inner join Roles r on ur.RoleID = r.ID where u.UserName = @UserName and r.RoleName = @RoleName)) nav on n.ID = nav.NavigationID"))
                {
                    cmd.Parameters.Add(db.CreateParameter("@UserName", userName));
                    cmd.Parameters.Add(db.CreateParameter("@Category", "菜单"));
                    cmd.Parameters.Add(db.CreateParameter("@RoleName", "Administrators"));
                    using (DbDataReader reader = db.ExecuteReader(cmd))
                    {
                        while (reader.Read())
                        {
                            menus.Add(new BootstrapMenu
                            {
                                Id = LgbConvert.ReadValue(reader[0], 0),
                                ParentId = LgbConvert.ReadValue(reader[1], 0),
                                Name = (string)reader[2],
                                Order = LgbConvert.ReadValue(reader[3], 0),
                                Icon = reader.IsDBNull(4) ? string.Empty : (string)reader[4],
                                Url = reader.IsDBNull(5) ? string.Empty : (string)reader[5],
                                Category = (string)reader[6],
                                Target = (string)reader[7],
                                IsResource = LgbConvert.ReadValue(reader[8], false) ? 1 : 0,
                                ApplicationCode = reader.IsDBNull(9) ? string.Empty : (string)reader[9],
                                CategoryName = (string)reader[10],
                                ParentName = reader.IsDBNull(11) ? string.Empty : (string)reader[11],
                            });
                        }
                    }
                }
                return menus;
            });
        }

        private static void CascadeMenus(IEnumerable<BootstrapMenu> navs, IEnumerable<BootstrapMenu> level)
        {
            level.ToList().ForEach(m =>
            {
                m.Menus = navs.Where(sub => sub.ParentId == m.Id).OrderBy(sub => sub.Order);
                CascadeMenus(navs, m.Menus);
            });
        }

        private static void ActiveMenu(BootstrapMenu parent, IEnumerable<BootstrapMenu> menus, string url)
        {
            if (menus == null || !menus.Any()) return;
            menus.AsParallel().ForAll(m =>
            {
                m.Active = m.Url.Equals(url, StringComparison.OrdinalIgnoreCase) ? "active" : "";
                ActiveMenu(m, m.Menus, url);
                if (parent != null && m.Active != "") parent.Active = m.Active;
            });
        }
    }
}

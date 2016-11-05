using Longbow;
using Longbow.Caching;
using Longbow.Caching.Configuration;
using Longbow.Data;
using Longbow.ExceptionManagement;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Globalization;
using System.Linq;

namespace Bootstrap.DataAccess
{
    public static class MenuHelper
    {
        internal const string RetrieveMenusDataKey = "MenuHelper-RetrieveMenus";
        internal const string RetrieveMenusByUserIDDataKey = "MenuHelper-RetrieveMenusByUserId";
        /// <summary>
        /// 查询所有菜单信息
        /// </summary>
        /// <param name="tId"></param>
        /// <returns></returns>
        public static IEnumerable<Menu> RetrieveMenus(string tId = null)
        {
            string sql = "select n.*, d.Name as CategoryName from Navigations n inner join Dicts d on n.Category = d.Code and d.Category = N'菜单' and d.Define = 0";
            var ret = CacheManager.GetOrAdd(RetrieveMenusDataKey, CacheSection.RetrieveIntervalByKey(RetrieveMenusDataKey), key =>
            {
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
                                CategoryName = (string)reader[7]
                            });
                        }
                    }
                }
                catch (Exception ex) { ExceptionManager.Publish(ex); }
                return Menus;
            }, CacheSection.RetrieveDescByKey(RetrieveMenusDataKey));
            return string.IsNullOrEmpty(tId) ? ret : ret.Where(t => tId.Equals(t.ID.ToString(), StringComparison.OrdinalIgnoreCase));
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
                string sql = "select distinct n.* from UserRole ur,NavigationRole nr,Navigations n where ur.RoleID=nr.RoleID and nr.NavigationID=n.ID and ur.UserID = @UserID";
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
        /// 删除菜单信息
        /// </summary>
        /// <param name="ids"></param>
        public static bool DeleteMenu(string ids)
        {
            bool ret = false;
            if (string.IsNullOrEmpty(ids) || ids.Contains("'")) return ret;
            try
            {
                string sql = string.Format(CultureInfo.InvariantCulture, "Delete from Navigations where ID in ({0})", ids);
                using (DbCommand cmd = DBAccessManager.SqlDBAccess.CreateCommand(CommandType.Text, sql))
                {
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
    }
}
